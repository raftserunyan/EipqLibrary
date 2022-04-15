using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Domain.Interfaces.EFInterfaces.Common;
using System.Threading.Tasks;

namespace EipqLibrary.Domain.Interfaces.EFInterfaces
{
    public interface IReservationRepository : IBaseRepository<Reservation>
    {
        Task<PagedData<Reservation>> GetAllAsync(PageInfo pageInfo, ReservationSortOption reservationSort, ReservationStatus? status = null);
        Task<PagedData<Reservation>> GetMyReservationsAsync(PageInfo pageInfo, ReservationSortOption reservationSort, string userId, ReservationStatus? status = null);
    }
}
