using System;

namespace EipqLibrary.Services.DTOs.Models
{
    public class BookModel
    {
        public int BookId { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int ProductionYear { get; set; }
        public int PagesCount { get; set; }
        public int TotalCount { get; set; }
        public string Status { get; set; }
        public string DeletionReason { get; set; }

        public CategoryModel Category { get; set; }
    }
}
