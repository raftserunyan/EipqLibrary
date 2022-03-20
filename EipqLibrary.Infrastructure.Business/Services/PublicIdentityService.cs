using AutoMapper;
using EipqLibrary.Domain.Core.Constants.Claims;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.EmailService.Interfaces;
using EipqLibrary.Services.DTOs.Authentication;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.Models.Tokens;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using EipqLibrary.Shared.CustomExceptions;
using EipqLibrary.Shared.Utils.Extensions;
using EipqLibrary.Shared.Web.Dtos.Tokens;
using EipqLibrary.Shared.Web.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EipqLibrary.Infrastructure.Business.Services
{
    public class PublicIdentityService : IPublicIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGroupService _groupService;
        private readonly ITokenService _tokenService;
        private readonly IPublicRefreshTokenService _refreshTokenService;
        private readonly IEmailService _emailService;

        public PublicIdentityService(UserManager<User> userManager,
                                     IMapper mapper, 
                                     IUnitOfWork unitOfWork,
                                     IGroupService groupService,
                                     ITokenService tokenService,
                                     IEmailService emailService,
                                     IPublicRefreshTokenService refreshTokenService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _groupService = groupService;
            _tokenService = tokenService;
            _emailService = emailService;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<RegistrationResponse> Register(RegistrationRequest request)
        {
            var userCreationDto = _mapper.Map<UserCreationDto>(request);
            await CreateUser(userCreationDto);

            return new RegistrationResponse("Շնորհակալություն\nՁեր գրանցման հայտը ուղարկվել է հաստատման\nԱրդյունքի մասին դուք կստանաք հաղորդագրություն ձեր կողմից նշված էլ․հասցեին");
        }

        public async Task<AuthenticationResponse> Login(AuthenticationRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Username);

            if (user != null
                && user.Status == UserStatus.BlockedByBruteforce
                && DateTime.Compare(DateTime.UtcNow, user.LockoutEnd.Value.DateTime) > 0)
            {
                user.Status = UserStatus.Active;
                await _unitOfWork.SaveChangesAsync();
            }

            if (user == null || user.Status == UserStatus.Active)
            {
                if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    if (user != null)
                    {
                        await _userManager.AccessFailedAsync(user);
                        if (UserIsLockedOut(user))
                        {
                            user.Status = UserStatus.BlockedByBruteforce;
                            await _unitOfWork.SaveChangesAsync();

                            throw GetInvalidUserStatus(user.Status);
                        }
                    }

                    throw GetInvalidCredentialsException();
                }
            }
            else
            {
                throw GetInvalidUserStatus(user.Status);
            }

            if (user.AccessFailedCount > 0)
            {
                await _userManager.ResetAccessFailedCountAsync(user);
            }

            if (user.Status == UserStatus.Inactive)
            {
                throw GetInvalidUserStatus(user.Status);
            }

            var authResponse = await CreateTokenAndRefreshToken(user);
            authResponse.DisplayName = user.FirstName + " " + user.LastName;

            await _userManager.ResetAccessFailedCountAsync(user);

            return authResponse;
        }

        public async Task<IdentityResult> ChangePassword(ChangePasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                throw GetInvalidCredentialsException();
            }

            if (!_userManager.CheckPasswordAsync(user, request.Password).Result)
            {
                throw IncorrectPWD();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
        }

        public async Task<AuthenticationResponse> RefreshToken(RefreshTokenRequest request)
        {
            if (!_tokenService.TryGetPrincipalFromToken(request.Token, out var principal))
            {
                throw GetInvalidTokenException();
            }

            var userId = _tokenService.GetUserId(principal);
            var jti = _tokenService.GetTokenJti(principal);
            var deviceId = _tokenService.GetDeviceId(principal);

            if (!await _refreshTokenService.ExistsUnexpiredForTokenAndDevice(request.RefreshToken, jti, deviceId))
            {
                throw GetInvalidTokenException();
            }

            var user = await _userManager.FindByIdAsync(userId);

            var refreshTokenDto = new PublicRefreshTokenDto
            {
                OldAccessTokenId = jti,
                DeviceId = deviceId,
                OldRefreshToken = request.RefreshToken,
                UserId = userId
            };

            var tokenProvider = GetTokenProvider(principal);

            return await CreateTokenAndRefreshToken(user, refreshTokenDto, tokenProvider);
        }

        public async Task Logout()
        {
            await _refreshTokenService.RemoveForDevice(_tokenService.CurrentTokenJti, _tokenService.CurrentDeviceId);
        }

        public async Task ResetPasswordToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw InvalidEmail();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var message = _emailService.GenerateResetPasswordMailMessage(user.Email, token);
            await _emailService.SendEmailMessageAsync(message);
        }

        public async Task<string> CheckTokenValidity(TokenValidation resetTokenValidation)
        {
            var user = await _userManager.FindByEmailAsync(resetTokenValidation.Email);
            if (user == null)
            {
                throw InvalidEmail();
            }

            if (await _userManager.VerifyUserTokenAsync(user, "Default", "ResetPassword", resetTokenValidation.Token) is true)
            {
                return user.Email;
            }

            throw GetInvalidTokenException();
        }

        public async Task<IdentityResult> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                InvalidEmail();
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!result.Succeeded)
            {
                throw GetInvalidTokenException();
            }

            return result;
        }


        // Private methods
        private async Task<User> CreateUser(UserCreationDto userCreationDto)
        {
            var newUser = _mapper.Map<User>(userCreationDto);
            newUser.GroupId = (await _groupService.GetActiveByNumberAsync(userCreationDto.GroupNumber)).Id;
            newUser.UserName = userCreationDto.Email;
            newUser.RegistrationDate = DateTime.Now;

            var creationResult = await _userManager.CreateAsync(newUser, userCreationDto.Password);

            if (!creationResult.Succeeded)
            {
                throw new BadDataException(creationResult.Errors.ToSelectiveErrorsDictionary(
                    new[] { nameof(userCreationDto.Email), nameof(userCreationDto.Password) }));
            }

            return newUser;
        }
        private async Task<AuthenticationResponse> CreateTokenAndRefreshToken(User user, PublicRefreshTokenDto refreshTokenDto = null, string tokenProvider = null)
        {
            refreshTokenDto ??= new PublicRefreshTokenDto
            {
                UserId = user.Id,
                DeviceId = Guid.NewGuid().ToString()
            };

            var tokenInfo = _tokenService.CreateToken(ConvertUserToTokenInfo(user), refreshTokenDto.DeviceId, tokenProvider);

            refreshTokenDto.AccessTokenId = tokenInfo.Id;
            var refreshTokenInfo = await _refreshTokenService.SubstituteWithNew(refreshTokenDto);

            return new AuthenticationResponse
            {
                Token = tokenInfo.Token,
                TokenExpiryDate = tokenInfo.ExpiryDate,
                RefreshToken = refreshTokenInfo.Token,
                RefreshTokenExpiryDate = refreshTokenInfo.ExpiryDate,
                UserFirstName = user.FirstName
            };
        }
        private UserTokenInfo ConvertUserToTokenInfo(User user)
        {
            return new UserTokenInfo()
            {
                Id = user.Id
            };
        }
        private bool UserIsLockedOut(User user)
        {
            if (user != null && user.LockoutEnabled && user.LockoutEnd != null && DateTimeOffset.UtcNow <= user.LockoutEnd)
            {
                return true;
            }

            return false;
        }
        private string GetTokenProvider(ClaimsPrincipal principal)
        {
            return principal.Claims.Any(c => c.Type.ToLower().Equals(CustomClaimNames.Provider))
                ? principal.Claims.First(c => c.Type.ToLower().Equals(CustomClaimNames.Provider)).Value
                : null;
        }

        private Exception GetInvalidUserStatus(UserStatus status)
        {
            if (status == UserStatus.Inactive)
            {
                return new AuthenticationException("Դուք չեք կարող մուտք գործել ձեր հաշիվ քանի որ այն դեռևս հաստատված չէ ադմինիստրատորի կողմից");
            }
            else if (status == UserStatus.BlockedByBruteforce)
            {
                return new AuthenticationException("Դուք չեք կարող մուտք գործել ձեր հաշվի վիճակի պատճառով" +
                    "Այն ժամանակավորապես ապաակտիվացվել է սխալ մուտքանուն կամ ծածկագիր ներմուծելու պատճառով");
            }
            else
            {
                return new AuthenticationException("Your account is blocked. Contact helpdesk@boardzone.com to further clarification");
            }
        }
        private Exception GetInvalidCredentialsException()
        {
            return new BadDataException("Մուտքագրված մուտքանունը և/կամ ծածկագիրը սխալ է");
        }
        private Exception GetInvalidTokenException() => new AuthenticationException("Invalid-token");
        private Exception IncorrectPWD() => new BadDataException("Ընթացիկ գաղտնաբառը սխալ է");
        private Exception InvalidEmail() => new BadDataException("Էլ․ հասցեն սխալ է");
    }
}
