using System;

namespace EipqLibrary.Services.DTOs.RequestModels
{
    public class DeleteUserRequest
    {
        public string UserId { get; set; }
        public string MessageToUser { get; set; }
    }
}
