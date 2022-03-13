using System;

namespace EipqLibrary.Services.DTOs.Models
{
    public class GroupModel
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime GraduationDate { get; set; }

        public ProfessionModel Profession { get; set; }
    }
}
