using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Interfaces.EFInterfaces.Common;
using System.Threading.Tasks;

namespace EipqLibrary.Domain.Interfaces.EFInterfaces
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        Task<PagedData<Book>> GetAllFilteredAndPagedAsync(PageInfo pageInfo, int? categoryId, string author);
        Task<Book> GetByIdWithInstancesAndReservationsAsync(int id);
        Task<Book> GetByIdIncludingInstancesAndTheirBorrowings(int id);
    }
}
