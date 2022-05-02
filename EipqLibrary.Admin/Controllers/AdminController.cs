using AutoMapper;
using EipqLibrary.Admin.Attributes;
using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.Constants.Admins;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using EipqLibrary.Shared.CustomExceptions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EipqLibrary.Admin.Controllers
{
    [Route("api/admins")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAdminIdentityService _adminIdentityService;
        private readonly IUserService _userService;

        public AdminController(IMapper mapper,
                               IAdminIdentityService adminIdentityService,
                               IUserService userService)
        {
            _mapper = mapper;
            _adminIdentityService = adminIdentityService;
            _userService = userService;
        }

        [AuthorizeRoles(AdminRoleNames.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(AdminUserModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateAdmin(AdminCreationRequest adminCreationRequest)
        {
            if (await _userService.GetByEmailOrDefaultAsync(adminCreationRequest.Email) != null)
            {
                throw new BadDataException($"A user with email {adminCreationRequest.Email} already exists");
            }

            var adminCreationDto = _mapper.Map<AdminCreationDto>(adminCreationRequest);
            var admin = await _adminIdentityService.CreateAsync(adminCreationDto);

            return Ok(admin);
        }

        [AuthorizeRoles(AdminRoleNames.SuperAdmin)]
        [HttpPut("changestatus")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> ChangeAdminStatus(AdminChangeStatusRequest adminUpdateModel)
        {
            var adminUpdateDto = _mapper.Map<AdminChangeStatusDto>(adminUpdateModel);
            await _adminIdentityService.ChangeStatusAsync(adminUpdateDto);
            return Ok();
        }

        [AuthorizeRoles(AdminRoleNames.SuperAdmin)]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteAdmin(AdminDeletionRequest adminDeletionModel)
        {
            var adminDeletionDto = _mapper.Map<AdminDeletionDto>(adminDeletionModel);
            await _adminIdentityService.DeleteAsync(adminDeletionDto);
            return NoContent();
        }

        [AuthorizeRoles(AdminRoleNames.SuperAdmin, AdminRoleNames.Accountant, AdminRoleNames.Librarian)]
        [HttpGet("{email}")]
        [ProducesResponseType(typeof(AdminInfo), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAdminByEmail(string email)
        {
            var admin = await _adminIdentityService.GetByEmail(email);
            return Ok(admin);
        }

        [AuthorizeRoles(AdminRoleNames.SuperAdmin)]
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<AdminInfo>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll([FromQuery] PageInfo pageInfo)
        {
            var admins = _adminIdentityService.GetAll(pageInfo);

            if (!admins.Data.Any())
            {
                return NoContent();
            }

            return Ok(admins);
        }
    }
}
