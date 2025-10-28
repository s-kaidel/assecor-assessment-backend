using CsvHelper;
using Microsoft.Extensions.DependencyInjection;

namespace Assecor.Backend.CsvAccess
{
    public static class CsvReaderIoC
    {
        public static IServiceCollection AddCsvReader(this IServiceCollection services)
        {
            services.AddScoped(typeof(ICsvReader<>), typeof(CsvReader<>));
            return services;
        }
    }
}
