using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;

namespace EipqLibrary.Domain.Core.AggregatedEntities
{
    public class BDRSortOption
    {
        public string SortBy { get; set; } = nameof(BookDeletionRequest.RequestCreationDate);
        public SortOrder Sorting { get; set; } = SortOrder.Desc;
    }
}
