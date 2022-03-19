using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using System.Threading.Tasks;

namespace EipqLibrary.Infrastructure.Business.Services
{
    public class AdminRefreshTokenService : IAdminRefreshTokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AdminRefreshTokenService(IUnitOfWork unitOfWork, IRefreshTokenRepository refreshTokenRepository)
        {
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task RemoveAllForAdminId(string adminId)
        {
            await _refreshTokenRepository.RemoveByAdminId(adminId);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
