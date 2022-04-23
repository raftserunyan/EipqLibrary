using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EipqLibrary.Services.Interfaces.ServiceInterfaces
{
    public interface IProfessionService
    {
        Task<bool> ExistsAsync(int professionId);
        Task<ProfessionModel> Create(ProfessionCreationRequest professionCreationRequest);
        Task<ProfessionModel> UpdateAsync(ProfessionUpdateRequest professionUpdateRequest);
        Task<ProfessionModel> GetByIdAsync(int id);
        Task<List<ProfessionModel>> GetAllAsync();
    }
}
