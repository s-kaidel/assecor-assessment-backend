
namespace Assecor.Backend.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args);

            // Configure the WebHost.
            hostBuilder.ConfigureWebHostDefaults(webBuilder =>
            {
                // Specify startup class.
                webBuilder.UseStartup<Startup>();
            });

            return hostBuilder;
        }
    }
}
