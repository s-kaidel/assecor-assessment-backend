using Assecor.Backend.Configuration;
using Assecor.Backend.Dal.IoC;
using Assecor.Services.IoC;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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

            services.Configure<CsvOptions>(Configuration.GetSection(nameof(CsvOptions)));
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
