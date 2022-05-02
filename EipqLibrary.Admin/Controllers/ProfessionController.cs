﻿using EipqLibrary.Admin.Attributes;
using EipqLibrary.Domain.Core.Constants.Admins;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace EipqLibrary.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessionController : ControllerBase
    {
        private readonly IProfessionService _professionService;

        public ProfessionController(IProfessionService professionService)
        {
            _professionService = professionService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProfessionModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _professionService.GetByIdAsync(id));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ProfessionModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _professionService.GetAllAsync());
        }

        [AuthorizeRoles(AdminRoleNames.SuperAdmin, AdminRoleNames.Librarian)]
        [HttpPost]
        [ProducesResponseType(typeof(ProfessionModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create(ProfessionCreationRequest professionCreationRequest)
        {
            return Ok(await _professionService.Create(professionCreationRequest));
        }

        [AuthorizeRoles(AdminRoleNames.SuperAdmin, AdminRoleNames.Librarian)]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProfessionModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create(int id, ProfessionUpdateRequest professionUpdateRequest)
        {
            return Ok(await _professionService.UpdateAsync(professionUpdateRequest));
        }
    }
}
