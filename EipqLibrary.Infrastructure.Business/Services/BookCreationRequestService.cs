using AutoMapper;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using EipqLibrary.Shared.CustomExceptions;
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

        public async Task<int> CreateBookAdditionRequestAsync(BookAdditionRequest bookAdditionRequest)
        {
            if (!await _uow.CategoryRepository.ExistsAsync(x => x.Id == bookAdditionRequest.CategoryId))
            {
                throw new EntityNotFoundException("Category with id {bookAdditionRequest.CategoryId} does not exist");
            }

            var bookCreationRequest = _mapper.Map<BookCreationRequest>(bookAdditionRequest);

            await _uow.BookCreationRequestRepository.AddAsync(bookCreationRequest);
            await _uow.SaveChangesAsync();

            return bookCreationRequest.Id;
        }

        public async Task<IEnumerable<BookCreationRequestModel>> GetAllAsync()
        {
            var requests = await _uow.BookCreationRequestRepository.GetAllWithIncludeAsync(x => x.Category);

            return _mapper.Map<IEnumerable<BookCreationRequestModel>>(requests);
        }

        public async Task<BookCreationRequestModel> GetByIdAsync(int id)
        {
            var request = await _uow.BookCreationRequestRepository.GetByIdWithIncludeAsync(id, x => x.Category);

            return _mapper.Map<BookCreationRequestModel>(request);
        }
    }
}
