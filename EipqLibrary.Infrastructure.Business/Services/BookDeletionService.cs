using AutoMapper;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EipqLibrary.Infrastructure.Business.Services
{
    public class BookDeletionService : BaseService, IBookDeletionService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public BookDeletionService(IUnitOfWork uow,
                                   IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        //TODO: Test This
        public async Task<BookDeletionRequest> CreateAsync(BookDeletionRequestDto requestDto)
        {
            var book = await _uow.BookRepository.GetByIdIncludingInstancesAndTheirBorrowings(requestDto.BookId);
            EnsureExists(book);

            if (requestDto.Count > book.TotalCount)
            {
                throw BadRequest("The count you've specified is exceeding the count of books in the library");
            }

            BookDeletionRequest deletionRequest = _mapper.Map<BookDeletionRequest>(requestDto);

            // If we have to remove from the borrowables too
            if (requestDto.Count > book.AvailableForUsingInLibraryCount)
            {
                // Trying to remove from borrowable books
                int borrowableBooksDeletingCount = requestDto.Count - book.AvailableForUsingInLibraryCount;
                var removeableBorrowableInstances = book.Instances.Where(x => x.CanBeRemovedFromBorrowablesList()).ToList();
                if (removeableBorrowableInstances.Count < borrowableBooksDeletingCount)
                {
                    throw BadRequest($"For the moment only {book.AvailableForUsingInLibraryCount + removeableBorrowableInstances.Count} books can be removed");
                }

                for (int i = 0; i < requestDto.Count - book.AvailableForUsingInLibraryCount; i++)
                {
                    _uow.BookInstanceRepository.Delete(removeableBorrowableInstances[i]);
                }

                deletionRequest.TemporarelyDeletedBorrowableBooksCount = borrowableBooksDeletingCount;
                deletionRequest.RequestCreationDate = DateTime.Now;

                book.AvailableForUsingInLibraryCount = 0;
            }
            else
            {
                book.AvailableForUsingInLibraryCount -= requestDto.Count;
            }

            book.TotalCount -= requestDto.Count;
            await _uow.BookDeletionRequestRepository.AddAsync(deletionRequest);

            await _uow.SaveChangesAsync();
            return deletionRequest;
        }
    }
}
