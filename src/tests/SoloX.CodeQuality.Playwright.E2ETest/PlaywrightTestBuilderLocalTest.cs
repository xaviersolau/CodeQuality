// ----------------------------------------------------------------------
// <copyright file="PlaywrightTestBuilderLocalTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;

namespace SoloX.CodeQuality.Playwright.E2ETest
{
    public class PlaywrightTestBuilderLocalTest
    {
        private readonly IPlaywrightTestBuilder builder;

        public PlaywrightTestBuilderLocalTest()
        {
            var path = Path.GetFullPath(Path.GetDirectoryName(this.GetType().Assembly.Location)!);

            this.builder = PlaywrightTestBuilder.Create()
                .WithLocalHost(localHostBuilder =>
                {
                    localHostBuilder
                        .UsePortRange(new PortRange(5000, 6000))
                        .UseWebHostWithWwwRoot(path, "home.html")
                        .UseWebHostBuilder(builder =>
                        {
                            //builder.ConfigureServices(services =>
                            //{
                            //    services.AddTransient<IService, ServiceMock>();
                            //})
                            //.ConfigureAppConfiguration((app, conf) =>
                            //{
                            //    conf.AddJsonFile("appsettings.Test.json");
                            //});
                        });
                });
        }

        [Theory]
        [InlineData(Browser.Chromium)]
        [InlineData(Browser.Firefox)]
#if !DEBUG
        [InlineData(Browser.Webkit)]
#endif
        public async Task ItShouldOpenTheHomePageFromStaticHomeFile(Browser browser)
        {
            var playwrightTest = await this.builder
                .BuildAsync(browser)
                .ConfigureAwait(true);

            await using var _ = playwrightTest.ConfigureAwait(false);

            await playwrightTest
                .GotoPageAsync(
                    "home.html",
                    async (page) =>
                    {
                        var body = page.Locator("body");

                        await body.WaitForAsync().ConfigureAwait(true);

                        var title = await page.TitleAsync().ConfigureAwait(true);

                        title.Should().Be("Home Title");
                    }).ConfigureAwait(true);
        }
    }
}