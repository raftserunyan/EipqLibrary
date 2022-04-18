using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Infrastructure.Data.Repositories.Common;
using EipqLibrary.Infrastructure.Data.Utils.Extensions;
using EipqLibrary.Infrastructure.Data.Utils.Extensions.ObsoletExtensions;
using EipqLibrary.Shared.Utils.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EipqLibrary.Infrastructure.Data.Repositories
{
    public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(EipqLibraryDbContext context) : base(context)
        {
        }

        public async Task<PagedData<Reservation>> GetAllAsync(PageInfo pageInfo, ReservationSortOption reservationSort, ReservationStatus? status = null)
        {
            return await _context.Reservations
                .FilterReservationsByStatus(status)
                .SortReservations(reservationSort)
                .Paged(pageInfo);
        }

        public async Task<PagedData<Reservation>> GetAllReservationsFilteredAndPagedAsync(PageInfo pageInfo, ReservationSortOption reservationSort, ReservationStatus? status = null)
        {
            return await _context.Reservations
                .Include(x => x.BookInstance)
                    .ThenInclude(x => x.Book)
                .Include(x => x.User)
                .FilterReservationsByStatus(status)
                .SortReservations(reservationSort)
                .Paged(pageInfo);
        }

        public async Task<PagedData<Reservation>> GetMyReservationsAsync(PageInfo pageInfo, ReservationSortOption reservationSort, string userId, ReservationStatus? status = null)
        {
            return await _context.Reservations
                .Where(x => x.UserId == userId)
                .FilterReservationsByStatus(status)
                .SortReservations(reservationSort)
                .Paged(pageInfo);
        }

        public async Task<PagedData<Reservation>> GetSoonEndingReservationsPagedAsync(PageInfo pageInfo, int daysUntilReturnDate)
        {
            var todayPlusDaysLeftUntilReturning = DateTime.Now.DropTimePart().AddDays(daysUntilReturnDate);

            var reservationsPaged = await _context.Reservations
                                     .Include(x => x.User)
                                     .Where(x => (x.Status != ReservationStatus.Returned &&
                                                  x.Status != ReservationStatus.Cancelled) &&
                                                  x.ExpectedReturnDate <= todayPlusDaysLeftUntilReturning)
                                     .Paged(pageInfo);

            return reservationsPaged;
        }
    }
}
