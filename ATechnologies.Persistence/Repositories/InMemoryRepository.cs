using ATechnologiesAssignment.App.Contracts.IRepositories;
using ATechnologiesAssignment.App.Helpers;
using ATechnologiesAssignment.Domain.Base;
using ATechnologiesAssignment.Domain.Context;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace ATechnologies.Persistence.Repositories
{
    public class InMemoryRepository<TEntity>(DataStoreContext dataStore) : IRepository<TEntity> where TEntity : BaseEntity
    {
        #region Fields

        protected readonly DataStoreContext DataStore = dataStore;
        protected readonly ConcurrentDictionary<string, TEntity> DataDict =
            dataStore.GetDataDictionary<TEntity>() ?? throw new KeyNotFoundException($"Data dictionary for {typeof(TEntity).Name} not found.");

        #endregion

        #region Methods

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Task.Run(() => DataDict.Values);
        }

        public virtual async Task<TEntity?> GetByIdAsync(string id)
        {
            return await Task.Run(() =>
            {
                if (DataDict.TryGetValue(id, out TEntity? entity))
                {
                    return entity;
                }
                return null;
            });
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.Run(() => DataDict.Values.Where(predicate.Compile()));
        }

        public virtual async Task<PagedList<TEntity>> GetPaginatedAsync(int pageIndex, int pageSize)
        {
            return await Task.Run(() =>
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
            pageIndex = pageIndex < 1 ? 1 : pageIndex;

            return await Task.Run(() =>
                new PagedList<TEntity>(
                    data: DataDict.Values.Where(predicate.Compile()).Skip((pageIndex - 1) * pageSize).Take(pageSize),
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    totalCount: DataDict.Values.Count(predicate.Compile())
                    )
                );
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.Run(() => DataDict.Values.Any(predicate.Compile()));
        }

        public virtual async Task<bool> AddAsync(TEntity entity, string entityKey = "")
        {
            entityKey = string.IsNullOrEmpty(entityKey) ? Guid.NewGuid().ToString() : entityKey;

            if (string.IsNullOrEmpty(entityKey) && DataDict.ContainsKey(entityKey))
                return false;

            return await Task.Run(() => DataDict.TryAdd(entityKey, entity));
        }

        public virtual async Task<string> AddWithReturnAsync(TEntity entity, string entityKey = "")
        {
            entityKey = string.IsNullOrEmpty(entityKey) ? Guid.NewGuid().ToString() : entityKey;

            if (string.IsNullOrEmpty(entityKey) && DataDict.ContainsKey(entityKey))
                return string.Empty;

            if (await Task.Run(() => DataDict.TryAdd(entityKey, entity)))
                return entityKey;

            return string.Empty;
        }

        public virtual async Task<bool> DeleteByIdAsync(string id)
        {
            return await Task.Run(() => DataDict.TryRemove(id, out _));
        }

        #endregion
    }
}
