using Assecor.Backend.Configuration;
using Assecor.Backend.CsvAccess;
using Assecor.Backend.Dal.IoC;
using Assecor.Backend.Mappings;
using Assecor.Backend.Services.IoC;
using Microsoft.OpenApi.Models;

namespace Assecor.Backend.Api
{
    public class Startup(IConfiguration configuration)
    {
        private IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDalServices();
            services.AddServiceLayerServices();
            services.AddHttpContextAccessor();
            services.AddHttpClient();
            services.AddCsvReader();
            services.AddMappings();

            // swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Assessment Backend API",
                    Version = "v1",
                    Description = string.Empty,
                    Contact = new OpenApiContact
                    {
                        Name = "Sebastian Kaidel",
                        Email = "sebastian.kaidel@gmail.com"
                    }
                });
            });

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

            // swagger
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Assessment Backend API v1");
                options.RoutePrefix = string.Empty;
            });

            // Endpoint mapping
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
