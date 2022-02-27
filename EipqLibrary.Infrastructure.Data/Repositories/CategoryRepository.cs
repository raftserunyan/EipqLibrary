using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Infrastructure.Data.Repositories.Common;

namespace EipqLibrary.Infrastructure.Data.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(EipqLibraryDbContext context) : base(context)
        {
        }
    }
}
