using System.Collections.Generic;

namespace EipqLibrary.Domain.Core.AggregatedEntities
{
    public class PagedData<T>
    {
        public Page Page { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
