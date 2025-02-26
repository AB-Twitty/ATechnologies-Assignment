using ATechnologiesAssignment.App.Contracts.IRepositories;
using ATechnologiesAssignment.App.Contracts.IServices.ICountryServices;
using ATechnologiesAssignment.App.Contracts.IValidators;
using ATechnologiesAssignment.App.Dtos.Common;
using ATechnologiesAssignment.App.Models;
using ATechnologiesAssignment.Domain.Entities;
using ATechnologiesAssignment.Services.Services.Base;

namespace ATechnologiesAssignment.Services.Services.CountryServices
{
    public class CountryService : BaseService, ICountryService
    {
        #region Fields

        private readonly IRepository<BlockedCountry> _blockedCountryRepo;
        private readonly IValidator<CountryCodeDto> _countryCodeValidator;
        private readonly ICountryInfoService _countryInfoService;

        #endregion

        #region Ctor

        public CountryService(IRepository<BlockedCountry> blockedCountryRepo,
            IValidator<CountryCodeDto> countryCodeValidator,
            ICountryInfoService countryInfoService)
        {
            _blockedCountryRepo = blockedCountryRepo;
            _countryCodeValidator = countryCodeValidator;
            _countryInfoService = countryInfoService;
        }

        #endregion

        #region Methods

        public async Task<BaseResponse> AddBlockedCountryAsync(CountryCodeDto countryCode)
        {
            var validationResult = _countryCodeValidator.Validate(countryCode);

            if (!validationResult.IsValid)
            {
                return ValidationError(validationResult.Errors);
            }

            // Check if country is already blocked
            if (await _blockedCountryRepo.AnyAsync(bc => bc.CountryCode == countryCode.CountryCode))
            {
                return Error("Country is already blocked");
            }

            // fetch the country name by country code
            var countryName = await _countryInfoService.GetCountryNameByCodeAsync(countryCode.CountryCode);

            var blockedCountry = new BlockedCountry
            {
                CountryCode = countryCode.CountryCode,
                CountryName = countryName
            };

            if (!await _blockedCountryRepo.AddAsync(blockedCountry))
            {
                return Error("Failed to block country");
            }

            return Created(blockedCountry, "Country blocked successfully");
        }

        #endregion
    }
}
