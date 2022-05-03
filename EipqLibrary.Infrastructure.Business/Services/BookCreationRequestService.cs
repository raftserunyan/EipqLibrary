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
            EnsureExists(request, $"Նշված գրքի ստեղծման հայտը չի գտնվել․ Id = {accountantAction.RequestId}");

            if (request.RequestStatus != BookCreationRequestStatus.Pending)
            {
                throw BadRequest("Հաշվապահը արդեն կատարել է գործողություն տվյալ հայտի հետ");
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
                throw BadRequest($"Նշված կատեգորիան չի գտնվել․ Id = {bookAdditionRequest.CategoryId}");
            }

            var bookCreationRequest = _mapper.Map<BookCreationRequest>(bookAdditionRequest);

            await _uow.BookCreationRequestRepository.AddAsync(bookCreationRequest);
            await _uow.SaveChangesAsync();

            return bookCreationRequest.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _uow.BookCreationRequestRepository.GetByIdAsync(id);
            EnsureExists(entity, $"Նշված գրքի ստեղծման հայտը չի գտնվել․ Id = {id}");

            if (entity.RequestStatus != Domain.Core.Enums.BookCreationRequestStatus.Pending)
            {
                throw BadRequest("Դուք չեք կարող ջնջել հայտը որը արդեն հաստատվել կամ մերժվել է");
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
            EnsureExists(request, $"Նշված գրքի ստեղծման հայտը չի գտնվել․ Id = {id}");

            return _mapper.Map<BookCreationRequestModel>(request);
        }

        public async Task<BookCreationRequestModel> UpdateAsync(UpdateBookCreationRequest updateRequest)
        {
            var entity = await _uow.BookCreationRequestRepository.GetByIdAsync(updateRequest.Id);
            EnsureExists(entity, $"Նշված գրքի ստեղծման հայտը չի գտնվել․ Id = {updateRequest.Id}");

            if (entity.RequestStatus != Domain.Core.Enums.BookCreationRequestStatus.Pending)
            {
                throw BadRequest("Դուք չեք կարող փոփոխել հայտը որը արդեն հաստատվել կամ մերժվել է");
            }

            var category = await _uow.CategoryRepository.GetByIdAsync(updateRequest.CategoryId);
            EnsureExists(category, $"Նշված կատեգորիան չի գտնվել․ Id = {updateRequest.CategoryId}");

            _mapper.Map(updateRequest, entity);
            entity.RequestLastUpdatedDate = System.DateTime.Now;
            await _uow.SaveChangesAsync();

            return _mapper.Map<BookCreationRequestModel>(entity);
        }
    }
}
