using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTask_1.Services;
using WebTask_1.Models;
using WebTask_1.Models.Impl;
using System.Net;
using System.Text.Json;

namespace WebTask_1
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISettings,Settings>();
            services.AddTransient<PrimesFinderService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
                    await context.Response.WriteAsync(
                        "Hello World!\n" +
                        "This web service was made by Michael Terekhov!\n" +
                        "You can easily search for prime numbers,\n" +
                        "pass certain parameters, or do a simple number check (Is it prime)");
                });
                endpoints.MapGet("/primes/{number:int}", async context =>
                {
                    var settings = context.RequestServices.GetRequiredService<ISettings>();
                    PrimesFinderService finder = context.RequestServices.GetRequiredService<PrimesFinderService>();
                    Int32.TryParse((string)context.Request.RouteValues["number"], out int num);
                    settings.PrimesFrom = num;
                    var checkIsPrime = await finder.CheckIsPrime();
                    
                    if (checkIsPrime == true)
                        context.Response.StatusCode = 200;
                    else
                    {
                        context.Response.StatusCode = 404;
                    }
                });
                endpoints.MapGet("/primes", async context => 
                {
                    var settings = context.RequestServices.GetRequiredService<ISettings>();
                    PrimesFinderService finder = context.RequestServices.GetRequiredService<PrimesFinderService>();
                    if ((string)context.Request.Query["from"] == null ||
                    (string)context.Request.Query["to"] == null)
                    {
                        context.Response.StatusCode = 400;
                    }
                    bool fromIsValid = int.TryParse((string)context.Request.Query["from"],out int from);
                    bool toIsValid = int.TryParse((string)context.Request.Query["to"], out int to);
                    if (!fromIsValid && !toIsValid)
                    {
                        context.Response.StatusCode = 400;
                    }
                    else 
                    {
                        settings.PrimesFrom = from;
                        settings.PrimesTo = to;
                        var result = await finder.FindPrimesInRange();
                        context.Response.StatusCode = 200;
                        var options = new JsonSerializerOptions
                        {
                            WriteIndented = true,
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        };
                        string jsonOutput;
                        jsonOutput = JsonSerializer.Serialize(result, options);
                        await context.Response.WriteAsync(jsonOutput);
                    }
                });
            });
        }
    }
}
