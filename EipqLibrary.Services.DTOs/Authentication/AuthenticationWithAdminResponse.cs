using EipqLibrary.Services.DTOs.Models;

namespace EipqLibrary.Services.DTOs.Authentication
{
    public class AuthenticationWithAdminResponse
    {
        public AuthenticationResponse TokensData;
        public AdminInfo Admin;
        public int Role { get; set; }
    }
}
