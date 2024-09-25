using Microsoft.Extensions.FileProviders;

namespace SoloX.CodeQuality.WebHost
{
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
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
