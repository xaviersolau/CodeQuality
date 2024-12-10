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
                            //})
                            //.UseSetting("SomeKey", "SomeValue");
                        });
                })
                .WithPlaywrightOptions(opt =>
                {
                    //opt.Headless = false;
                    //opt.SlowMo = 5000;
                    //opt.Timeout = 60000;
                })
                .WithPlaywrightNewContextOptions(opt =>
                {
                    //opt.ViewportSize = new Microsoft.Playwright.ViewportSize() { Height = 800, Width = 1000 };
                    //opt.StorageStatePath = "State Json file";
                });
        }

        [Theory]
        [InlineData(Browser.Chromium, null)]
        [InlineData(Browser.Chromium, Devices.Pixel)]
        [InlineData(Browser.Chromium, Devices.PixelLandscape)]
        [InlineData(Browser.Firefox, null)]
        [InlineData(Browser.Webkit, null)]
        [InlineData(Browser.Webkit, Devices.iPhone)]
        [InlineData(Browser.Webkit, Devices.iPhoneLandscape)]
        public async Task ItShouldOpenTheHomePageFromStaticHomeFile(Browser browser, string? deviceName)
        {
            var playwrightTest = await this.builder
                .BuildAsync(browser, deviceName: deviceName)
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