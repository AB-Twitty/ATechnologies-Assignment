using ATechnologiesAssignment.App.Contracts.IServices.ICountryServices;
using ATechnologiesAssignment.App.Dtos.Common;
using ATechnologiesAssignment.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace ATechnologiesAssignment.WebApi.Controllers
{
    public class CountriesController : DefaultController
    {
        private readonly ICountryService _countryService;

        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpPost("block")]
        public async Task<IActionResult> AddBlockedCountry([FromBody] CountryCodeDto countryCode)
        {
            var response = await _countryService.AddBlockedCountryAsync(countryCode);
            return HandleResponse(response);
        }
    }
}
