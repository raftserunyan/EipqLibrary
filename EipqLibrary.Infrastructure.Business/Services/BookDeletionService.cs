using AutoMapper;
using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Services.DTOs.Models;
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

        public async Task AddAccountantAction(BookDeletionRequestAccountantAction accountantAction)
        {
            var request = await _uow.BookDeletionRequestRepository.GetByIdAsync(accountantAction.RequestId);
            EnsureExists(request, $"Նշված գրքի հեռացման հայտը չի գտնվել․ Id = {accountantAction.RequestId}");

            if (request.Status != BookDeletionRequestStatus.Pending)
            {
                throw BadRequest("Հաշվապահը արդեն կատարել է գործողություն տվյալ հայտի հետ");
            }
            if (request.BookId == null)
            {
                throw BadRequest("Տվյալ հայտում նշված գիրքը գոյություն չունի(այլևս)");
            }

            var book = await _uow.BookRepository.GetByIdWithIncludeAsync((int)request.BookId, x => x.Instances);
            EnsureExists(book, $"Նշված գիրքը չի գտնվել․ Id = {accountantAction.RequestId}");

            if (accountantAction.AccountantActionResult == BookDeletionRequestStatus.Rejected)
            {
                if (request.TemporarelyDeletedBorrowableBooksCount > 0)
                {
                    for (int i = 0; i < request.TemporarelyDeletedBorrowableBooksCount; i++)
                    {
                        book.Instances.Add(new BookInstance());
                    }
                    book.AvailableForBorrowingCount += request.TemporarelyDeletedBorrowableBooksCount;
                }

                book.AvailableForUsingInLibraryCount += request.Count - request.TemporarelyDeletedBorrowableBooksCount;
                book.TotalCount += request.Count;
            }

            request.AccountantActionDate = DateTime.Now;
            request.AccountantNote = accountantAction.AccountantMessage;
            request.Status = accountantAction.AccountantActionResult;
            await _uow.SaveChangesAsync();
        }

        public async Task<BookDeletionRequest> CreateAsync(BookDeletionRequestDto requestDto)
        {
            var book = await _uow.BookRepository.GetByIdIncludingInstancesAndTheirBorrowings(requestDto.BookId);
            EnsureExists(book);

            if (requestDto.Count > book.TotalCount)
            {
                throw BadRequest("Ձեր կողմից նշված քանակը գերազանցում է գրադարանում առկա գրքերի ընդհանուր քանակը");
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
                    throw BadRequest($"Տվյալ պահին կարող է հեռացվել միայն {book.AvailableForUsingInLibraryCount + removeableBorrowableInstances.Count} գիրք");
                }

                for (int i = 0; i < borrowableBooksDeletingCount; i++)
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

            deletionRequest.BookName = book.Name;
            deletionRequest.BookAuthor = book.Author;

            book.TotalCount -= requestDto.Count;
            await _uow.BookDeletionRequestRepository.AddAsync(deletionRequest);

            await _uow.SaveChangesAsync();
            return deletionRequest;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _uow.BookDeletionRequestRepository.GetByIdWithIncludeAsync(id);
            EnsureExists(entity, $"Նշված գրքի հեռացման հայտը չի գտնվել․ Id = {id}");

            if (entity.Status != BookDeletionRequestStatus.Pending)
            {
                throw BadRequest("Դուք չեք կարող ջնջել հայտը որը արդեն հաստատվել կամ մերժվել է");
            }

            var book = await _uow.BookRepository.GetByIdWithIncludeAsync(entity.BookId ?? 0, x => x.Instances);
            EnsureExists(book, "Տվյալ հեռացման հայտում նշված գիրքը գոյություն չունի(այլևս)");

            if (entity.TemporarelyDeletedBorrowableBooksCount > 0)
            {
                for (int i = 0; i < entity.TemporarelyDeletedBorrowableBooksCount; i++)
                {
                    book.Instances.Add(new BookInstance());
                }
                book.AvailableForBorrowingCount += entity.TemporarelyDeletedBorrowableBooksCount;
            }

            book.AvailableForUsingInLibraryCount += entity.Count - entity.TemporarelyDeletedBorrowableBooksCount;
            book.TotalCount += entity.Count;

            _uow.BookDeletionRequestRepository.Delete(entity);
            await _uow.SaveChangesAsync();
        }

        public async Task<PagedData<BookDeletionRequestModel>> GetAllAsync(PageInfo pageInfo, BDRSortOption bdrSort, BookDeletionRequestStatus? status)
        {
            var requests = await _uow.BookDeletionRequestRepository.GetAllSortedAndPagedAsync(pageInfo, bdrSort, status);

            return _mapper.Map<PagedData<BookDeletionRequestModel>>(requests);
        }

        public async Task<BookDeletionRequestModel> GetByIdAsync(int id)
        {
            var request = await _uow.BookDeletionRequestRepository.GetByIdWithIncludeAsync(id, x => x.Book);
            EnsureExists(request, $"Նշված գրքի հեռացման հայտը չի գտնվել․ Id = {id}");

            return _mapper.Map<BookDeletionRequestModel>(request);
        }

        public async Task<BookDeletionRequestModel> UpdateAsync(UpdateBookDeletionRequest updateRequest)
        {
            var entity = await _uow.BookDeletionRequestRepository.GetByIdWithIncludeAsync(updateRequest.RequestId, x => x.Book);
            EnsureExists(entity, $"Նշված գրքի հեռացման հայտը չի գտնվել․ Id = {updateRequest.RequestId}");

            if (entity.Status != BookDeletionRequestStatus.Pending)
            {
                throw BadRequest("Դուք չեք կարող փոփոխել հայտը որը արդեն հաստատվել կամ մերժվել է");
            }

            entity.Note = updateRequest.Note;
            entity.DeletionReason = updateRequest.DeletionReason;
            await _uow.SaveChangesAsync();

            return _mapper.Map<BookDeletionRequestModel>(entity);
        }
    }
}
