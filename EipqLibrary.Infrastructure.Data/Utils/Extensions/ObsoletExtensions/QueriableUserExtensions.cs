using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using System.Linq;

namespace EipqLibrary.Infrastructure.Data.Utils.Extensions.ObsoletExtensions
{
    public static class QueryableUserExtensions
    {
        private const string DefaultSortCriteria = "RegistrationDate";

        public static IQueryable<T> SortUsers<T>(this IQueryable<T> users, UserSortOption userSort) where T : User
        {
            if (!string.IsNullOrEmpty(userSort.SortBy) && typeof(User).HasProperty(userSort.SortBy))
            {
                return users.SortBy(userSort.Sorting, userSort.SortBy);
            }

            return users.SortBy(userSort.Sorting, DefaultSortCriteria);
        }

        public static IQueryable<T> FilterUsersByStatus<T>(this IQueryable<T> users, UserStatus? userStatus) where T : User
        {
            if (userStatus.HasValue)
            {
                return users.Where(x => x.Status == userStatus.Value);
            }

            return users;
        }

        public static IQueryable<T> SortBy<T>(this IQueryable<T> users, SortOrder sortOrder, string sortCriteria) where T : User
        {
            return (sortOrder == SortOrder.Asc ? users.OrderByFieldName(sortCriteria) : users.OrderByFieldNameDescending(sortCriteria))
                .ThenBy(c => c.Email);
        }
    }
}
