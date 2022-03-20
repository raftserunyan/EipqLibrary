using System;

namespace EipqLibrary.Services.DTOs.Models
{
    public class ProfessionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DeletionDate { get; set; }
    }
}
