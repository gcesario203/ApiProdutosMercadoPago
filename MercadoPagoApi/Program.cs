using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MercadoPagoApi.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MercadoPagoApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MercadoPago.SDK.AccessToken = Settings.AccessToken;
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
