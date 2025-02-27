using ATechnologiesAssignment.App.Contracts.IServices.IBlockedAttemptLogServices;
using ATechnologiesAssignment.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace ATechnologiesAssignment.WebApi.Controllers
{
    public class LogsController : DefaultController
    {
        private readonly IBlockedAttemptLogService _blockedAttemptLogService;

        public LogsController(IBlockedAttemptLogService blockedAttemptLogService)
        {
            _blockedAttemptLogService = blockedAttemptLogService;
        }

        [HttpGet("blocked-attempts")]
        public async Task<IActionResult> GetBlockedAttemptLogsPaginated(int page = 1, int pageSize = 25, string ip = "", string countryCode = "")
        {
            var response = await _blockedAttemptLogService.GetBlockAttemptLogPaginatedAsync(page, pageSize, ip, countryCode);
            return HandleResponse(response);
        }
    }
}
