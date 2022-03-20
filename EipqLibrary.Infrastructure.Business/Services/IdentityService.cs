using AutoMapper;
using EipqLibrary.Domain.Core.Constants.Admins;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Services.DTOs.Authentication;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using EipqLibrary.Shared.CustomExceptions;
using EipqLibrary.Shared.Web.Dtos.Tokens;
using EipqLibrary.Shared.Web.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace EipqLibrary.Infrastructure.Business.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AdminUser> _userManager;
        private readonly IAdminRefreshTokenService _refreshTokenService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public IdentityService(
            UserManager<AdminUser> userManager,
            IAdminRefreshTokenService refreshTokenService,
            ITokenService tokenService,
            IMapper mapper)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
            _mapper = mapper;
        }

        public async Task<AuthenticationWithAdminResponse> Login(AuthenticationRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw InvalidCredentialsException();
            }

            var roles = AdminRoleNames.GetAll();
            var userRoles = await _userManager.GetRolesAsync(user);

            if (!userRoles.Any(roles.Contains))
            {
                throw InvalidCredentialsException();
            }

            if (!user.IsActive)
            {
                throw DisabledAdminException();
            }

            AuthenticationWithAdminResponse authenticationResponse = new AuthenticationWithAdminResponse();
            authenticationResponse.TokensData = await CreateTokenAndRefreshToken(user);
            authenticationResponse.Admin = _mapper.Map<AdminInfo>(user);
            return authenticationResponse;
        }

        public async Task<AuthenticationResponse> RefreshToken(RefreshTokenRequest request)
        {
            if (!_tokenService.TryGetPrincipalFromToken(request.Token, out var principal))
            {
                throw InvalidTokenException();
            }

            var userId = _tokenService.GetUserId(principal);
            var deviceId = _tokenService.GetDeviceId(principal);

            if (!await _refreshTokenService.ExistsUnexpiredForDevice(request.RefreshToken, deviceId))
            {
                throw InvalidTokenException();
            }

            var user = await _userManager.FindByIdAsync(userId);

            return await CreateTokenAndRefreshToken(user, deviceId, request.RefreshToken);
        }

        public async Task<UserActiveTokensResponse> ActiveTokensCount()
        {
            var adminId = _tokenService.CurrentUserId;
            return new UserActiveTokensResponse()
            {
                Quantity = await _refreshTokenService.ActiveTokensCountForAdminId(adminId)
            };
        }

        public async Task Logout()
        {
            var adminId = _tokenService.CurrentUserId;
            await _refreshTokenService.RemoveAllForAdminId(adminId);
        }

        public async Task<IdentityResult> ChangePassword(ChangePasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                throw InvalidCredentialsException();
            }

            if (!_userManager.CheckPasswordAsync(user, request.Password).Result)
            {
                throw IncorrectPWD();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
        }

        // Private methods
        private async Task<AuthenticationResponse> CreateTokenAndRefreshToken(AdminUser user, string deviceId = null, string oldRefreshToken = null)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                deviceId = Guid.NewGuid().ToString();
            }

            var tokenInfo = _tokenService.CreateToken(await ConvertUserToTokenInfo(user), deviceId);
            var refreshTokenInfo = await _refreshTokenService.SubstituteWithNew(user.Id, deviceId, oldRefreshToken);

            return new AuthenticationResponse
            {
                Token = tokenInfo.Token,
                TokenExpiryDate = tokenInfo.ExpiryDate,
                RefreshToken = refreshTokenInfo.Token,
                RefreshTokenExpiryDate = refreshTokenInfo.ExpiryDate
            };
        }
        private async Task<UserTokenInfo> ConvertUserToTokenInfo(AdminUser user)
        {
            return new UserTokenInfo()
            {
                Id = user.Id,
                Roles = await _userManager.GetRolesAsync(user)
            };
        }

        private Exception DisabledAdminException() => new AuthenticationException("Մուտքը ձախողվեց քանի որ ձեր հաշիվը ապաակտիվացված է");
        private Exception InvalidTokenException() => new AuthenticationException("Invalid-token");
        private Exception InvalidCredentialsException() => new BadDataException("Մուտքագրված մուտքանունը և/կամ ծածկագիրը սխալ է");
        private Exception IncorrectPWD() => new BadDataException("Ընթացիկ գաղտնաբառը սխալ է");
    }
}
