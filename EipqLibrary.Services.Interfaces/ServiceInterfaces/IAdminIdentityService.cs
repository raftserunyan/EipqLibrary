using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Services.DTOs.Models;
using System.Threading.Tasks;

namespace EipqLibrary.Services.Interfaces.ServiceInterfaces
{
    public interface IAdminIdentityService
    {
        Task<AdminUserModel> CreateAsync(AdminCreationDto adminCreationDto);

        Task ChangeStatusAsync(AdminChangeStatusDto adminChangeStatusDto);

        Task DeleteAsync(AdminDeletionDto adminDeletionDto);

        Task<AdminInfo> GetByEmail(string email);

        PagedData<AdminInfo> GetAll(PageInfo pageInfo);
    }
}
