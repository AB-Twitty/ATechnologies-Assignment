using ATechnologiesAssignment.App.Models;

namespace ATechnologiesAssignment.App.Contracts.IServices.IBlockedAttemptLogServices
{
    public interface IBlockedAttemptLogService
    {
        Task<BaseResponse> GetBlockAttemptLogPaginatedAsync(int page = 1, int pageSize = 25, string ip = "", string countryCode = "");
    }
}
