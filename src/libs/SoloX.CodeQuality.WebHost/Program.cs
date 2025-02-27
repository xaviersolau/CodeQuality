﻿// ----------------------------------------------------------------------
// <copyright file="Program.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.Extensions.FileProviders;

namespace SoloX.CodeQuality.WebHost
{
    internal sealed class Program
    {
        public static void Main(string[] args)
        {
            var binaryFolder = Path.GetDirectoryName(typeof(Program).Assembly.Location);

            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                Args = args,
                ContentRootPath = binaryFolder,
            });
            var app = builder.Build();

            var root = builder.Configuration["RootPath"] ?? Environment.CurrentDirectory;
            var index = builder.Configuration["Index"] ?? "index.html";

            var fileProvider = new PhysicalFileProvider(root);

            app.UseStaticFiles(
                new StaticFileOptions()
                {
                    FileProvider = fileProvider,
                    ServeUnknownFileTypes = true,
                });

            app.Map(string.Empty, ctx =>
            {
                var fileInfo = fileProvider.GetFileInfo(index);

                ctx.Response.ContentType = "text/html";

                return ctx.Response.SendFileAsync(fileInfo);
            });

            app.Run();
        }
    }
}
