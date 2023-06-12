using Serilog;

namespace NetCoreEx.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
           
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Log.Logger = new LoggerConfiguration()
                             .ReadFrom.Configuration(config)
                             .CreateLogger();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(hostingContext.Configuration))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseIISIntegration();
                });
    }
}