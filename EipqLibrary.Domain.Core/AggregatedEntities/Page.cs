using System;

namespace EipqLibrary.Domain.Core.AggregatedEntities
{
    public class Page
    {
        public Page(int totalItems, int itemsPerPage, int pageNumber)
        {
            TotalItems = totalItems;
            ItemsPerPage = itemsPerPage;
            NumberOfPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);
            PageNumber = pageNumber;
        }

        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int NumberOfPages { get; set; }
        public int PageNumber { get; set; }
    }
}
