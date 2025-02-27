using ATechnologiesAssignment.App.Helpers;
using ATechnologiesAssignment.Domain.Base;
using System.Linq.Expressions;

namespace ATechnologiesAssignment.App.Contracts.IRepositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<PagedList<TEntity>> GetPaginatedAsync(int pageIndex, int pageSize);
        Task<PagedList<TEntity>> GetPaginatedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetByIdAsync(string id);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AddAsync(TEntity entity, string entityKey = "");
        Task<string> AddWithReturnAsync(TEntity entity, string entityKey = "");
        Task<bool> DeleteByIdAsync(string id);
    }
}
