using EipqLibrary.Domain.Core.DomainModels.Common;
using System.Collections.Generic;

namespace EipqLibrary.Domain.Core.DomainModels
{
    public class Book : BaseEntity
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public int ProductionYear { get; set; }
        public string Description { get; set; }
        public int? PagesCount { get; set; }
        public int TotalCount { get; set; }
        public int AvailableForBorrowingCount { get; set; }
        public int AvailableForUsingInLibraryCount { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get ; set; }

        public ICollection<BookInstance> Instances { get; set; }
    }
}
