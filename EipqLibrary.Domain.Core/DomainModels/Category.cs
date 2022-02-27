using EipqLibrary.Domain.Core.DomainModels.Common;
using System.Collections.Generic;

namespace EipqLibrary.Domain.Core.DomainModels
{
    public class Category : BaseEntity<int>
    {
        public string Name { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
