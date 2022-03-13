using EipqLibrary.Domain.Core.DomainModels.Common;
using EipqLibrary.Domain.Interfaces.EFInterfaces.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EipqLibrary.Infrastructure.Data.Repositories.Common
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly EipqLibraryDbContext _context;

        public BaseRepository(EipqLibraryDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _context.AddAsync(entity);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return !(await _context.Set<T>().FirstOrDefaultAsync(predicate) == default(T));
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<List<T>> GetAllWithIncludeAsync(params Expression<Func<T, object>>[] includeExpressions)
        {
            var table = GetAllInclude(includeExpressions);

            return await table.ToListAsync();
        }

        public async Task<List<T>> GetAllWithIncludeAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeExpressions)
        {
            var table = GetAllInclude(includeExpressions);

            return await table.Where(predicate).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int entityId)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == entityId);
        }

        public async Task<T> GetByIdWithIncludeAsync(int entityId, params Expression<Func<T, object>>[] includeExpressions)
        {
            var table = GetAllInclude(includeExpressions);

            return await table.FirstOrDefaultAsync(x => x.Id == entityId);
        }

        public async Task<T> GetFirstWithIncludeAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeExpressions)
        {
            var table = GetAllInclude(includeExpressions);

            return await table.FirstOrDefaultAsync(predicate);
        }

        private IQueryable<T> GetAllInclude(params Expression<Func<T, object>>[] includeExpressions)
        {
            var table = _context.Set<T>().AsQueryable();

            foreach (var includeExpression in includeExpressions)
            {
                table = table.Include(includeExpression);
            }

            return table;
        }
    }
}
