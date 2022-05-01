using EipqLibrary.Services.DTOs.Authentication;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Shared.Web.Dtos.Tokens;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace EipqLibrary.Services.Interfaces.ServiceInterfaces
{
    public interface IPublicIdentityService
    {
        Task<RegistrationResponse> Register(RegistrationRequest request);
        Task<AuthenticationResponse> Login(AuthenticationRequest request);
        Task<IdentityResult> ChangePassword(string userId, ChangePasswordRequest request);
        Task<AuthenticationResponse> RefreshToken(RefreshTokenRequest request);
        Task Logout();
        Task ResetPasswordToken(string email);
        Task<string> CheckTokenValidity(TokenValidation resetTokenValidation);
        Task<IdentityResult> ResetPassword(ResetPasswordRequest request);
    }
}
