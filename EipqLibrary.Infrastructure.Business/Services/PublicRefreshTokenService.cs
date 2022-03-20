using System;
using System.Threading.Tasks;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Services.DTOs.Models.Tokens;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;

namespace EipqLibrary.Infrastructure.Business.Services
{
    public class PublicRefreshTokenService : BaseService, IPublicRefreshTokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublicRefreshTokenRepository _refreshTokenRepository;

        public PublicRefreshTokenService(IUnitOfWork unitOfWork, IPublicRefreshTokenRepository refreshTokenRepository)
        {
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<bool> ExistsUnexpiredForTokenAndDevice(string refreshToken, string accessTokenId, string deviceId)
        {
            var token = await _refreshTokenRepository.GetByTokenAndAccessTokenIdAndDeviceId(refreshToken, accessTokenId, deviceId);

            if (token == null)
            {
                return false;
            }

            return DateTime.UtcNow < token.ExpiryDate;
        }

        public async Task<RefreshTokenInfo> SubstituteWithNew(PublicRefreshTokenDto publicRefreshTokenDto)
        {
            await RemoveForDeviceIfExists(publicRefreshTokenDto);

            var token = new PublicRefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                AccessTokenId = publicRefreshTokenDto.AccessTokenId,
                UserId = publicRefreshTokenDto.UserId,
                DeviceId = publicRefreshTokenDto.DeviceId,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(1)
            };
            await _refreshTokenRepository.Add(token);

            await _unitOfWork.SaveChangesAsync();

            return new RefreshTokenInfo
            {
                Token = token.Token,
                ExpiryDate = token.ExpiryDate
            };
        }

        private async Task RemoveForDeviceIfExists(PublicRefreshTokenDto publicRefreshTokenDto)
        {
            if (string.IsNullOrEmpty(publicRefreshTokenDto.OldRefreshToken))
            {
                return;
            }

            var token = await _refreshTokenRepository.GetByTokenAndAccessTokenIdAndDeviceId(publicRefreshTokenDto.OldRefreshToken, publicRefreshTokenDto.OldAccessTokenId, publicRefreshTokenDto.DeviceId);
            if (token != null)
            {
                await _refreshTokenRepository.Remove(token);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task RemoveForDevice(string accessTokenId, string deviceId)
        {
            var refreshToken = await _refreshTokenRepository.GetByAccessTokenIdAndDeviceId(accessTokenId, deviceId);

            EnsureExists(refreshToken);

            await _refreshTokenRepository.Remove(refreshToken);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
