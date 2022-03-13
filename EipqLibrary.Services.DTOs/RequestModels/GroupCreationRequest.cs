using System;

namespace EipqLibrary.Services.DTOs.RequestModels
{
    public class GroupCreationRequest
    {
        public string Number { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime GraduationDate { get; set; }
        public int ProfessionId { get; set; }
    }
}
