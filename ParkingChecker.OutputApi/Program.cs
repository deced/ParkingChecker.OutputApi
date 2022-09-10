using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ParkingChecker.OutputApi.Controllers;
using ParkingChecker.OutputApi.Services;

namespace ParkingChecker.OutputApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
