using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using EipqLibrary.Shared.CustomExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
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
        private readonly IReservationService _reservationService;

        public ReservationController(UserManager<User> userManager,
                                     IReservationService reservationService)
        {
            _userManager = userManager;
            _reservationService = reservationService;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create(ReservationCreationRequest request)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(currentUserId);
            if (user == null)
            {
                throw new BadDataException("You have to be logged in as a student/lecturer");
            }

            var reservation = await _reservationService.CreateAsync(request, user);

            return Ok(reservation.Id);
        }
    }
}
