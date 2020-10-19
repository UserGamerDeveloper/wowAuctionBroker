/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Mvc.Client.Data;
using Serilog;
using System.Linq;
using System.Net;
using static Mvc.Client.Models.RealmModel;

namespace Mvc.Client
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            ServicePointManager.DefaultConnectionLimit = 100;
            Log.Logger = new LoggerConfiguration()
                    .WriteTo.File("Exception.log"/*, fileSizeLimitBytes: 1, rollOnFileSizeLimit: true*/)
                    .WriteTo.Console()
                    /*(new JsonFormatter(renderMessage: true), @"logs\log-{Date}.txt")*/.CreateLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                //webBuilder.ConfigureKestrel(serverOptions =>
                // {
                //     serverOptions.Listen(IPAddress.Loopback, 5001,
                //         listenOptions =>
                //         {
                //             listenOptions.UseHttps();
                //         });
                // });
            }).UseSerilog();
    }
}
