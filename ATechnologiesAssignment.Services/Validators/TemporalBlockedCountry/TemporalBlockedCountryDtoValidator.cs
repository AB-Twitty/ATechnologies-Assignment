using ATechnologiesAssignment.App.Contracts.IValidators;
using ATechnologiesAssignment.App.Contracts.IValidators.Models;
using ATechnologiesAssignment.App.Dtos.Common;
using ATechnologiesAssignment.App.Dtos.TemporalBlockedCountryDtos;

namespace ATechnologiesAssignment.Services.Validators.TemporalBlockedCountry
{
    public class TemporalBlockedCountryDtoValidator : IValidator<TemporalBlockedCountryDto>
    {
        private readonly IValidator<CountryCodeDto> _countryCodeValidator;

        public TemporalBlockedCountryDtoValidator(IValidator<CountryCodeDto> countryCodeValidator)
        {
            _countryCodeValidator = countryCodeValidator;
        }

        public ValidationResult Validate(TemporalBlockedCountryDto entity)
        {
            var validationResult = _countryCodeValidator.Validate(new CountryCodeDto { CountryCode = entity.CountryCode });

            // Ensure durationMinutes is between 1 and 1440 (24 hours)
            if (entity.DurationMinutes < 1 || entity.DurationMinutes > 1440)
            {
                validationResult.Errors.Add(nameof(entity.DurationMinutes), "DurationMinutes must be between 1 and 1440");
            }

            validationResult.IsValid = validationResult.Errors.Count == 0;
            return validationResult;
        }
    }
}
