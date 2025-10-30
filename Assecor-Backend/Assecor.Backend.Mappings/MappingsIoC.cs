using Microsoft.Extensions.DependencyInjection;

namespace Assecor.Backend.Mappings
{
    public static class MappingsIoC
    {
        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            services.AddScoped<ICsvPersonMapper, CsvPersonMapper>();
            services.AddScoped<IPersonMapper, PersonMapper>();
            return services;
        }
    }
}
