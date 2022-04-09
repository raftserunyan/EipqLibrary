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
    public class BookCreationRequestRepository : BaseRepository<BookCreationRequest>, IBookCreationRequestRepository
    {
        public BookCreationRequestRepository(EipqLibraryDbContext context) : base(context)
        {
        }

        public async Task<PagedData<BookCreationRequest>> GetAllSortedAndPagedAsync(PageInfo pageInfo, BCRSortOption bcrSort, BookCreationRequestStatus? status)
        {
            return await _context.BookCreationRequests
                .Include(x => x.Category)
                .FilterBCRsByStatus(status)
                .SortRequests(bcrSort)
                .Paged(pageInfo);
        }
    }
}
