using Serilog;
using System;

using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ParksVictoriaApi.Services;


using System.Reflection;


namespace ParksVictoriaApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddOptions();
            services.Configure<IConfiguration>(Configuration);
            services.AddSingleton(Configuration);
            services.AddSingleton<IFileService, FileSystemService>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            // Enable middleware to serve swagger - ui(HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Parks victoria API", Version = "v1" });

                // Get/Create xml comments path
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlFile);

                // Set xml path
                options.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //Add serilog support for logging
            loggerFactory.AddSerilog();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            object p = app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Parks Victoria API V1");
            });

            app.Map("/HealthCheck", a =>
            {
                a.Run(async context =>
                {
                    await context.Response.WriteAsync($"{env.ApplicationName} is alive in {env.EnvironmentName}");
                });
            });
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
