using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EipqLibrary.Services.Interfaces.ServiceInterfaces
{
    public interface IUserService
    {
        Task<UserModel> UpdateAsync(UserUpdateRequest updateRequest);
        Task<UpdateUserStatusDto> UpdateUserStatusAsync(UpdateUserStatusRequest customerUpdateRequest);
        Task<User> ConfirmUserAccount(string userId);
        Task<User> DeleteUserAccount(string userId);
        Task<UserModel> GetByEmailOrDefaultAsync(string email);
        Task<PagedData<UserDto>> GetAllAsync(PageInfo pageInfo, UserSortOption userSort, UserStatus? status);
        Task<IEnumerable<UserModel>> GetByGroupIdAsync(int groupId);
    }
}

