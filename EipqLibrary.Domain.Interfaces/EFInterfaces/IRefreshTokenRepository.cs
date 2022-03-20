using EipqLibrary.Domain.Core.DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EipqLibrary.Domain.Interfaces.EFInterfaces
{
    public interface IRefreshTokenRepository
    {
        Task RemoveByAdminId(string adminId);
        Task<AdminRefreshToken> GetByTokenAndDeviceId(string refreshToken, string deviceId);
        Task Add(AdminRefreshToken refreshToken);
        void Remove(AdminRefreshToken refreshToken);
        Task<IList<AdminRefreshToken>> GetAllTokensByAdminId(string adminId);
    }
}
