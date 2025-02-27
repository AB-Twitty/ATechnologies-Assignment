using ATechnologiesAssignment.App.Contracts.IServices.IIpGeolocationServices;
using ATechnologiesAssignment.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace ATechnologiesAssignment.WebApi.Controllers
{
    public class IpController : DefaultController
    {
        private readonly IIpGeolocationService _ipGeolocationService;

        public IpController(IIpGeolocationService ipGeolocationService)
        {
            _ipGeolocationService = ipGeolocationService;
        }

        [HttpGet("lookup")]
        public async Task<IActionResult> LookupCountryViaIp(string ip = "")
        {
            var response = await _ipGeolocationService.LookupCountryViaIpAsync(ip);
            return HandleResponse(response);
        }
    }
}
