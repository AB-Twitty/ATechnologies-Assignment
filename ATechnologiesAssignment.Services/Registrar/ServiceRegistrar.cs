using ATechnologiesAssignment.App.Contracts.IServices.IBlockedAttemptLogServices;
using ATechnologiesAssignment.App.Contracts.IServices.ICountryServices;
using ATechnologiesAssignment.App.Contracts.IServices.IIpGeolocationServices;
using ATechnologiesAssignment.App.Contracts.IValidators;
using ATechnologiesAssignment.App.Dtos.Common;
using ATechnologiesAssignment.App.Dtos.TemporalBlockedCountryDtos;
using ATechnologiesAssignment.Services.Services.BackgroundServices;
using ATechnologiesAssignment.Services.Services.BlockedAttemptLogServices;
using ATechnologiesAssignment.Services.Services.CountryServices;
using ATechnologiesAssignment.Services.Services.IpGeolocationServices;
using ATechnologiesAssignment.Services.Validators.Common;
using ATechnologiesAssignment.Services.Validators.TemporalBlockedCountry;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ATechnologiesAssignment.Services.Registrar
{
    public static class ServiceRegistrar
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            // register http context accessor
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // register validators
            services.AddTransient<IValidator<CountryCodeDto>, CountryCodeValidator>();
            services.AddTransient<IValidator<IpAddressDto>, IpAddressValidator>();
            services.AddTransient<IValidator<TemporalBlockedCountryDto>, TemporalBlockedCountryDtoValidator>();

            // register services
            services.AddTransient<ICountryService, CountryService>();
            services.AddHttpClient<ICountryInfoService, CountryInfoService>(client =>
            {
                client.BaseAddress = new Uri("https://restcountries.com/v3.1/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddTransient<IIpGeolocationService, IpGeolocationService>();
            services.AddHttpClient<IIpGeolocationApiService, IpGeolocationApiService>(client =>
            {
                client.BaseAddress = new Uri("https://api.ipapi.com/api/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddTransient<IBlockedAttemptLogService, BlockedAttemptLogService>();

            // register background service
            services.AddHostedService<TemporalBlockCleanupService>();

            return services;
        }
    }
}
