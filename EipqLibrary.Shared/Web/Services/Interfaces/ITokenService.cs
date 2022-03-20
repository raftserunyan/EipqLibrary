using System.Security.Claims;
using EipqLibrary.Shared.Web.Dtos.Tokens;

namespace EipqLibrary.Shared.Web.Services.Interfaces
{
    public interface ITokenService : ICurrentUserService
    {
        public TokenInfo CreateToken(UserTokenInfo user, string deviceId, string loginProvider = null);
        public bool TryGetPrincipalFromToken(string token, out ClaimsPrincipal principal);
        public string GetUserId(ClaimsPrincipal principal);
        string GetDeviceId(ClaimsPrincipal principal);
        public string GetTokenJti(ClaimsPrincipal principal);
        public string CurrentTokenJti { get; }
        public string CurrentDeviceId { get; }
    }
}
