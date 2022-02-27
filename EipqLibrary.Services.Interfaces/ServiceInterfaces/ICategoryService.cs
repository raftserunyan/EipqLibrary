using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EipqLibrary.Services.Interfaces.ServiceInterfaces
{
    public interface ICategoryService
    {
        public Task<List<CategoryModel>> GetAllAsync();
        public Task<CategoryModel> GetByIdAsync(int categoryId);
        public Task<int> CreateCategory(CategoryCreationRequest categoryCreationRequest);
    }
}
