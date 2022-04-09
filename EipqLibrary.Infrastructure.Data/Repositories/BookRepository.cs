using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Infrastructure.Data.Repositories.Common;
using EipqLibrary.Infrastructure.Data.Utils.Extensions;
using EipqLibrary.Infrastructure.Data.Utils.Extensions.ObsoletExtensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EipqLibrary.Infrastructure.Data.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(EipqLibraryDbContext context) : base(context)
        {
        }

        public async Task<PagedData<Book>> GetAllFilteredAndPagedAsync(PageInfo pageInfo, int? categoryId, string author)
        {
            return await _context.Books
                .Include(x => x.Category)
                .FilterBooksByCategory(categoryId)
                .FilterBooksByAuthor(author)
                .SortBooks()
                .Paged(pageInfo);
        }
    }
}
