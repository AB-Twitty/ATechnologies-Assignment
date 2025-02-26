using ATechnologiesAssignment.App.Dtos.Common;
using ATechnologiesAssignment.App.Models;

namespace ATechnologiesAssignment.App.Contracts.IServices.ICountryServices
{
    public interface ICountryService
    {
        Task<BaseResponse> AddBlockedCountryAsync(CountryCodeDto countryCode);
        Task<BaseResponse> DeleteBlockedCountryAsync(CountryCodeDto countryCode);
    }
}
