using System.Threading.Tasks;

namespace EipqLibrary.Domain.Interfaces.EFInterfaces
{
    public interface IRefreshTokenRepository
    {
        Task RemoveByAdminId(string adminId);
    }
}
