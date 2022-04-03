using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Infrastructure.Data.Repositories.Common;

namespace EipqLibrary.Infrastructure.Data.Repositories
{
    public class BookCreationRequestRepository : BaseRepository<BookCreationRequest>, IBookCreationRequestRepository
    {
        public BookCreationRequestRepository(EipqLibraryDbContext context) : base(context)
        {
        }
    }
}
