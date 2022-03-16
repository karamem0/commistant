//
// Copyright (c) 2022 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant
{

    public static class Program
    {

        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults((builder) => builder
                    .ConfigureLogging((context, builder) => builder
                        .AddDebug()
                        .AddConsole()
                        .AddAzureWebAppDiagnostics())
                    .UseStartup<Startup>())
                .Build()
                .Run();
        }

    }

}
