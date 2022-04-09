using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EipqLibrary.Services.Interfaces.ServiceInterfaces
{
    public interface IBookService
    {
        public Task<List<BookModel>> GetAllAsync();
        public Task<BookModel> GetByIdAsync(int bookId);
        public Task<BookModel> UpdateAsync(BookUpdateRequest updateRequest);
        public Task DeleteAsync(int bookId);
        public Task<BookModel> GetByNameAndAuthorAsync(string name, string author);
    }
}
