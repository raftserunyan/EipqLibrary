using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Services.DTOs.Models.Tokens;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using EipqLibrary.Shared.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EipqLibrary.Infrastructure.Business.Services
{
    public class AdminRefreshTokenService : IAdminRefreshTokenService
    {
        private readonly TokenSettings _tokenSettings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AdminRefreshTokenService(TokenSettings tokenSettings,
                                        IUnitOfWork unitOfWork, 
                                        IRefreshTokenRepository refreshTokenRepository)
        {
            _tokenSettings = tokenSettings;
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task RemoveAllForAdminId(string adminId)
        {
            await _refreshTokenRepository.RemoveByAdminId(adminId);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<RefreshTokenInfo> SubstituteWithNew(string userId, string deviceId, string oldRefreshToken)
        {
            await RemoveForDeviceIfExists(deviceId, oldRefreshToken);

            var token = new AdminRefreshToken()
            {
                Token = Guid.NewGuid().ToString(),
                UserId = userId,
                DeviceId = deviceId,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(_tokenSettings.AdminRefreshTokenLifetimeInDays)
            };

            await _refreshTokenRepository.Add(token);
            await _unitOfWork.SaveChangesAsync();

            return new RefreshTokenInfo()
            {
                Token = token.Token,
                ExpiryDate = token.ExpiryDate
            };
        }

        public async Task<bool> ExistsUnexpiredForDevice(string refreshToken, string deviceId)
        {
            var token = await _refreshTokenRepository.GetByTokenAndDeviceId(refreshToken, deviceId);

            if (token == null)
            {
                return false;
            }

            return DateTime.UtcNow < token.ExpiryDate;
        }

        public async Task<int> ActiveTokensCountForAdminId(string adminId)
        {
            var allTokens = await _refreshTokenRepository.GetAllTokensByAdminId(adminId);
            return allTokens.Where(x => x.ExpiryDate > DateTime.UtcNow).Count();
        }

        // Private methods
        private async Task RemoveForDeviceIfExists(string deviceId, string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return;
            }

            var token = await _refreshTokenRepository.GetByTokenAndDeviceId(refreshToken, deviceId);
            if (token != null)
            {
                _refreshTokenRepository.Remove(token);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
