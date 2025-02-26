using ATechnologiesAssignment.App.Contracts.IServices.ICountryServices;
using System.Text.Json;

namespace ATechnologiesAssignment.Services.Services.CountryServices
{
    public class CountryInfoService : ICountryInfoService
    {
        #region Fields

        private readonly HttpClient _httpClient;

        #endregion

        #region Ctor

        public CountryInfoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #endregion

        #region Methods

        public async Task<string> GetCountryNameByCodeAsync(string countryCode)
        {
            try
            {
                var response = await _httpClient.GetAsync($"alpha/{countryCode}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(content);
                var countryName = jsonDoc.RootElement[0].GetProperty("name").GetProperty("common").GetString();

                if (string.IsNullOrEmpty(countryName))
                {
                    throw new Exception("Country name not found");
                }

                return countryName;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error fetching country data", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Error parsing country data", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred", ex);
            }
        }

        #endregion
    }
}