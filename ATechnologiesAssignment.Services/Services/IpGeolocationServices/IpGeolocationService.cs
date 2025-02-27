using ATechnologiesAssignment.App.Contracts.IRepositories;
using ATechnologiesAssignment.App.Contracts.IServices.ICountryServices;
using ATechnologiesAssignment.App.Contracts.IServices.IIpGeolocationServices;
using ATechnologiesAssignment.App.Contracts.IValidators;
using ATechnologiesAssignment.App.Dtos.Common;
using ATechnologiesAssignment.App.Models;
using ATechnologiesAssignment.Domain.Entities;
using ATechnologiesAssignment.Services.Services.Base;
using ATechnologiesAssignment.Services.Services.IpGeolocationServices.Exceptions;
using Microsoft.AspNetCore.Http;

namespace ATechnologiesAssignment.Services.Services.IpGeolocationServices
{
    public class IpGeolocationService : BaseService, IIpGeolocationService
    {
        #region Fields

        private readonly IIpGeolocationApiService _ipGeolocationApiService;
        private readonly IValidator<IpAddressDto> _ipAddressValidator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICountryService _countryService;
        private readonly IRepository<BlockedAttemptLog> _blockedAttemptLogRepo;

        #endregion

        #region Ctor

        public IpGeolocationService(IIpGeolocationApiService ipGeolocationApiService,
            IValidator<IpAddressDto> ipAddressValidator,
            IHttpContextAccessor httpContextAccessor,
            ICountryService countryService,
            IRepository<BlockedAttemptLog> blockedAttemptLogRepo)
        {
            _ipGeolocationApiService = ipGeolocationApiService;
            _ipAddressValidator = ipAddressValidator;
            _httpContextAccessor = httpContextAccessor;
            _countryService = countryService;
            _blockedAttemptLogRepo = blockedAttemptLogRepo;
        }

        #endregion

        #region Utils

        protected virtual string GetIpFromHttpContext()
        {
            var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString()
                ?? throw new InvalidOperationException("IP address not found in HTTP context.");

            return ip;
        }

        #endregion

        #region Methods

        public async Task<BaseResponse> LookupCountryViaIpAsync(string ip = "")
        {
            if (string.IsNullOrEmpty(ip))
            {
                ip = GetIpFromHttpContext();
            }

            var validationResult = _ipAddressValidator.Validate(new IpAddressDto { IpAddress = ip });
            if (!validationResult.IsValid)
            {
                return ValidationError(validationResult.Errors);
            }

            try
            {
                var geolocation = await _ipGeolocationApiService.GetGeolocationByIpAsync(ip);
                return Success(geolocation);
            }
            catch (IpApiErrorException ex)
            {
                return Error(ex.ErrorResponse.Error.Info);
            }
        }

        public async Task<BaseResponse> CheckIpBlockedAsync()
        {
            var ip = GetIpFromHttpContext();

            try
            {
                var countryCode = await _ipGeolocationApiService.GetCountryCodeByIpAsync(ip);

                var isCountryBlocked = await _countryService.IsCountryBlockedAsync(countryCode);

                var msg = isCountryBlocked ? "Country is blocked." : "Country is not blocked.";

                var attemptLog = new BlockedAttemptLog
                {
                    IpAddress = ip,
                    CountryCode = countryCode,
                    IsBlocked = isCountryBlocked,
                    Timestamp = DateTime.UtcNow,
                    UserAgent = _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString() ?? "Unknown"
                };

                var id = await _blockedAttemptLogRepo.AddWithReturnAsync(attemptLog);

                if (string.IsNullOrEmpty(id))
                {
                    return Error("Failed to log attempt.");
                }

                attemptLog.Id = id;

                return Success(attemptLog, msg);
            }
            catch (IpApiErrorException ex)
            {
                return Error(ex.ErrorResponse.Error.Info);
            }
        }

        #endregion
    }
}
