using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Infrastructure.Data.Repositories.Common;

namespace EipqLibrary.Infrastructure.Data.Repositories
{
    public class BookDeletionRequestRepository : BaseRepository<BookDeletionRequest>, IBookDeletionRequestRepository
    {
        public BookDeletionRequestRepository(EipqLibraryDbContext context) : base(context)
        {
        }
    }
}
