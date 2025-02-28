using ATechnologiesAssignment.App.Dtos.Common;
using ATechnologiesAssignment.App.Dtos.TemporalBlockedCountryDtos;
using ATechnologiesAssignment.App.Models;

namespace ATechnologiesAssignment.App.Contracts.IServices.ICountryServices
{
    public interface ICountryService
    {
        Task<BaseResponse> AddBlockedCountryAsync(CountryCodeDto countryCode);
        Task<BaseResponse> DeleteBlockedCountryAsync(CountryCodeDto countryCode);
        Task<BaseResponse> GetBlockedCountriesAsync(int page, int pageSize, string search);
        Task<bool> IsCountryBlockedAsync(string countryCode);
        Task<BaseResponse> BlockCountryTemporalyAsync(TemporalBlockedCountryDto dto);
    }
}
