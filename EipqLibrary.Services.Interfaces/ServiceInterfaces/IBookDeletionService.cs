using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using System.Threading.Tasks;

namespace EipqLibrary.Services.Interfaces.ServiceInterfaces
{
    public interface IBookDeletionService
    {
        Task<BookDeletionRequest> CreateAsync(BookDeletionRequestDto requestDto);
        Task AddAccountantAction(BookDeletionRequestAccountantAction accountantAction);
        Task<BookDeletionRequestModel> UpdateAsync(UpdateBookDeletionRequest updateRequest);
        Task<PagedData<BookDeletionRequestModel>> GetAllAsync(PageInfo pageInfo, BDRSortOption bdrSort, BookDeletionRequestStatus? status);
        Task<BookDeletionRequestModel> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
