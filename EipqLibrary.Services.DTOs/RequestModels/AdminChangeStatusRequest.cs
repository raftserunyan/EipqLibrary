using System;

namespace EipqLibrary.Services.DTOs.RequestModels
{
    public class AdminChangeStatusRequest
    {
        public string Email { get; set; }
        public bool ActiveStatus { get; set; }
    }
}
