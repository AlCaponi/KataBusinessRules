using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApplication.Interfaces;
using WebApplication.Projections.Article;
using WebApplication.Repositories;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace WebApplication
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvcCore().AddJsonFormatters();

            services.AddTransient<IProjection, NewArticleProjection>();

            if (Configuration["persistence:type"] == "mongodb")
            {
                services.Configure<MongoDbSettings>(Configuration.GetSection("persistence"));
            }
            
            services.AddScoped<IMongoClient>( srv => 
            {
                var mongoSettings = srv.GetService<IOptions<MongoDbSettings>>().Value;
                var mongoDbSettings = new MongoClientSettings()
                {
                    Server = new MongoServerAddress(mongoSettings.Host, mongoSettings.Port),
                    ConnectTimeout = new TimeSpan(0, 0, 2)
                };
                
                return new MongoClient(mongoDbSettings);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715
            app.UseMvc();
        }
    }
}
