using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Domain.Interfaces.EFInterfaces.Common;
using System.Threading.Tasks;

namespace EipqLibrary.Domain.Interfaces.EFInterfaces
{
    public interface IBookCreationRequestRepository : IBaseRepository<BookCreationRequest>
    {
        Task<PagedData<BookCreationRequest>> GetAllSortedAndPagedAsync(PageInfo pageInfo, BCRSortOption requestSort, BookCreationRequestStatus? status);
    }
}
