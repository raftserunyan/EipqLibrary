using System;
using System.Collections.Generic;
using EipqLibrary.Domain.Core.DomainModels.Common;

namespace EipqLibrary.Domain.Core.DomainModels
{
    public class Profession : BaseEntity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? DeletionDate { get; set; }

        public IEnumerable<Group> Groups { get; set; }
    }
}
