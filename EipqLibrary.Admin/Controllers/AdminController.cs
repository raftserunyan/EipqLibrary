using AutoMapper;
using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
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

        public AdminController(IMapper mapper,
                               IAdminIdentityService adminIdentityService)
        {
            _mapper = mapper;
            _adminIdentityService = adminIdentityService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AdminUserModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateAdmin(AdminCreationRequest adminCreationRequest)
        {
            var adminCreationDto = _mapper.Map<AdminCreationDto>(adminCreationRequest);
            var admin = await _adminIdentityService.CreateAsync(adminCreationDto);

            return Ok(admin);
        }

        [HttpPut("changestatus")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> ChangeAdminStatus(AdminChangeStatusRequest adminUpdateModel)
        {
            var adminUpdateDto = _mapper.Map<AdminChangeStatusDto>(adminUpdateModel);
            await _adminIdentityService.ChangeStatusAsync(adminUpdateDto);
            return Ok();
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteAdmin(AdminDeletionRequest adminDeletionModel)
        {
            var adminDeletionDto = _mapper.Map<AdminDeletionDto>(adminDeletionModel);
            await _adminIdentityService.DeleteAsync(adminDeletionDto);
            return NoContent();
        }

        [HttpGet("{email}")]
        [ProducesResponseType(typeof(AdminInfo), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAdminByEmail(string email)
        {
            var admin = await _adminIdentityService.GetByEmail(email);
            return Ok(admin);
        }

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
