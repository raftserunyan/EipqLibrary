using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EipqLibrary.Domain.Interfaces.EFInterfaces.Common
{
    public interface IBaseRepository<T> where T : class
    {
        public Task AddAsync(T entity);
        public Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        public Task<List<T>> GetAllAsync();
        public Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        public Task<List<T>> GetAllWithIncludeAsync(params Expression<Func<T, object>>[] includeExpressions);
        public Task<List<T>> GetAllWithIncludeAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeExpressions);
        public Task<T> GetByIdAsync(int entityId);
        public Task<T> GetByIdWithIncludeAsync(int entityId, params Expression<Func<T, object>>[] includeExpressions);
        public Task<T> GetFirstAsync(Expression<Func<T, bool>> predicate);
        public Task<T> GetFirstWithIncludeAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeExpressions);
    }
}   
