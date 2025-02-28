using ATechnologiesAssignment.App.Contracts.IRepositories;
using ATechnologiesAssignment.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ATechnologiesAssignment.Services.Services.BackgroundServices
{
    public class TemporalBlockCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<TemporalBlockCleanupService> _logger;

        public TemporalBlockCleanupService(IServiceScopeFactory serviceScopeFactory,
            ILogger<TemporalBlockCleanupService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await RemoveExpiredTemporalBlocksAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while removing expired temporal blocks.");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        private async Task RemoveExpiredTemporalBlocksAsync()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var temporalBlockedCountryRepo = scope.ServiceProvider.GetRequiredService<IRepository<TemporalBlockedCountry>>();
                var blockedCountryRepo = scope.ServiceProvider.GetRequiredService<IRepository<BlockedCountry>>();

                var now = DateTime.UtcNow;
                var expiredBlocks = await temporalBlockedCountryRepo.FindAsync(tbc => tbc.BlockExpiredAt <= now);

                foreach (var expiredBlock in expiredBlocks)
                {
                    await temporalBlockedCountryRepo.DeleteByIdAsync(expiredBlock.CountryCode);
                    await blockedCountryRepo.DeleteByIdAsync(expiredBlock.CountryCode);
                    _logger.LogInformation($"Removed expired temporal block for country code: {expiredBlock.CountryCode}");
                }
            }
        }
    }
}