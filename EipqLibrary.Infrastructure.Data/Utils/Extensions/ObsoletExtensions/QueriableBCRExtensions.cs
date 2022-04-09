using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using System.Linq;

namespace EipqLibrary.Infrastructure.Data.Utils.Extensions.ObsoletExtensions
{
    public static class QueriableBCRExtensions
    {
        private const string DefaultSortCriteria = nameof(BookCreationRequest.RequestCreationDate);

        public static IQueryable<T> SortRequests<T>(this IQueryable<T> requests, BCRSortOption bcrSort) where T : BookCreationRequest
        {
            if (!string.IsNullOrEmpty(bcrSort.SortBy) && typeof(BookCreationRequest).HasProperty(bcrSort.SortBy))
            {
                return requests.SortBy(bcrSort.Sorting, bcrSort.SortBy);
            }

            return requests.SortBy(bcrSort.Sorting, DefaultSortCriteria);
        }

        public static IQueryable<T> SortBy<T>(this IQueryable<T> requests, SortOrder sortOrder, string sortCriteria) where T : BookCreationRequest
        {
            return (sortOrder == SortOrder.Asc ? requests.OrderByFieldName(sortCriteria) : requests.OrderByFieldNameDescending(sortCriteria))
                .ThenBy(c => c.Name);
        }

        public static IQueryable<T> FilterBCRsByStatus<T>(this IQueryable<T> requests, BookCreationRequestStatus? requestStatus) where T : BookCreationRequest
        {
            if (requestStatus.HasValue)
            {
                return requests.Where(x => x.RequestStatus == requestStatus.Value);
            }

            return requests;
        }
    }
}
