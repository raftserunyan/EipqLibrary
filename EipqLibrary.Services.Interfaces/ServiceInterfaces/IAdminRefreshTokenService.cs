using System.Threading.Tasks;

namespace EipqLibrary.Services.Interfaces.ServiceInterfaces
{
    public interface IAdminRefreshTokenService
    {
        Task RemoveAllForAdminId(string adminId);
    }
}
