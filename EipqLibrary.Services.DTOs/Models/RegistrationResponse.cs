using System;

namespace EipqLibrary.Services.DTOs.Models
{
    public class RegistrationResponse
    {
        public string Message { get; set; }

        public RegistrationResponse(string message)
        {
            Message = message;
        }
    }
}
