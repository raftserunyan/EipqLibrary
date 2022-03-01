using System.Threading.Tasks;
using EipqLibrary.Domain.Core.DomainModels;

namespace EipqLibrary.Domain.Interfaces.EFInterfaces
{
    public interface IPublicRefreshTokenRepository
    {
        Task<PublicRefreshToken> GetByAccessTokenIdAndDeviceId(string accessTokenId, string deviceId);
        Task<PublicRefreshToken> GetByTokenAndAccessTokenIdAndDeviceId(string refreshToken, string accessTokenId, string deviceId);
        Task Add(PublicRefreshToken refreshToken);
        Task Remove(PublicRefreshToken refreshToken);
    }
}
