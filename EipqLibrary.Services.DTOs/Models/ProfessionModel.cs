using System;

namespace EipqLibrary.Services.DTOs.Models
{
    public class ProfessionModel
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DeletionDate { get; set; }
    }
}
