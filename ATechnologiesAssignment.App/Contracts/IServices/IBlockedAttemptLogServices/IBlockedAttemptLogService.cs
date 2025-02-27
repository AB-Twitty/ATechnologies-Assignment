using ATechnologiesAssignment.App.Models;

namespace ATechnologiesAssignment.App.Contracts.IServices.IBlockedAttemptLogServices
{
    public interface IBlockedAttemptLogService
    {
        Task<BaseResponse> LogBlockedAttemptAsync(string ipAddress);
    }
}
