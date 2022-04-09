using AutoMapper;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EipqLibrary.Infrastructure.Business.Services
{
    public class BookService : BaseService, IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task DeleteAsync(int bookId)
        {
            await _unitOfWork.BookRepository.DeleteAsync(bookId);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<BookModel>> GetAllAsync()
        {
            var books = await _unitOfWork.BookRepository.GetAllWithIncludeAsync(x => x.Category);

            return _mapper.Map<List<BookModel>>(books);
        }

        public async Task<BookModel> GetByIdAsync(int bookId)
        {
            var book = await _unitOfWork.BookRepository.GetByIdWithIncludeAsync(bookId, x => x.Category);
            EnsureExists(book, $"Book with id {bookId} was not found");

            return _mapper.Map<BookModel>(book);
        }

        public async Task<BookModel> GetByNameAndAuthorAsync(string name, string author)
        {
            var book = await _unitOfWork.BookRepository.GetFirstWithIncludeAsync(x => x.Name == name && x.Author == author, x => x.Category);

            return book == null ? null : _mapper.Map<BookModel>(book);
        }

        public async Task<BookModel> UpdateAsync(BookUpdateRequest updateRequest)
        {
            var existingBook = await _unitOfWork.BookRepository.GetByIdAsync(updateRequest.Id);
            EnsureExists(existingBook, $"Book with id {updateRequest.Id} does not exist");

            _mapper.Map(updateRequest, existingBook);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BookModel>(existingBook);
        }
    }
}
