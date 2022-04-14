using AutoMapper;
using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EipqLibrary.Infrastructure.Business.Services
{
    public class BookCreationRequestService : BaseService, IBookCreationRequestService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public BookCreationRequestService(IUnitOfWork uow,
                                          IMapper mapper)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task AddAccountantAction(BookCreationRequestAccountantAction accountantAction)
        {
            var request = await _uow.BookCreationRequestRepository.GetByIdAsync(accountantAction.RequestId);
            EnsureExists(request, $"BookCreationRequest with id {accountantAction.RequestId} does not exist");

            if (request.RequestStatus != BookCreationRequestStatus.Pending)
            {
                throw BadRequest("Accountant already acted on this request");
            }

            if (accountantAction.AccountantActionResult == BookCreationRequestStatus.Approved)
            {
                var book = await _uow.BookRepository.GetFirstAsync(x => x.Name == request.Name && x.Author == request.Author);
                if (book == null)
                {
                    book = _mapper.Map<Book>(request);
                    book.TotalCount = request.Quantity;
                    book.AvailableForBorrowingCount = request.AvailableForBorrowingCount;
                    book.AvailableForUsingInLibraryCount = request.AvailableForUsingInLibraryCount;

                    await _uow.BookRepository.AddAsync(book);
                }
                else
                {
                    book.ProductionYear = request.ProductionYear;
                    book.Description = request.Description;
                    book.PagesCount = request.PagesCount;
                    book.PagesCount = request.PagesCount;

                    book.TotalCount += request.Quantity;
                    book.AvailableForBorrowingCount += request.AvailableForBorrowingCount;
                    book.AvailableForUsingInLibraryCount += request.AvailableForUsingInLibraryCount;
                }

                book.Instances = new List<BookInstance>();
                for (int i = 0; i < request.AvailableForBorrowingCount; i++)
                {
                    book.Instances.Add(new BookInstance());
                }

                request.RequestStatus = BookCreationRequestStatus.Approved;
            }
            else
            {
                request.RequestStatus = BookCreationRequestStatus.Rejected;
            }

            request.AccountantActionDate = System.DateTime.Now;
            request.AccountantNote = accountantAction.AccountantMessage;
            request.RequestStatus = accountantAction.AccountantActionResult;
            await _uow.SaveChangesAsync();
        }

        public async Task<int> CreateBookAdditionRequestAsync(BookAdditionRequest bookAdditionRequest)
        {
            if (!await _uow.CategoryRepository.ExistsAsync(x => x.Id == bookAdditionRequest.CategoryId))
            {
                throw BadRequest($"Category with id {bookAdditionRequest.CategoryId} does not exist");
            }

            var bookCreationRequest = _mapper.Map<BookCreationRequest>(bookAdditionRequest);

            await _uow.BookCreationRequestRepository.AddAsync(bookCreationRequest);
            await _uow.SaveChangesAsync();

            return bookCreationRequest.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _uow.BookCreationRequestRepository.GetByIdAsync(id);
            EnsureExists(entity, $"BookCreationRequest with ID {id} does not exist");

            if (entity.RequestStatus != Domain.Core.Enums.BookCreationRequestStatus.Pending)
            {
                throw BadRequest("You can not edit a request which is already approved/rejected");
            }

            _uow.BookCreationRequestRepository.Delete(entity);
            await _uow.SaveChangesAsync();
        }

        public async Task<PagedData<BookCreationRequestModel>> GetAllAsync(PageInfo pageInfo, BCRSortOption bcrSort, BookCreationRequestStatus? status)
        {
            var requests = await _uow.BookCreationRequestRepository.GetAllSortedAndPagedAsync(pageInfo, bcrSort, status);

            return _mapper.Map<PagedData<BookCreationRequestModel>>(requests);
        }

        public async Task<BookCreationRequestModel> GetByIdAsync(int id)
        {
            var request = await _uow.BookCreationRequestRepository.GetByIdWithIncludeAsync(id, x => x.Category);
            EnsureExists(request, $"BookCreationRequest with id {id} was not found");

            return _mapper.Map<BookCreationRequestModel>(request);
        }

        public async Task<BookCreationRequestModel> UpdateAsync(UpdateBookCreationRequest updateRequest)
        {
            var entity = await _uow.BookCreationRequestRepository.GetByIdAsync(updateRequest.Id);
            EnsureExists(entity, $"BookCreationRequest with the id {updateRequest.Id} does not exist");

            if (entity.RequestStatus != Domain.Core.Enums.BookCreationRequestStatus.Pending)
            {
                throw BadRequest("You can not edit a request which is already approved/rejected");
            }

            var category = await _uow.CategoryRepository.GetByIdAsync(updateRequest.CategoryId);
            EnsureExists(category, $"Category with ID {updateRequest.CategoryId} was not found");

            _mapper.Map(updateRequest, entity);
            entity.RequestLastUpdatedDate = System.DateTime.Now;
            await _uow.SaveChangesAsync();

            return _mapper.Map<BookCreationRequestModel>(entity);
        }
    }
}
