/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.BattleNet;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Serilog;
using Serilog.Core;
using System;
using System.IO;
using wowCalc;

namespace Mvc.Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        private IHostEnvironment HostingEnvironment { get; }

        //private static readonly Logger logger =
        //    new LoggerConfiguration()
        //    //.Enrich.WithExceptionDetails()
        //    .WriteTo.File("Exception.txt"/*, fileSizeLimitBytes: 1, rollOnFileSizeLimit: true*/)
        //    /*(new JsonFormatter(renderMessage: true), @"logs\log-{Date}.txt")*/.CreateLogger();

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            //services.AddMemoryCache();
            //services.AddSession();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                //new FileLogger("logger.txt");
            .AddCookie(options =>
            {
                options.LoginPath = "/signin";
                options.LogoutPath = "/signout";
                options.ExpireTimeSpan = TimeSpan.FromSeconds(1d);
                options.SlidingExpiration = false;
            })
            .AddBattleNet(options =>
            {
                options.ClientId = Configuration["BattleNet:ClientId"];
                options.ClientSecret = Configuration["BattleNet:ClientSecret"];
                options.Region = BattleNetAuthenticationRegion.Europe;
                options.Scope.Add("wow.profile");
            });
            services.AddSignalR();
            services.AddSingleton<ParseService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        public void Configure(IApplicationBuilder app/*, ParseService parseService*/)
        {
            app.UseSerilogRequestLogging();

            if (HostingEnvironment.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
                app.UseDeveloperExceptionPage();
            }

            // Required to serve files with no extension in the .well-known folder
            var options = new StaticFileOptions()
            {
                ServeUnknownFileTypes = true,
            };

            app.UseStaticFiles(options);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<LogHub>("/chat");
                endpoints.MapDefaultControllerRoute();
            });

            //parseService.Start();
        }
    }
}
