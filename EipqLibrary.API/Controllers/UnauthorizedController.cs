using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace EipqLibrary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnauthorizedController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            return Unauthorized();
        }
    }
}
