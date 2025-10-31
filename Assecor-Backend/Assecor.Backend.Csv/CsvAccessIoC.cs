using Assecor.Backend.CsvAccess.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Assecor.Backend.CsvAccess
{
    public static class CsvAccessIoC
    {
        public static IServiceCollection AddCsvReader(this IServiceCollection services)
        {
            services.AddScoped(typeof(ICsvReader<>), typeof(CsvReader<>));
            services.AddScoped(typeof(ICsvWriter<>), typeof(CsvWriter<>));
            services.AddScoped<ICsvFileLocationHandler, CsvFileLocationHandler>();
            return services;
        }
    }
}
