using System;
using System.ComponentModel.DataAnnotations;

namespace EipqLibrary.Domain.Core.AggregatedEntities
{
    public class PageInfo
    {
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Range(1, int.MaxValue)]
        public int ItemsPerPage { get; set; } = 25;
    }
}
