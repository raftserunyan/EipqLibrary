using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using System.Linq;

namespace EipqLibrary.Infrastructure.Data.Utils.Extensions.ObsoletExtensions
{
    public static class QueriableReservationExtensions
    {
        private const string DefaultSortCriteria = "ExpectedBorrowingDate";

        public static IQueryable<T> SortReservations<T>(this IQueryable<T> reservations, ReservationSortOption reservationSort) where T : Reservation
        {
            if (!string.IsNullOrEmpty(reservationSort.SortBy) && typeof(User).HasProperty(reservationSort.SortBy))
            {
                return reservations.SortBy(reservationSort.Sorting, reservationSort.SortBy);
            }

            return reservations.SortBy(reservationSort.Sorting, DefaultSortCriteria);
        }

        public static IQueryable<T> FilterReservationsByStatus<T>(this IQueryable<T> reservations, ReservationStatus? reservationStatus) where T : Reservation
        {
            if (reservationStatus.HasValue)
            {
                return reservations.Where(x => x.Status == reservationStatus.Value);
            }

            return reservations;
        }

        public static IQueryable<T> SortBy<T>(this IQueryable<T> reservations, SortOrder sortOrder, string sortCriteria) where T : Reservation
        {
            return (sortOrder == SortOrder.Asc ? reservations.OrderByFieldName(sortCriteria) : reservations.OrderByFieldNameDescending(sortCriteria))
                .ThenBy(c => c.Status);
        }
    }
}
