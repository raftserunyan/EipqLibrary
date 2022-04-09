using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using System.Threading.Tasks;

namespace EipqLibrary.Services.Interfaces.ServiceInterfaces
{
    public interface IBookCreationRequestService
    {
        Task<int> CreateBookAdditionRequestAsync(BookAdditionRequest bookAdditionRequest);
        Task AddAccountantAction(BookCreationRequestAccountantAction accountantAction);
        Task<BookCreationRequestModel> UpdateAsync(UpdateBookCreationRequest updateRequest);
        Task<PagedData<BookCreationRequestModel>> GetAllAsync(PageInfo pageInfo, BCRSortOption bcrSort, BookCreationRequestStatus? status);
        Task<BookCreationRequestModel> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
