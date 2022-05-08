using System;

namespace EipqLibrary.Services.DTOs.Models
{
    public class GroupModel
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string CreationDate { get; set; }
        public string GraduationDate { get; set; }
        public int CreationYear { get; set; }
        public int GraduationYear { get; set; }

        public ProfessionModel Profession { get; set; }
    }
}
