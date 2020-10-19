using Hangfire;
using Hangfire.MemoryStorage;
using HangFire.Domain.Repositories;
using HangFire.Infrastructure;
using HangFire.Infrastructure.Repositories;
using HangFire.ProcessUrl.Helper;
using HangFire.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace HangFire.ProcessUrl
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                 .AddUserSecrets<Program>();

            Configuration = builder.Build();

            var connectionString = Configuration["DatabaseConnection:SqlLocal"];

            var serviceProvider = new ServiceCollection()
                  .AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Transient)
                  .AddHangfire(config =>
                     config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                            .UseDefaultTypeSerializer()
                            .UseDefaultTypeSerializer()
                            .UseMemoryStorage()
                            .UseSqlServerStorage(connectionString)
             )
                  
                  .AddTransient<IScrapingJobService, ScrapingJobService>()
                  .AddTransient<IScanningUrlRepository, ScanningUrlRepository>()
                   .Configure<DatabaseConnection>(Configuration.GetSection(nameof(DatabaseConnection)))
             .BuildServiceProvider();


            //serviceProvider.GetService<IScrapingJobService>().StartScraping(Configuration["UrlToScraping"]);
            serviceProvider.GetService<IScrapingJobService>().ScanUrls();
            
        }
      
    }
}
