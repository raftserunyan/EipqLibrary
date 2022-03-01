using EipqLibrary.Services.DTOs.Models.Tokens;
using System.Threading.Tasks;

namespace EipqLibrary.Services.Interfaces.ServiceInterfaces
{
    public interface IPublicRefreshTokenService
    {
        public Task<bool> ExistsUnexpiredForTokenAndDevice(string refreshToken, string accessTokenId, string deviceId);
        public Task<RefreshTokenInfo> SubstituteWithNew(PublicRefreshTokenDto publicRefreshTokenDto);
        public Task RemoveForDevice(string accessTokenId, string deviceId);
    }
}
