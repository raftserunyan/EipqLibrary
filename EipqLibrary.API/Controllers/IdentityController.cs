using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
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
            var authResponse = await _identityService.Register(request);

            return Ok(authResponse);
        }
    }
}
