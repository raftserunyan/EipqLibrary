using EipqLibrary.Domain.Core.Enums;

namespace EipqLibrary.Domain.Core.AggregatedEntities
{
    public class ReservationSortOption
    {
        public string SortBy { get; set; } = "ExpectedBorrowingDate";
        public SortOrder Sorting { get; set; } = SortOrder.Asc;
    }
}
