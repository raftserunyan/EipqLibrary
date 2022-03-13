using EipqLibrary.Domain.Core.DomainModels.Common;
using System;
using System.Collections.Generic;

namespace EipqLibrary.Domain.Core.DomainModels
{
    public class Group : BaseEntity
    {
        public string Number { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime GraduationDate { get; set; }

        public int ProfessionId { get; set; }
        public Profession Profession { get; set; }

        public IEnumerable<User> Students { get; set; }
    }
}
