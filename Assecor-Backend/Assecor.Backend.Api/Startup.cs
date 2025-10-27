using Assecor.Backend.Configuration;
using Assecor.Backend.Dal.IoC;

namespace Assecor.Backend.Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureService(IServiceCollection services)
        {
            services.AddDalServices();
            services.AddControllers();

            var csvOptions = new CsvOptions();
            var csvOptionsSection = Configuration.GetSection("Connectionstrings");
            csvOptionsSection.Bind(csvOptions);
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Setup development ASP.NET pipeline.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           
            // Adds middleware for dynamically compressing HTTP Responses.
            app.UseResponseCompression();

            app.UseStaticFiles();
            app.UseRouting();
           
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();

            // Endpoint mapping
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
