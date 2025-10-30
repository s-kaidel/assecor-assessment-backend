using Assecor.Backend.Configuration;
using Assecor.Backend.CsvAccess;
using Assecor.Backend.Dal.IoC;
using Assecor.Backend.Mappings;
using Assecor.Backend.Services.IoC;

namespace Assecor.Backend.Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDalServices();
            services.AddServiceLayerServices();
            services.AddHttpContextAccessor();
            services.AddHttpClient();
            services.AddCsvReader();
            services.AddMappings();

            services.Configure<CsvSettings>(Configuration.GetSection(nameof(CsvSettings)));
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Setup development ASP.NET pipeline.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseMiddleware<ExceptionHandler>();
            app.UseHttpsRedirection();
            
            // Endpoint mapping
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
