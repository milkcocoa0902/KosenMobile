using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Timers;


namespace REST
{
    public class Program
    {
        public static void Main(string[] args)
        {
						Timer timer = new Timer(10 * 60 * 1000);
						timer.Elapsed += (sender, e)=>{
								REST.RSS.RSSController.RSSDataBase.Build();
						};
						timer.Start();

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
