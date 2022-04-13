using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace EipqLibrary.Admin.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAdminIdentityService _adminService;

        public UserRoleController(IAdminIdentityService adminService,
                                  IUserService userService)
        {
            _adminService = adminService;
            _userService = userService;
        }

        [HttpPost("getUserRole")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserRole([FromQuery] string email)
        {
            var response = new GetUserRoleResponse();

            if (email == null)
            {
                response.UserExists = false;
            }
            else
            {
                if (await _userService.GetByEmailOrDefaultAsync(email) != null)
                {
                    response.UserExists = true;
                    response.UserRole = (int)UserRole.Student;
                }
                else if (await _adminService.GetByEmailOrDefaultAsync(email) != null)
                {
                    response.UserExists = true;
                    response.UserRole = (int)UserRole.Admin;
                }
            }

            return Ok(response);
        }
    }
}
