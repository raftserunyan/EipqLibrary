using AutoMapper;
using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
                return new UnauthorizedObjectResult("Դուք պետք է մուտք գործած լինեք որպես ուսանող/դասախոս");
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
                return new UnauthorizedObjectResult("Դուք պետք է մուտք գործած լինեք որպես ուսանող/դասախոս");
            }

            var reservations = await _reservationService.GetReservationsByUserIdAsync(user.Id, pageInfo, reservationSort, status);
            var reservationModels = _mapper.Map<PagedData<ReservationModel>>(reservations);

            return Ok(reservationModels);
        }

        [Authorize]
        [HttpPost("{id}/cancel")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> CancelReservation([FromBody]CancelReservationRequest request)
        {
            var user = await GetCurrentStudentUserAsync();
            if (user == null)
            {
                return new UnauthorizedObjectResult("Դուք պետք է մուտք գործած լինեք որպես ուսանող/դասախոս");
            }

            await _reservationService.CancelReservationForStudentAsync(request.id, user.Id);
            return Ok();
        }

        // Private methods
        private async Task<User> GetCurrentStudentUserAsync()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(currentUserId);

            return user;
        }
    }
}
