using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using System.Threading.Tasks;

namespace EipqLibrary.Services.Interfaces.ServiceInterfaces
{
    public interface IUserService
    {
        Task<UpdateUserStatusDto> UpdateUserStatusAsync(UpdateUserStatusRequest customerUpdateRequest);
        Task<User> ConfirmUserAccount(string userId);
        Task<User> DeleteUserAccount(string userId);
    }
}

