using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;

namespace EipqLibrary.Domain.Core.AggregatedEntities
{
    public class BCRSortOption
    {
        public string SortBy { get; set; } = nameof(BookCreationRequest.RequestCreationDate);
        public SortOrder Sorting { get; set; } = SortOrder.Desc;
    }
}
