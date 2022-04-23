using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using System.Linq;

namespace EipqLibrary.Infrastructure.Data.Utils.Extensions.ObsoletExtensions
{
    public static class QueriableBDRExtensions
    {
        private const string DefaultSortCriteria = nameof(BookDeletionRequest.RequestCreationDate);

        public static IQueryable<T> SortRequests<T>(this IQueryable<T> requests, BDRSortOption bdrSort) where T : BookDeletionRequest
        {
            if (!string.IsNullOrEmpty(bdrSort.SortBy) && typeof(BookDeletionRequest).HasProperty(bdrSort.SortBy))
            {
                return requests.SortBy(bdrSort.Sorting, bdrSort.SortBy);
            }

            return requests.SortBy(bdrSort.Sorting, DefaultSortCriteria);
        }

        public static IQueryable<T> SortBy<T>(this IQueryable<T> requests, SortOrder sortOrder, string sortCriteria) where T : BookDeletionRequest
        {
            return (sortOrder == SortOrder.Asc ? requests.OrderByFieldName(sortCriteria) : requests.OrderByFieldNameDescending(sortCriteria))
                .ThenBy(c => c.BookName);
        }

        public static IQueryable<T> FilterBDRsByStatus<T>(this IQueryable<T> requests, BookDeletionRequestStatus? requestStatus) where T : BookDeletionRequest
        {
            if (requestStatus.HasValue)
            {
                return requests.Where(x => x.Status == requestStatus.Value);
            }

            return requests;
        }
    }
}
