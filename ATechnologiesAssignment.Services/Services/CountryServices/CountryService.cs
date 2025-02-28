using ATechnologiesAssignment.App.Contracts.IRepositories;
using ATechnologiesAssignment.App.Contracts.IServices.ICountryServices;
using ATechnologiesAssignment.App.Contracts.IValidators;
using ATechnologiesAssignment.App.Dtos.Common;
using ATechnologiesAssignment.App.Dtos.TemporalBlockedCountryDtos;
using ATechnologiesAssignment.App.Models;
using ATechnologiesAssignment.Domain.Entities;
using ATechnologiesAssignment.Services.Services.Base;
using System.Net;

namespace ATechnologiesAssignment.Services.Services.CountryServices
{
    public class CountryService : BaseService, ICountryService
    {
        #region Fields

        private readonly IRepository<BlockedCountry> _blockedCountryRepo;
        private readonly IValidator<CountryCodeDto> _countryCodeValidator;
        private readonly ICountryInfoService _countryInfoService;
        private readonly IRepository<TemporalBlockedCountry> _temporalBlockedCountryRepo;
        private readonly IValidator<TemporalBlockedCountryDto> _temporalBlockedCountryDtoValidator;

        #endregion

        #region Ctor

        public CountryService(IRepository<BlockedCountry> blockedCountryRepo,
            IValidator<CountryCodeDto> countryCodeValidator,
            ICountryInfoService countryInfoService,
            IRepository<TemporalBlockedCountry> temporalBlockedCountryRepo,
            IValidator<TemporalBlockedCountryDto> temporalBlockedCountryDtoValidator)
        {
            _blockedCountryRepo = blockedCountryRepo;
            _countryCodeValidator = countryCodeValidator;
            _countryInfoService = countryInfoService;
            _temporalBlockedCountryRepo = temporalBlockedCountryRepo;
            _temporalBlockedCountryDtoValidator = temporalBlockedCountryDtoValidator;
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

            if (!await _blockedCountryRepo.AddAsync(blockedCountry, blockedCountry.CountryCode))
            {
                return Error("Failed to block country");
            }

            return Created(blockedCountry, "Country blocked successfully");
        }

        public async Task<BaseResponse> BlockCountryTemporalyAsync(TemporalBlockedCountryDto dto)
        {
            var validationResult = _temporalBlockedCountryDtoValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                return ValidationError(validationResult.Errors);
            }

            if (await _temporalBlockedCountryRepo.AnyAsync(tbc => tbc.CountryCode == dto.CountryCode))
            {
                return Error($"Country with code '{dto.CountryCode}' already temporaly blocked.", HttpStatusCode.Conflict);
            }

            var blockedCountry = new BlockedCountry
            {
                CountryCode = dto.CountryCode,
                CountryName = await _countryInfoService.GetCountryNameByCodeAsync(dto.CountryCode)
            };

            if (!await _blockedCountryRepo.AddAsync(blockedCountry, blockedCountry.CountryCode))
            {
                return Error("Failed to temporaly block country");
            }

            var temporalBlockCountry = new TemporalBlockedCountry
            {
                CountryCode = dto.CountryCode,
                BlockExpiredAt = DateTime.UtcNow.AddMinutes(dto.DurationMinutes),
            };

            if (!await _temporalBlockedCountryRepo.AddAsync(temporalBlockCountry, temporalBlockCountry.CountryCode))
            {
                await _blockedCountryRepo.DeleteByIdAsync(blockedCountry.CountryCode);
                return Error("Failed to temporaly block country");
            }

            return Success($"Country temporaly blocked until {temporalBlockCountry.BlockExpiredAt:yyyy-MM-dd HH:mm:ss} UTC");
        }

        public async Task<BaseResponse> DeleteBlockedCountryAsync(CountryCodeDto countryCode)
        {
            var validationResult = _countryCodeValidator.Validate(countryCode);

            if (!validationResult.IsValid)
            {
                return ValidationError(validationResult.Errors);
            }

            // Check if country is not blocked
            if (!await _blockedCountryRepo.AnyAsync(bc => bc.CountryCode == countryCode.CountryCode))
            {
                return NotFound($"Country with code '{countryCode.CountryCode}' is not blocked.");
            }

            if (await _blockedCountryRepo.DeleteByIdAsync(countryCode.CountryCode))
                return Success("Country unblocked successfully");

            return Error("Failed to unblock country");
        }

        public async Task<BaseResponse> GetBlockedCountriesAsync(int page, int pageSize, string search)
        {
            var blockedCountries = await _blockedCountryRepo.GetPaginatedAsync(
                pageIndex: page,
                pageSize: pageSize,
                predicate: bc => string.IsNullOrEmpty(search)
                    || bc.CountryCode.Contains(search, StringComparison.CurrentCultureIgnoreCase)
                    || bc.CountryName.Contains(search, StringComparison.CurrentCultureIgnoreCase)
                );

            return Success(blockedCountries);
        }

        public async Task<bool> IsCountryBlockedAsync(string countryCode)
        {
            return await _blockedCountryRepo.AnyAsync(bc => bc.CountryCode.Equals(countryCode, StringComparison.CurrentCultureIgnoreCase));
        }

        #endregion
    }
}
