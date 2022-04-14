using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
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
        public async Task<IActionResult> GetUserRole([FromBody] GetUserRoleRequest request)
        {
            var response = new GetUserRoleResponse();

            if (request.Email == null)
            {
                response.UserExists = false;
            }
            else
            {
                if (await _userService.GetByEmailOrDefaultAsync(request.Email) != null)
                {
                    response.UserExists = true;
                    response.UserRole = (int)UserRole.Student;
                }
                else if (await _adminService.GetByEmailOrDefaultAsync(request.Email) != null)
                {
                    response.UserExists = true;
                    response.UserRole = (int)UserRole.Admin;
                }
            }

            return Ok(response);
        }
    }
}
