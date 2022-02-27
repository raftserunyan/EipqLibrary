using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Infrastructure.Data.Repositories.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EipqLibrary.Infrastructure.Data.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(EipqLibraryDbContext context) : base(context)
        {
        }

        public async Task<Book> GetByIdForStudentAsync(int bookId)
        {
            return await GetFirstWithIncludeAsync(x => x.Id == bookId && x.Status == BookStatus.Available,
                                                    x => x.Category);
        }

        public async Task<List<Book>> GetAllForStudentAsync()
        {
            return await GetAllWithIncludeAsync(x => x.Status == BookStatus.Available,
                                                    x => x.Category);
        }
    }
}
