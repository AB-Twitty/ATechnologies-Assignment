using ATechnologies.Persistence.Repositories;
using ATechnologiesAssignment.App.Contracts.IRepositories;
using ATechnologiesAssignment.Domain.Context;
using Microsoft.Extensions.DependencyInjection;

namespace ATechnologies.Persistence.Registrar
{
    public static class ServiceRegistrar
    {
        public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services)
        {
            services.AddSingleton<DataStoreContext>();
            services.AddScoped(typeof(IRepository<>), typeof(InMemoryRepository<>));

            return services;
        }
    }
}
