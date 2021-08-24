// ----------------------------------------------------------------------
// <copyright file="HttpClientMockBuilderTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;
using SoloX.CodeQuality.Test.Helpers.Http;
using SoloX.CodeQuality.Test.Helpers.UTest.Data;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace SoloX.CodeQuality.Test.Helpers.UTest.Http
{
    public class HttpClientMockBuilderTest
    {
        [Theory]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Forbidden)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.Unauthorized)]
        public async Task ItShouldMockHttpClientAndRepondWithTheGivenStatusCodeAsync(HttpStatusCode httpStatusCode)
        {
            var baseAddress = "http://host/api/test";

            var builder = new HttpClientMockBuilder();

            var httpClient = builder
                .WithBaseAddress(new Uri(baseAddress))
                .WithRequest("/api/test/target").RespondingStatus(httpStatusCode)
                .Build();

            Assert.NotNull(httpClient);

            using var request = new HttpRequestMessage(HttpMethod.Get, "target");
            var response = await httpClient.SendAsync(request).ConfigureAwait(false);

            if (httpStatusCode == HttpStatusCode.OK)
            {
                response.EnsureSuccessStatusCode();
            }
            else
            {
                Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode());
            }

            Assert.Equal(httpStatusCode, response.StatusCode);
        }

        [Fact]
        public async Task IsShouldRespondWithJsonContentAsync()
        {
            var baseAddress = "http://host/api/test";

            var dataObject = new Person()
            {
                FirstName = "John",
                LastName = "Doe",
            };

            var builder = new HttpClientMockBuilder();

            var httpClient = builder
                .WithBaseAddress(new Uri(baseAddress))
                .WithRequest("/api/test/target").RespondingJsonContent(request => dataObject)
                .Build();

            Assert.NotNull(httpClient);

            using var request = new HttpRequestMessage(HttpMethod.Get, "target");
            var response = await httpClient.SendAsync(request).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync(dataObject.GetType()).ConfigureAwait(false);

            result.Should().NotBeNull();

            result.Should().BeEquivalentTo(dataObject);
        }

        [Fact]
        public async Task ItShouldMockHttpClientAndRepondWithNotFundAsync()
        {
            var baseAddress = "http://host/api/test";

            var builder = new HttpClientMockBuilder();

            var httpClient = builder
                .WithBaseAddress(new Uri(baseAddress))
                .Build();

            Assert.NotNull(httpClient);

            using var request = new HttpRequestMessage(HttpMethod.Get, "target");

            var response = await httpClient.SendAsync(request).ConfigureAwait(false);

            Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode());

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ItShouldMockHttpClientAndSendJsonDataAndRespondBackWithJsonDataAsync()
        {
            var baseAddress = "http://host/api/test";

            var dataObject = new Person()
            {
                FirstName = "John",
                LastName = "Doe",
            };

            var builder = new HttpClientMockBuilder();

            var httpClient = builder
                .WithBaseAddress(new Uri(baseAddress))
                .WithJsonContentRequest<Person>("/api/test/target", HttpMethod.Post).RespondingJsonContent(requestContent => $"{requestContent.FirstName} {requestContent.LastName}")
                .Build();

            Assert.NotNull(httpClient);

            var response = await httpClient.PostAsJsonAsync<Person>("target", dataObject).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadFromJsonAsync<string>().ConfigureAwait(false);

            Assert.Equal("John Doe", responseContent);
        }

        [Fact]
        public async Task ItShouldMockHttpClientAndSendJsonDataAsync()
        {
            var baseAddress = "http://host/api/test";

            var dataObject = new Person()
            {
                FirstName = "John",
                LastName = "Doe",
            };

            var builder = new HttpClientMockBuilder();

            var httpClient = builder
                .WithBaseAddress(new Uri(baseAddress))
                .WithJsonContentRequest<Person>("/api/test/target", HttpMethod.Post).RespondingStatus(requestContent => HttpStatusCode.OK)
                .Build();

            Assert.NotNull(httpClient);

            var response = await httpClient.PostAsJsonAsync<Person>("target", dataObject).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
        }
    }
}
