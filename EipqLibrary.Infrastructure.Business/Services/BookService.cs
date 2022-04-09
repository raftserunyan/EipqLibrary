﻿using AutoMapper;
using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
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

        public async Task<PagedData<BookModel>> GetAllAsync(PageInfo pageInfo, int? categoryId, string author)
        {
            var books = await _unitOfWork.BookRepository.GetAllFilteredAndPagedAsync(pageInfo, categoryId, author);

            return _mapper.Map<PagedData<BookModel>>(books);
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
            if (!await _unitOfWork.CategoryRepository.ExistsAsync(x => x.Id == updateRequest.CategoryId))
            {
                throw BadRequest($"Category with ID {updateRequest.CategoryId} does not exist");
            }

            var existingBook = await _unitOfWork.BookRepository.GetByIdAsync(updateRequest.Id);
            EnsureExists(existingBook, $"Book with id {updateRequest.Id} does not exist");

            if (existingBook.TotalCount != updateRequest.Quantity)
            {
                throw BadRequest("You cannot change the total quantity of a book");
            }
            _mapper.Map(updateRequest, existingBook);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BookModel>(existingBook);
        }
    }
}
