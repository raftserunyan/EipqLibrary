using AutoMapper;
using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using EipqLibrary.Shared.CustomExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EipqLibrary.API.Controllers
{
    [Route("api/reservations")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IReservationService _reservationService;

        public ReservationController(UserManager<User> userManager,
                                     IMapper mapper,
                                     IReservationService reservationService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _reservationService = reservationService;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create(ReservationCreationRequest request)
        {
            var user = await GetCurrentStudentUserAsync();
            if (user == null)
            {
                return new UnauthorizedObjectResult("You have to be logged in as a student/lecturer");
            }

            var reservation = await _reservationService.CreateAsync(request, user);

            return Ok(reservation.Id);
        }

        [Authorize]
        [HttpGet("myReservations")]
        [ProducesResponseType(typeof(PagedData<ReservationModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCurrentUserReservations([FromQuery] PageInfo pageInfo, [FromQuery] ReservationSortOption reservationSort, [FromQuery] ReservationStatus? status)
        {
            var user = await GetCurrentStudentUserAsync();
            if (user == null)
            {
                return new UnauthorizedObjectResult("You have to be logged in as a student/lecturer");
            }

            var reservations = await _reservationService.GetMyReservationsAsync(pageInfo, reservationSort, status, user.Id);
            var reservationModels = _mapper.Map<PagedData<ReservationModel>>(reservations);

            return Ok(reservationModels);
        }

        [Authorize]
        [HttpPost("{id}/cancel")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> CancelReservation([FromQuery]int id)
        {
            var user = await GetCurrentStudentUserAsync();
            if (user != null)
            {
                await _reservationService.CancelReservationForStudentAsync(id, user.Id);
                return Ok();
            }

            //If the canceller is admin
            await _reservationService.CancelReservationForAdminAsync(id);
            return Ok();
        }

        private async Task<User> GetCurrentStudentUserAsync()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(currentUserId);

            return user;
        }
    }
}
