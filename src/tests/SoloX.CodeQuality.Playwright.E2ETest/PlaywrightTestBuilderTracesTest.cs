// ----------------------------------------------------------------------
// <copyright file="PlaywrightTestBuilderTracesTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;

namespace SoloX.CodeQuality.Playwright.E2ETest
{
    public class PlaywrightTestBuilderTracesTest
    {
        [Theory]
        [InlineData(Browser.Chromium)]
        [InlineData(Browser.Firefox)]
#if !DEBUG
        [InlineData(Browser.Webkit)]
#endif
        public async Task ItShouldOpenPageAndGenerateTraces(Browser browser)
        {
            var path = Path.GetFullPath(Path.GetDirectoryName(this.GetType().Assembly.Location)!);

            var tracesFile = $"Traces_{Guid.NewGuid()}.zip";

            var builder = PlaywrightTestBuilder.Create()
                .WithLocalHost(localHostBuilder =>
                {
                    localHostBuilder.UseWebHostWithWwwRoot(path, "home.html");
                })
                .WithTraces(b =>
                {
                    b.UseFilePattern(f => tracesFile);
                });

            var playwrightTest = await builder
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
                    })
                .ConfigureAwait(true);

            File.Exists(tracesFile).Should().BeTrue();

            File.Delete(tracesFile);
        }
    }
}