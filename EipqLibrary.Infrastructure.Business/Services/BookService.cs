using AutoMapper;
using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using System;
using System.Linq;
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
            EnsureExists(book, $"Նշված գիրքը չի գտնվել․ Id = {bookId}");

            return _mapper.Map<BookModel>(book);
        }

        public async Task<BookModel> GetByNameAndAuthorAsync(string name, string author)
        {
            var book = await _unitOfWork.BookRepository.GetFirstWithIncludeAsync(x => x.Name == name && x.Author == author, x => x.Category);

            return book == null ? null : _mapper.Map<BookModel>(book);
        }

        public async Task<BookModel> UpdateAsync(BookUpdateRequest updateRequest)
        {
            var existingBook = await _unitOfWork.BookRepository.GetByIdIncludingInstancesAndTheirBorrowings(updateRequest.Id);
            EnsureExists(existingBook, $"Նշված գիրքը չի գտնվել․ Id = {updateRequest.Id}");

            if (!await _unitOfWork.CategoryRepository.ExistsAsync(x => x.Id == updateRequest.CategoryId))
            {
                throw BadRequest($"Նշված կատեգորիան չի գտնվել Id = {updateRequest.CategoryId}");
            }
            if (existingBook.TotalCount != updateRequest.Quantity)
            {
                throw BadRequest("Դուք չեք կարող փոփոխել գրքի ընդհանուր քանակը");
            }
            if (updateRequest.AvailableForBorrowingCount != existingBook.AvailableForBorrowingCount)
            {
                CheckIfTheCountsCanBeChanged(updateRequest, existingBook);
            }

            var difference = existingBook.AvailableForBorrowingCount - updateRequest.AvailableForBorrowingCount;

            _mapper.Map(updateRequest, existingBook);

            if (difference < 0)
            {
                difference = Math.Abs(difference);
                for (int i = 0; i < difference; i++)
                {
                    existingBook.Instances.Add(new BookInstance());
                }
            }
            else if (difference > 0)
            {
                int i = 0;
                foreach (var instance in existingBook.Instances.Where(x => x.CanBeRemovedFromBorrowablesList()))
                {
                    existingBook.Instances.Remove(instance);
                    i++;
                    if (i == difference)
                    {
                        break;
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BookModel>(existingBook);
        }

        // Private methods
        private void CheckIfTheCountsCanBeChanged(BookUpdateRequest updateRequest, Book existingBook)
        {
            var difference = existingBook.AvailableForBorrowingCount - updateRequest.AvailableForBorrowingCount;
            if (difference > 0)
            {
                var removeableBorrowableInstancesCount = existingBook.Instances.Where(x => x.CanBeRemovedFromBorrowablesList()).Count();
                if (removeableBorrowableInstancesCount < difference)
                {
                    if (removeableBorrowableInstancesCount == 0)
                    {
                        throw BadRequest($"Բոլոր վերցնելու համար հասանելի գրքերը զբաղված են կամ ամրագրված");
                    }
                    throw BadRequest($"Վերցնելու համար նախատեսված գրքերից կարող է հեռացվել միայն {removeableBorrowableInstancesCount} հատ");
                }
            }
        }
    }
}
