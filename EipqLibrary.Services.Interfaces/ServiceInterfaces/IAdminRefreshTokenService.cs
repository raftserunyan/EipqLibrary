using EipqLibrary.Services.DTOs.Models.Tokens;
using System.Threading.Tasks;

namespace EipqLibrary.Services.Interfaces.ServiceInterfaces
{
    public interface IAdminRefreshTokenService
    {
        Task RemoveAllForAdminId(string adminId);
        Task<RefreshTokenInfo> SubstituteWithNew(string userId, string deviceId, string oldRefreshToken);
        Task<bool> ExistsUnexpiredForDevice(string refreshToken, string deviceId);
        Task<int> ActiveTokensCountForAdminId(string adminId);
    }
}
