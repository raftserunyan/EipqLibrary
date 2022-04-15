using EipqLibrary.Services.DTOs.Authentication;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using EipqLibrary.Shared.Web.Dtos.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace EipqLibrary.API.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        private readonly IPublicIdentityService _identityService;

        public IdentityController(IPublicIdentityService identityService)
        {
            _identityService = identityService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(typeof(RegistrationResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RegisterUser([FromBody] RegistrationRequest request)
        {
            var registrationResponse = await _identityService.Register(request);

            return Ok(registrationResponse);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthenticationResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Login([FromBody] AuthenticationRequest request)
        {
            var authResponse = await _identityService.Login(request);

            return Ok(authResponse);
        }

        [HttpPost("change-password")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            return Ok(await _identityService.ChangePassword(request));
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(AuthenticationResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var authResponse = await _identityService.RefreshToken(request);

            return Ok(authResponse);
        }

        [HttpPost("logout")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Logout()
        {
            await _identityService.Logout();
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("reset-pass")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GenerateResetToken(string email)
        {
            await _identityService.ResetPasswordToken(email);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("check-resetToken")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CheckTokenValidity(TokenValidation resetTokenValidation)
        {
            var respone = await _identityService.CheckTokenValidity(resetTokenValidation);

            return Ok(respone);
        }

        [AllowAnonymous]
        [HttpPost("password-reset")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            return Ok(await _identityService.ResetPassword(request));
        }
    }
}
