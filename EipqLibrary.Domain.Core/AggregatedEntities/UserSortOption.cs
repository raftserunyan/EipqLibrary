using EipqLibrary.Domain.Core.Enums;    

namespace EipqLibrary.Domain.Core.AggregatedEntities
{
    public class UserSortOption
    {
        public string SortBy { get; set; } = "RegistrationDate";
        public SortOrder Sorting { get; set; } = SortOrder.Desc;
    }
}
