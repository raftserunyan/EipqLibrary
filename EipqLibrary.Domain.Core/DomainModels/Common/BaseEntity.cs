using System.ComponentModel.DataAnnotations;

namespace EipqLibrary.Domain.Core.DomainModels.Common
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
