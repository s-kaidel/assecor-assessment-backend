using Assecor.Backend.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Assecor.Backend.Services.IoC
{
    public static class ServiceLayerServices
    {
        public static IServiceCollection AddServiceLayerServices(this IServiceCollection services)
        {
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IValidationService, ValidationService>();
            return services;
        }
    }
}
