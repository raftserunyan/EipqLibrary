using EipqLibrary.Domain.Core.Enums;
using System;

namespace EipqLibrary.Services.DTOs.Models
{
    public class BookModel
    {
        public int BookId { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int ProductionYear { get; set; }
        public string Description { get; set; }
        public int? PagesCount { get; set; }
        public int TotalCount { get; set; }
        public int AvailableForBorrowingCount { get; set; }
        public int AvailableForUsingInLibraryCount { get; set; }
        public DeletionReason? DeletionReason { get; set; }

        public CategoryModel Category { get; set; }
    }
}
