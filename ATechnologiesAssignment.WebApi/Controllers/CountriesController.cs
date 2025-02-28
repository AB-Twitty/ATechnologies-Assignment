using ATechnologiesAssignment.App.Contracts.IServices.ICountryServices;
using ATechnologiesAssignment.App.Dtos.Common;
using ATechnologiesAssignment.App.Dtos.TemporalBlockedCountryDtos;
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

        [HttpGet("blocked")]
        public async Task<IActionResult> GetAllBlockedCountries(int page = 1, int pageSize = 25, string search = "")
        {
            var response = await _countryService.GetBlockedCountriesAsync(page, pageSize, search);
            return HandleResponse(response);
        }

        [HttpPost("block")]
        public async Task<IActionResult> AddBlockedCountry([FromBody] CountryCodeDto countryCode)
        {
            var response = await _countryService.AddBlockedCountryAsync(countryCode);
            return HandleResponse(response);
        }

        [HttpDelete("block/{countryCode}")]
        public async Task<IActionResult> DeleteBlockedCountry(string countryCode)
        {
            var response = await _countryService.DeleteBlockedCountryAsync(new CountryCodeDto { CountryCode = countryCode });
            return HandleResponse(response);
        }

        [HttpPost("temporal-block")]
        public async Task<IActionResult> BlockCountryTemporaly([FromBody] TemporalBlockedCountryDto dto)
        {
            var response = await _countryService.BlockCountryTemporalyAsync(dto);
            return HandleResponse(response);
        }
    }
}
