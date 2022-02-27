using System;
using System.Threading.Tasks;

namespace EipqLibrary.Domain.Interfaces.EFInterfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IBookRepository BookRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public Task SaveChangesAsync();
    }
}
