using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Infrastructure.Data.Repositories.Common;

namespace EipqLibrary.Infrastructure.Data.Repositories
{
    public class BookInstanceRepository : BaseRepository<BookInstance>, IBookInstanceRepository
    {
        public BookInstanceRepository(EipqLibraryDbContext context) : base(context)
        {
        }
    }
}
