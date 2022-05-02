using EipqLibrary.Admin.Attributes;
using EipqLibrary.Domain.Core.Constants.Admins;
using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using EipqLibrary.Shared.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EipqLibrary.Admin.Controllers
{
    [AuthorizeRoles(AdminRoleNames.SuperAdmin, AdminRoleNames.Librarian, AdminRoleNames.Accountant)]
    [Route("api/")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAdminIdentityService _adminService;
        private readonly ITokenService _tokenService;

        public UserRoleController(IAdminIdentityService adminService,
                                  IUserService userService,
                                  ITokenService tokenService)
        {
            _adminService = adminService;
            _userService = userService;
            _tokenService = tokenService;
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

        [HttpPost("getUserRoleByToken")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserRole([FromBody] GetUserRoleByTokenRequest request)
        {
            var response = new IsUserAdminResponse();
            response.IsUserAdmin = IsUserAdmin(request.Token);            

            return Ok(response);
        }

        // Private methods
        private bool IsUserAdmin(string accessToken)
        {
            var tokenS = _tokenService.DecodeToken(accessToken);

            var role = tokenS.Claims.FirstOrDefault(claim => claim.Type == "role")?.Value;

            return !(role is null);
        }
    }
}
