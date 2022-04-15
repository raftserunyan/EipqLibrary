using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Services.DTOs.RequestModels;
using System.Threading.Tasks;

namespace EipqLibrary.Services.Interfaces.ServiceInterfaces
{
    public interface IReservationService
    {
        Task<Reservation> CreateAsync(ReservationCreationRequest request, User user);
        Task CancelReservationForStudentAsync(int reservationId, string userId);
        Task CancelReservationForAdminAsync(int reservationId);
        Task<PagedData<Reservation>> GetAllAsync(PageInfo pageInfo, ReservationSortOption reservationSort, ReservationStatus? status);
        Task<PagedData<Reservation>> GetMyReservationsAsync(PageInfo pageInfo, ReservationSortOption reservationSort, ReservationStatus? status, string userId);
    }
}
