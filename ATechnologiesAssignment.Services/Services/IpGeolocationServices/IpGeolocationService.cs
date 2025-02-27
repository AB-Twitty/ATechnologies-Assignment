using ATechnologiesAssignment.App.Contracts.IServices.IIpGeolocationServices;
using ATechnologiesAssignment.App.Contracts.IValidators;
using ATechnologiesAssignment.App.Dtos.Common;
using ATechnologiesAssignment.App.Models;
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

        #endregion

        #region Ctor

        public IpGeolocationService(IIpGeolocationApiService ipGeolocationApiService,
            IValidator<IpAddressDto> ipAddressValidator,
            IHttpContextAccessor httpContextAccessor)
        {
            _ipGeolocationApiService = ipGeolocationApiService;
            _ipAddressValidator = ipAddressValidator;
            _httpContextAccessor = httpContextAccessor;
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

        #endregion
    }
}
