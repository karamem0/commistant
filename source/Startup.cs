//
// Copyright (c) 2022 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant
{

    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            _ = services
                .AddHttpClient()
                .AddControllers()
                .AddNewtonsoftJson();
            _ = services.AddApplicationInsightsTelemetry();
            _ = services.AddBots(this.Configuration);
            _ = services.AddCommands();
            _ = services.AddDialogs();
            _ = services.AddServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                _ = app.UseDeveloperExceptionPage();
            }
            _ = app.UseDefaultFiles();
            _ = app.UseStaticFiles();
            _ = app.UseWebSockets();
            _ = app.UseRouting();
            _ = app.UseAuthorization();
            _ = app.UseEndpoints((endpoints) => endpoints.MapControllers());
        }

    }

}
