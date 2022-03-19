using EipqLibrary.Domain.Interfaces.EFInterfaces;
using System.Linq;
using System.Threading.Tasks;

namespace EipqLibrary.Infrastructure.Data.Repositories
{
    internal class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly EipqLibraryDbContext _context;

        public RefreshTokenRepository(EipqLibraryDbContext context)
        {
            _context = context;
        }

        public async Task RemoveByAdminId(string adminId)
        {
            _context.AdminRefreshTokens.RemoveRange(_context.AdminRefreshTokens.Where(rt => rt.UserId.Equals(adminId)));

            await _context.SaveChangesAsync();
        }
    }
}
