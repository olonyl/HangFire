using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.MemoryStorage;
using HangFire.Domain.Repositories;
using HangFire.Infrastructure;
using HangFire.Infrastructure.Repositories;
using HangFire.Service.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HangFire.Implement.MS
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuratio)
        {
            _configuration = configuratio;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = _configuration["DatabaseConnection:SqlLocal"];
                 services.AddHangfire(config =>
                 config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseDefaultTypeSerializer()
                .UseDefaultTypeSerializer()
                .UseMemoryStorage()
             )
             .AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));

            services.AddTransient<IScrapingJobService, ScrapingJobService>()
                  .AddTransient<IScanningUrlRepository, ScanningUrlRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IWebHostEnvironment env,
             IServiceProvider serviceProvider
             )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });


            var maxHangfireWorkers = (Environment.ProcessorCount * 12);
            ConfigurationHangfireQueuesWorkers(app, new[] { "default" }, maxHangfireWorkers);

            app.UseHangfireDashboard();

            serviceProvider.GetService<IScrapingJobService>().ScanUrlsWithQueue();

        }
        public static void ConfigurationHangfireQueuesWorkers(IApplicationBuilder app, string[] queues, int workerCount)
        {
            var hangfireQueueOptions = new BackgroundJobServerOptions
            {
                Queues = queues,
                WorkerCount = workerCount,
                ServerName = Guid.NewGuid().ToString(),
            };

            app.UseHangfireServer(hangfireQueueOptions);

        }
    }
}
