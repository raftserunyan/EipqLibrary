using AutoMapper;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EipqLibrary.Infrastructure.Business.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> CreateBook(BookCreationRequest bookCreationRequest)
        {
            var book = await _unitOfWork.BookRepository.GetFirstWithIncludeAsync(b => b.Name == bookCreationRequest.Name && b.Author == bookCreationRequest.Author);
            if (book == null)
            {
                book = _mapper.Map<Book>(bookCreationRequest);
                await _unitOfWork.BookRepository.AddAsync(book);
            }
            else
            {
                book.TotalCount += bookCreationRequest.TotalCount;
            }

            await _unitOfWork.SaveChangesAsync();

            return book.Id;
        }

        public async Task<List<BookModel>> GetAllAsync()
        {
            var books = await _unitOfWork.BookRepository.GetAllForStudentAsync();

            return _mapper.Map<List<BookModel>>(books);
        }

        public async Task<BookModel> GetByIdAsync(int bookId)
        {
            var books = await _unitOfWork.BookRepository.GetByIdForStudentAsync(bookId);

            return _mapper.Map<BookModel>(books);
        }
    }
}
