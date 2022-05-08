using EipqLibrary.Admin.Attributes;
using EipqLibrary.Domain.Core.Constants.Admins;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace EipqLibrary.Admin.Controllers
{
    [ApiController]
    public class GroupStudentsController : ControllerBase
    {
        private readonly IUserService _userService;

        public GroupStudentsController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("api/admin/groups/{groupId}/users")]
        //[AuthorizeRoles(AdminRoleNames.SuperAdmin, AdminRoleNames.Librarian)]
        [ProducesResponseType(typeof(IEnumerable<UserModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsersByGroupId(int groupId)
        {
            return Ok(await _userService.GetByGroupIdAsync(groupId));
        }
    }
}
