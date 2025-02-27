using ATechnologiesAssignment.App.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ATechnologiesAssignment.App.Registrar
{
    public static class ServiceRegistrar
    {
        public static IServiceCollection RegisterApplication(this IServiceCollection services, IConfiguration config)
        {
            // Register Ip Geolocation Settings
            services.RegisterIpGeolocation(config);

            return services;
        }

        private static IServiceCollection RegisterIpGeolocation(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<IpGeolocationSettings>(config.GetSection("IpGeolocation"));

            return services;
        }
    }
}
