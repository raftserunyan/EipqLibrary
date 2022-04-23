using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Domain.Interfaces.EFInterfaces.Common;
using System.Threading.Tasks;

namespace EipqLibrary.Domain.Interfaces.EFInterfaces
{
    public interface IBookDeletionRequestRepository : IBaseRepository<BookDeletionRequest>
    {
        Task<PagedData<BookDeletionRequest>> GetAllSortedAndPagedAsync(PageInfo pageInfo, BDRSortOption requestSort, BookDeletionRequestStatus? status);
    }
}
