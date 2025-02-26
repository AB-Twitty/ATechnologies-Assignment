using ATechnologiesAssignment.App.Contracts.IValidators;
using ATechnologiesAssignment.App.Contracts.IValidators.Models;
using ATechnologiesAssignment.App.Dtos.Common;
using System.Net;

namespace ATechnologiesAssignment.Services.Validators.Common
{
    public class IpAddressValidator : IValidator<IpAddressDto>
    {
        public ValidationResult Validate(IpAddressDto entity)
        {
            var result = new ValidationResult();

            if (string.IsNullOrEmpty(entity.IpAddress))
            {
                result.IsValid = false;
                result.Errors.Add("IpAddress", "IP address is required.");
                return result;
            }

            if (!IPAddress.TryParse(entity.IpAddress, out _))
            {
                result.IsValid = false;
                result.Errors.Add("IpAddress", "Invalid IP address format.");
                return result;
            }

            result.IsValid = true;
            return result;
        }
    }
}