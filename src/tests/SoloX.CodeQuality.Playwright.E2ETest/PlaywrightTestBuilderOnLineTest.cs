// ----------------------------------------------------------------------
// <copyright file="PlaywrightTestBuilderOnLineTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;

namespace SoloX.CodeQuality.Playwright.E2ETest
{
    public class PlaywrightTestBuilderOnLineTest
    {
        private readonly IPlaywrightTestBuilder builder;

        public PlaywrightTestBuilderOnLineTest()
        {
            this.builder = PlaywrightTestBuilder.Create()
                .WithOnLineHost("https://www.google.com/");
        }

        [Theory]
        [InlineData(Browser.Chromium)]
        [InlineData(Browser.Firefox)]
        [InlineData(Browser.Webkit)]
        public async Task ItShouldOpenPageFromOnLineWebHost(Browser browser)
        {
            var playwrightTest = await this.builder
                .BuildAsync(browser)
                .ConfigureAwait(true);

            await using var _ = playwrightTest.ConfigureAwait(false);

            await playwrightTest
                .GotoPageAsync(
                    string.Empty,
                    async (page) =>
                    {
                        var body = page.Locator("body");

                        await body.WaitForAsync().ConfigureAwait(true);

                        var title = await page.TitleAsync().ConfigureAwait(true);

                        title.Should().Be("Google");
                    })
                .ConfigureAwait(true);
        }
    }
}