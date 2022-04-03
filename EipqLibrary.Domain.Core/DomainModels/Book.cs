using EipqLibrary.Domain.Core.DomainModels.Common;
using EipqLibrary.Domain.Core.Enums;

namespace EipqLibrary.Domain.Core.DomainModels
{
    public class Book : BaseEntity
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public int ProductionYear { get; set; }
        public int? PagesCount { get; set; }
        public int TotalCount { get; set; }
        public BookAvailability Availability { get; set; }
        public DeletionReason? DeletionReason { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get ; set; }
    }
}
