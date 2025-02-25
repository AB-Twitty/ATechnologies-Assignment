using ATechnologiesAssignment.Domain.Entities;
using System.Collections.Concurrent;

namespace ATechnologiesAssignment.Domain.Context
{
    public class DataStoreContext
    {
        public ConcurrentDictionary<string, BlockedCountry> BlockedCountries { get; set; }
        public ConcurrentDictionary<string, TemporalBlockedCountry> TemporalBlockedCountries { get; set; }
        public ConcurrentDictionary<string, BlockedAttemptLog> BlockedAttemptLogs { get; set; }
    }
}
