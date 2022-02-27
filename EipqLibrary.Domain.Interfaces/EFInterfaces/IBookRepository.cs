using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Interfaces.EFInterfaces.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EipqLibrary.Domain.Interfaces.EFInterfaces
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        public Task<Book> GetByIdForStudentAsync(int bookId);
        public Task<List<Book>> GetAllForStudentAsync();
    }
}
