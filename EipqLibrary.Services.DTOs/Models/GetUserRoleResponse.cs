using System;

namespace EipqLibrary.Services.DTOs.Models
{
    public class GetUserRoleResponse
    {
        public bool UserExists { get; set; }
        public int? UserRole { get; set; }

        public GetUserRoleResponse()
        {
            UserRole = null;
        }

        public GetUserRoleResponse(bool userExists, int? userRole = null)
        {
            UserExists = userExists;
            UserRole = userRole;
        }
    }
}
