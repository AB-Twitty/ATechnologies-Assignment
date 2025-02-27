using ATechnologiesAssignment.App.Models;

namespace ATechnologiesAssignment.App.Contracts.IServices.IIpGeolocationServices
{
    public interface IIpGeolocationService
    {
        Task<BaseResponse> LookupCountryViaIpAsync(string ip = "");
        Task<BaseResponse> CheckIpBlockedAsync();
    }
}
