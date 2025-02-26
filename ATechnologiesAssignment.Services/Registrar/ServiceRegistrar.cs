using ATechnologiesAssignment.App.Contracts.IValidators;
using ATechnologiesAssignment.App.Dtos.Common;
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

            return services;
        }
    }
}
