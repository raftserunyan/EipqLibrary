using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace EipqLibrary.Admin.Controllers
{
    [Route("api/admin/groups")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GroupModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _groupService.GetByIdAsync(id, true));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GroupModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _groupService.GetAllAsync(true));
        }

        [HttpPost]
        [ProducesResponseType(typeof(GroupModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create(GroupCreationRequest groupCreationRequest)
        {
            return Ok(await _groupService.Create(groupCreationRequest));
        }
    }
}
