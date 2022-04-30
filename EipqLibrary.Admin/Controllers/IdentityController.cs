using EipqLibrary.Admin.Attributes;
using EipqLibrary.Domain.Core.Constants.Admins;
using EipqLibrary.Services.DTOs.Authentication;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using EipqLibrary.Shared.Web.Dtos.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace EipqLibrary.Admin.Controllers
{
    [Produces("application/json")]
    [Route("api/admin/identity")]
    [ApiController]
    [AuthorizeRoles(AdminRoleNames.SuperAdmin, AdminRoleNames.Librarian, AdminRoleNames.Accountant)]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthenticationResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Login([FromBody] AuthenticationRequest request)
        {
            var authResponse = await _identityService.Login(request);

            return Ok(authResponse);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(AuthenticationResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var authResponse = await _identityService.RefreshToken(request);

            return Ok(authResponse);
        }

        [HttpGet("activetokenscount")]
        [ProducesResponseType(typeof(UserActiveTokensResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetActiveTokensCount()
        {
            var authResponse = await _identityService.ActiveTokensCount();

            return Ok(authResponse);
        }

        [HttpPost("logout")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Logout()
        {
            await _identityService.Logout();
            return NoContent();
        }

        [HttpPost("change-password")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            return Ok(await _identityService.ChangePassword(request));
        }
    }
}
