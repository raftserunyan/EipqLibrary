using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EipqLibrary.Domain.Interfaces.EFInterfaces
{
    public interface IUserRepository
    {
        Task<PagedData<User>> GetAllAsync(PageInfo pageInfo, UserSortOption customerSort, UserStatus? status = null);
        Task<User> FindAsync(Expression<Func<User, bool>> predicate);
        void Delete(User user);
    }
}
