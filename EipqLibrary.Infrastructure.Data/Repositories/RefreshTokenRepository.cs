using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public async Task<AdminRefreshToken> GetByTokenAndDeviceId(string refreshToken, string deviceId)
        {
            return await _context.AdminRefreshTokens.FirstOrDefaultAsync(x =>
                x.Token == refreshToken &&
                x.DeviceId == deviceId);
        }

        public async Task Add(AdminRefreshToken refreshToken)
        {
            await _context.AdminRefreshTokens.AddAsync(refreshToken);
        }

        public void Remove(AdminRefreshToken refreshToken)
        {
            _context.AdminRefreshTokens.Remove(refreshToken);
        }

        public async Task<IList<AdminRefreshToken>> GetAllTokensByAdminId(string adminId)
        {
            return await _context.AdminRefreshTokens.Where(x => x.UserId == adminId).ToListAsync();
        }
    }
}
