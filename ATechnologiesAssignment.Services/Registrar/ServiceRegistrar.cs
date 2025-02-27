using ATechnologiesAssignment.App.Contracts.IServices.ICountryServices;
using ATechnologiesAssignment.App.Contracts.IServices.IIpGeolocationServices;
using ATechnologiesAssignment.App.Contracts.IValidators;
using ATechnologiesAssignment.App.Dtos.Common;
using ATechnologiesAssignment.Services.Services.CountryServices;
using ATechnologiesAssignment.Services.Services.IpGeolocationServices;
using ATechnologiesAssignment.Services.Validators.Common;
using Microsoft.Extensions.DependencyInjection;

namespace ATechnologiesAssignment.Services.Registrar
{
    public static class ServiceRegistrar
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            // register validators
            services.AddTransient<IValidator<CountryCodeDto>, CountryCodeValidator>();
            services.AddTransient<IValidator<IpAddressDto>, IpAddressValidator>();


            // register services
            services.AddTransient<ICountryService, CountryService>();
            services.AddHttpClient<ICountryInfoService, CountryInfoService>(client =>
            {
                client.BaseAddress = new Uri("https://restcountries.com/v3.1/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddHttpClient<IIpGeolocationApiService, IpGeolocationApiService>(client =>
            {
                client.BaseAddress = new Uri("https://api.ipapi.com/api/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            return services;
        }
    }
}
