//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Aspire.Hosting;
using Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

var builder = DistributedApplication.CreateBuilder(args);

var storage = builder
    .AddAzureStorage("storage")
    .RunAsEmulator();
var blobs = storage.AddBlobs("blobs");

_ = builder.AddProject("server", "../Karamem0.Commistant.Web/Karamem0.Commistant.Web.csproj")
    .WithReference(blobs)
    .WaitFor(blobs);
_ = builder
    .AddViteApp(
        "client",
        "../../client",
        useHttps: true
    )
    .WithNpmPackageInstallation()
    .WithExternalHttpEndpoints();

var app = builder.Build();

await app.RunAsync();
