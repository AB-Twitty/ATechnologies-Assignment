using ATechnologiesAssignment.App.Contracts.IRepositories;
using ATechnologiesAssignment.App.Helpers;
using ATechnologiesAssignment.Domain.Base;
using ATechnologiesAssignment.Domain.Context;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace ATechnologies.Persistence.Repositories
{
    public abstract class InMemoryRepository<TEntity>(DataStoreContext dataStore) : IRepository<TEntity> where TEntity : BaseEntity
    {
        #region Fields

        protected readonly DataStoreContext DataStore = dataStore;
        protected readonly ConcurrentDictionary<string, TEntity> DataDict =
            dataStore.GetDataDictionary<TEntity>() ?? throw new KeyNotFoundException($"Data dictionary for {typeof(TEntity).Name} not found.");

        #endregion

        #region Methods

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Task.FromResult(DataDict.Values);
        }

        public virtual async Task<TEntity> GetByIdAsync(string id)
        {
            return await Task.FromResult(DataDict[id]);
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.FromResult(DataDict.Values.Where(predicate.Compile()));
        }

        public virtual async Task<PagedList<TEntity>> GetPaginatedAsync(int pageIndex, int pageSize)
        {
            return await Task.FromResult(
                new PagedList<TEntity>(
                    data: DataDict.Values.Skip((pageIndex - 1) * pageSize).Take(pageSize),
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    totalCount: DataDict.Count
                    )
                );
        }

        public virtual async Task<PagedList<TEntity>> GetPaginatedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.FromResult(
                new PagedList<TEntity>(
                    data: DataDict.Values.Where(predicate.Compile()).Skip((pageIndex - 1) * pageSize).Take(pageSize),
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    totalCount: DataDict.Values.Count(predicate.Compile())
                    )
                );
        }

        public virtual async Task<bool> AddAsync(TEntity entity, string entityKey = "")
        {
            entityKey = string.IsNullOrEmpty(entityKey) ? entityKey : Guid.NewGuid().ToString();

            if (string.IsNullOrEmpty(entityKey) && DataDict.ContainsKey(entityKey))
                return false;

            return await Task.FromResult(DataDict.TryAdd(entityKey, entity));
        }

        public virtual async Task<TEntity> AddWithReturnAsync(TEntity entity, string entityKey = "")
        {
            entityKey = string.IsNullOrEmpty(entityKey) ? entityKey : Guid.NewGuid().ToString();

            if (string.IsNullOrEmpty(entityKey) && DataDict.ContainsKey(entityKey))
                return default;

            if (await Task.FromResult(DataDict.TryAdd(entityKey, entity)))
                return DataDict[entityKey];

            return default;
        }

        public virtual async Task DeleteByIdAsync(string id)
        {
            await Task.FromResult(DataDict.TryRemove(id, out _));
        }

        #endregion
    }
}
