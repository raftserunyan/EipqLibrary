using EipqLibrary.Services.DTOs.Authentication;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Shared.Web.Dtos.Tokens;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace EipqLibrary.Services.Interfaces.ServiceInterfaces
{
    public interface IIdentityService
    {
        Task<AuthenticationResponse> Login(AuthenticationRequest request);
        Task<AuthenticationResponse> RefreshToken(RefreshTokenRequest request);
        Task<UserActiveTokensResponse> ActiveTokensCount();
        Task Logout();
        Task<IdentityResult> ChangePassword(ChangePasswordRequest request);
    }
}
