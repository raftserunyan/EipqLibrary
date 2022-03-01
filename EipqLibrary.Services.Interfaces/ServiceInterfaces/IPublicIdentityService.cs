using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Shared.Web.Dtos.Tokens;
using System.Threading.Tasks;

namespace EipqLibrary.Services.Interfaces.ServiceInterfaces
{
    public interface IPublicIdentityService
    {
        public Task<RegistrationResponse> Register(RegistrationRequest request);
        public Task<AuthenticationResponse> Login(AuthenticationRequest request);
    }
}
