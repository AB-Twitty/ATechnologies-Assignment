using ATechnologiesAssignment.Domain.Entities;
using System.Collections.Concurrent;

namespace ATechnologiesAssignment.Domain.Context
{
    public class DataStoreContext
    {
        public ConcurrentDictionary<string, BlockedCountry> BlockedCountries { get; set; }
        public ConcurrentDictionary<string, TemporalBlockedCountry> TemporalBlockedCountries { get; set; }
        public ConcurrentDictionary<string, BlockedAttemptLog> BlockedAttemptLogs { get; set; }


        public DataStoreContext()
        {
            BlockedCountries = new ConcurrentDictionary<string, BlockedCountry>();
            TemporalBlockedCountries = new ConcurrentDictionary<string, TemporalBlockedCountry>();
            BlockedAttemptLogs = new ConcurrentDictionary<string, BlockedAttemptLog>();
        }


        public ConcurrentDictionary<string, TEntity>? GetDataDictionary<TEntity>()
        {
            if (typeof(TEntity) == typeof(BlockedCountry))
            {
                return BlockedCountries as ConcurrentDictionary<string, TEntity>;
            }
            else if (typeof(TEntity) == typeof(TemporalBlockedCountry))
            {
                return TemporalBlockedCountries as ConcurrentDictionary<string, TEntity>;
            }
            else if (typeof(TEntity) == typeof(BlockedAttemptLog))
            {
                return BlockedAttemptLogs as ConcurrentDictionary<string, TEntity>;
            }
            else
            {
                throw new KeyNotFoundException($"Data dictionary for {typeof(TEntity).Name} not found.");
            }
        }
    }
}
