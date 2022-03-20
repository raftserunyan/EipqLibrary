using System.Threading.Tasks;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using Microsoft.EntityFrameworkCore;

namespace EipqLibrary.Infrastructure.Data.Repositories
{
    public class PublicRefreshTokenRepository : IPublicRefreshTokenRepository
    {
        private readonly EipqLibraryDbContext _context;

        public PublicRefreshTokenRepository(EipqLibraryDbContext context)
        {
            _context = context;
        }

        public async Task<PublicRefreshToken> GetByAccessTokenIdAndDeviceId(string accessTokenId, string deviceId)
        {
            return await _context.PublicRefreshTokens.FirstOrDefaultAsync(x =>
                x.AccessTokenId == accessTokenId &&
                x.DeviceId == deviceId);
        }

        public async Task<PublicRefreshToken> GetByTokenAndAccessTokenIdAndDeviceId(string refreshToken, string mainTokenId, string deviceId)
        {
            return await _context.PublicRefreshTokens.FirstOrDefaultAsync(x =>
                x.Token == refreshToken &&
                x.AccessTokenId == mainTokenId &&
                x.DeviceId == deviceId);
        }

        public async Task Add(PublicRefreshToken refreshToken)
        {
            await _context.PublicRefreshTokens.AddAsync(refreshToken);
        }

        public async Task Remove(PublicRefreshToken refreshToken)
        {
            _context.PublicRefreshTokens.Remove(refreshToken);

            await _context.SaveChangesAsync();
        }
    }
}
