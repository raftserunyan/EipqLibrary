using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Infrastructure.Data.Repositories.Common;
using EipqLibrary.Infrastructure.Data.Utils.Extensions;
using EipqLibrary.Infrastructure.Data.Utils.Extensions.ObsoletExtensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EipqLibrary.Infrastructure.Data.Repositories
{
    public class BookDeletionRequestRepository : BaseRepository<BookDeletionRequest>, IBookDeletionRequestRepository
    {
        public BookDeletionRequestRepository(EipqLibraryDbContext context) : base(context)
        {
        }

        public async Task<PagedData<BookDeletionRequest>> GetAllSortedAndPagedAsync(PageInfo pageInfo, BDRSortOption requestSort, BookDeletionRequestStatus? status)
        {
            return await _context.BookDeletionRequests
                .Include(x => x.Book)
                .FilterBDRsByStatus(status)
                .SortRequests(requestSort)
                .Paged(pageInfo);
        }
    }
}
