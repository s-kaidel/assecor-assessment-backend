using Assecor.Backend.Dal.Contracts;
using Assecor.Backend.Dal.Provider;
using Microsoft.Extensions.DependencyInjection;

namespace Assecor.Backend.Dal.IoC
{
    public static class DalServices
    {
        public static IServiceCollection AddDalServices(this IServiceCollection services)
        {
            services.AddScoped<ICsvPersonProvider, CsvPersonProvider>();
            return services;
        }
    }
}
