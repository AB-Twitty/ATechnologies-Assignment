using ATechnologiesAssignment.App.Contracts.IRepositories;
using ATechnologiesAssignment.App.Contracts.IServices.IBlockedAttemptLogServices;
using ATechnologiesAssignment.App.Models;
using ATechnologiesAssignment.Domain.Entities;
using ATechnologiesAssignment.Services.Services.Base;

namespace ATechnologiesAssignment.Services.Services.BlockedAttemptLogServices
{
    public class BlockedAttemptLogService : BaseService, IBlockedAttemptLogService
    {
        #region Fields 

        private readonly IRepository<BlockedAttemptLog> _blockedAttempLog;

        #endregion

        #region Ctor

        public BlockedAttemptLogService(IRepository<BlockedAttemptLog> blockedAttempLog)
        {
            _blockedAttempLog = blockedAttempLog;
        }

        #endregion

        #region Methods

        public async Task<BaseResponse> GetBlockAttemptLogPaginatedAsync(int page = 1, int pageSize = 25, string ip = "", string countryCode = "")
        {
            var attemptLogs = await _blockedAttempLog.GetPaginatedAsync(
                pageIndex: page,
                pageSize: pageSize,
                predicate: l => (string.IsNullOrEmpty(ip) || l.IpAddress == ip) &&
                    (string.IsNullOrEmpty(countryCode) || l.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase))
            );

            return Success(attemptLogs);
        }

        #endregion
    }
}
