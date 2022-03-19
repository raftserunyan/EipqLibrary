using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Infrastructure.Data.Utils.Extensions;
using EipqLibrary.Infrastructure.Data.Utils.Extensions.ObsoletExtensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EipqLibrary.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EipqLibraryDbContext _context;
        public UserRepository(EipqLibraryDbContext context)
        {
            _context = context;
        }

        public async Task<PagedData<User>> GetAllAsync(PageInfo pageInfo, UserSortOption userSort, UserStatus? status = null)
        {
            return await _context.Students
                .Include(x => x.Group)
                .FilterUsersByStatus(status)
                .SortUsers(userSort)
                .Paged(pageInfo);
        }
    }
}
