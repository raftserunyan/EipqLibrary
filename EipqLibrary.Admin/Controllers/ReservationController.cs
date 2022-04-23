using AutoMapper;
using EipqLibrary.Admin.Attributes;
using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.Constants.Admins;
using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace EipqLibrary.Admin.Controllers
{
    [AuthorizeRoles(AdminRoleNames.SuperAdmin, AdminRoleNames.Librarian)]
    [Route("api/admin/reservations")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IReservationService _reservationService;

        public ReservationController(IMapper mapper,
                                     IReservationService reservationService)
        {
            _mapper = mapper;
            _reservationService = reservationService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedData<ReservationModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromQuery] PageInfo pageInfo, [FromQuery] ReservationSortOption reservationSort, [FromQuery] ReservationStatus? status)
        {
            var reservationsPaged = await _reservationService.GetAllReservationsPagedAsync(pageInfo, reservationSort, status);
            var reservationModelsPaged = _mapper.Map<PagedData<ReservationModel>>(reservationsPaged);

            return Ok(reservationModelsPaged);
        }

        [HttpGet]
        [Route("getEndingSoonReservations")]
        [ProducesResponseType(typeof(PagedData<ReservationModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSoonEndingReservations([FromQuery] PageInfo pageInfo)
        {
            var reservationsPaged = await _reservationService.GetSoonEndingReservationsPagedAsync(pageInfo);
            var reservationModelsPaged = _mapper.Map<PagedData<ReservationModel>>(reservationsPaged);

            return Ok(reservationModelsPaged);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> CancelReservation(int id, ReservationStatusChangeRequest changes)
        {
            await _reservationService.ChangeReservationStatusAsync(id, changes);
            return Ok();
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(PagedData<ReservationModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsersReservations(string userId, [FromQuery] PageInfo pageInfo, [FromQuery] ReservationSortOption sort, [FromQuery] ReservationStatus? status)
        {
            var reservations = await _reservationService.GetReservationsByUserIdAsync(userId, pageInfo, sort, status);
            var reservationModels = _mapper.Map<PagedData<ReservationModel>>(reservations);

            return Ok(reservationModels);
        }
    }
}
