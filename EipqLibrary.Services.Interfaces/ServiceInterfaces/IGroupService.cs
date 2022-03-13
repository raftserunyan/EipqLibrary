using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EipqLibrary.Services.Interfaces.ServiceInterfaces
{
    public interface IGroupService
    {
        Task<GroupModel> Create(GroupCreationRequest groupCreationRequest);
        Task<GroupModel> GetByIdAsync(int id, bool includeInactive = false);
        Task<List<GroupModel>> GetAllAsync(bool includeInactive = false);
    }
}
