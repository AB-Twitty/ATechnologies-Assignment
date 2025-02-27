using ATechnologiesAssignment.App.Configurations;
using ATechnologiesAssignment.App.Contracts.IServices.IIpGeolocationServices;
using ATechnologiesAssignment.Services.Services.IpGeolocationServices.Exceptions;
using ATechnologiesAssignment.Services.Services.IpGeolocationServices.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ATechnologiesAssignment.Services.Services.IpGeolocationServices
{
    public class IpGeolocationApiService : IIpGeolocationApiService
    {
        #region Fields

        private readonly IpGeolocationSettings _ipGeolocationSettings;
        private readonly HttpClient _httpClient;

        #endregion

        #region Ctor

        public IpGeolocationApiService(IOptions<IpGeolocationSettings> options, HttpClient httpClient)
        {
            _ipGeolocationSettings = options.Value;
            _httpClient = httpClient;
        }

        #endregion

        #region Utils

        protected virtual async Task HandleErrorResponse(HttpResponseMessage response)
        {
            var errorContent = await response.Content.ReadAsStringAsync();

            var ipApiError = JsonSerializer.Deserialize<IpApiErrorResponse>(errorContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new JsonException("Error parsing ip data error response.");

            throw new IpApiErrorException(ipApiError);
        }

        #endregion

        #region Methods

        public async Task<dynamic> GetGeolocationByIpAsync(string ip)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{ip}?access_key={_ipGeolocationSettings.ApiKey}");
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                // check if request failed 
                if (responseContent.Contains("error"))
                {
                    await HandleErrorResponse(response);
                }

                return JsonSerializer.Deserialize<dynamic>(responseContent)
                    ?? throw new JsonException("Error parsing geolocation data.");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error fetching geolocation data", ex);
            }
        }

        public async Task<string> GetCountryCodeByIpAsync(string ip)
        {
            var geolocation = await GetGeolocationByIpAsync(ip);
            if (geolocation is JsonElement jsonElement && jsonElement.TryGetProperty("country_code", out var countryCode))
            {
                return countryCode.GetString() ?? throw new JsonException("Country code not found in geolocation data.");
            }

            throw new JsonException("Country code not found in geolocation data.");
        }

        #endregion
    }
}
