using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using EipqLibrary.Shared.Web.Dtos.Tokens;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace EipqLibrary.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IPublicIdentityService _identityService;

        public IdentityController(IPublicIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(RegistrationResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RegisterUser([FromBody] RegistrationRequest request)
        {
            var registrationResponse = await _identityService.Register(request);

            return Ok(registrationResponse);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthenticationResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Login([FromBody] AuthenticationRequest request)
        {
            var authResponse = await _identityService.Login(request);

            return Ok(authResponse);
        }
    }
}
