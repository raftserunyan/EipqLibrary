using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EipqLibrary.Services.Interfaces.ServiceInterfaces
{
    public interface IBookCreationRequestService
    {
        Task<int> CreateBookAdditionRequestAsync(BookAdditionRequest bookAdditionRequest);
        Task<IEnumerable<BookCreationRequestModel>> GetAllAsync();
        Task<BookCreationRequestModel> GetByIdAsync(int id);
    }
}
