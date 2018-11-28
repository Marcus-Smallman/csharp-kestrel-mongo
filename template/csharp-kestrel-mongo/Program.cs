// Copyright (c) Alex Ellis 2017. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// Influenced by https://github.com/burtonr/csharp-kestrel-template

using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Function;
using Microsoft.AspNetCore.Http;

namespace root
{
    public class Program
    {
        static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://localhost:5000/")
                .Build();
    }

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Run(async (context) =>
            {
                var functionHandler = new FunctionHandler();

                // Set up the MongoDB connection before calling the function handler.
                functionHandler.SetupConnection();
                
                try
                {
                    var requestBody = getRequest(context.Request.Body);
                    var result = functionHandler.Handle(requestBody).Result;

                    await context.Response.WriteAsync(result);
                }
                catch (Exception ex)
                {
                    await context.Response.WriteAsync(ex.Message);
                }
            });
        }

        private string getRequest(Stream inputBody)
        {
            StreamReader reader = new StreamReader(inputBody);
            string text = reader.ReadToEnd();
            return text;
        }
    }
}
