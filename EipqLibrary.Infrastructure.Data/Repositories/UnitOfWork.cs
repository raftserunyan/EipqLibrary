using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Infrastructure.Data.Utils.Extensions;
using EipqLibrary.Shared.CustomExceptions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EipqLibrary.Infrastructure.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EipqLibraryDbContext _context;
        private IBookRepository _bookRepo;
        private ICategoryRepository _categoryRepo;

        public UnitOfWork(EipqLibraryDbContext context)
        {
            _context = context;
        }

        public IBookRepository BookRepository
        {
            get { return _bookRepo ??= _bookRepo = new BookRepository(_context); }
        }
        public ICategoryRepository CategoryRepository
        {
            get { return _categoryRepo ??= new CategoryRepository(_context); }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new EntityUpdateConcurrencyException(ex.Entries);
            }
            catch (DbUpdateException ex) when (ex.ForeignKeyConstraintConflictOnInsert())
            {
                throw new BadDataException("Related entity not found.");
            }
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
