using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace EipqLibrary.API.Controllers
{
    [Route("api/groups")]
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
            return Ok(await _groupService.GetByIdAsync(id));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GroupModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _groupService.GetAllAsync());
        }
    }
}
