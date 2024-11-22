# CodeQuality

This project provides an easy way to set up .Net Core [static code analysis](#coding-style-and-analysis) with pre-configured quality and coding style
rules for your Test and your Prod C# projects.

It also provides:
* [Test helpers](#test-helpers) to use in your unit test projects helping with test logging or running processes.
* A [Playwright Test Builder](#playwright-test-builder) to help you make Web application tests.
* And a [HttpClient mock](#httpclient-mocking) to be able to inject HttpClient when ever you need it in your unit tests;



## Project dashboard
[![Build - CI](https://github.com/xaviersolau/CodeQuality/actions/workflows/build-ci.yml/badge.svg)](https://github.com/xaviersolau/CodeQuality/actions/workflows/build-ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

| Package                                 | Nuget.org | Pre-release |
|-----------------------------------------|-----------|-------------|
|**SoloX.CodeQuality.Prod**               |[![NuGet Beta](https://img.shields.io/nuget/v/SoloX.CodeQuality.Prod.svg)](https://www.nuget.org/packages/SoloX.CodeQuality.Prod)                             |[![NuGet Beta](https://img.shields.io/nuget/vpre/SoloX.CodeQuality.Prod.svg)](https://www.nuget.org/packages/SoloX.CodeQuality.Prod)
|**SoloX.CodeQuality.Test**               |[![NuGet Beta](https://img.shields.io/nuget/v/SoloX.CodeQuality.Test.svg)](https://www.nuget.org/packages/SoloX.CodeQuality.Test)                             |[![NuGet Beta](https://img.shields.io/nuget/vpre/SoloX.CodeQuality.Test.svg)](https://www.nuget.org/packages/SoloX.CodeQuality.Test)
|**SoloX.CodeQuality.Playwright**         |[![NuGet Beta](https://img.shields.io/nuget/v/SoloX.CodeQuality.Playwright.svg)](https://www.nuget.org/packages/SoloX.CodeQuality.Playwright)                 |[![NuGet Beta](https://img.shields.io/nuget/vpre/SoloX.CodeQuality.Playwright.svg)](https://www.nuget.org/packages/SoloX.CodeQuality.Playwright)
|**SoloX.CodeQuality.Test.Helpers**       |[![NuGet Beta](https://img.shields.io/nuget/v/SoloX.CodeQuality.Test.Helpers.svg)](https://www.nuget.org/packages/SoloX.CodeQuality.Test.Helpers)             |[![NuGet Beta](https://img.shields.io/nuget/vpre/SoloX.CodeQuality.Test.Helpers.svg)](https://www.nuget.org/packages/SoloX.CodeQuality.Test.Helpers)
|**SoloX.CodeQuality.Test.Helpers.XUnit** |[![NuGet Beta](https://img.shields.io/nuget/v/SoloX.CodeQuality.Test.Helpers.XUnit.svg)](https://www.nuget.org/packages/SoloX.CodeQuality.Test.Helpers.XUnit) |[![NuGet Beta](https://img.shields.io/nuget/vpre/SoloX.CodeQuality.Test.Helpers.XUnit.svg)](https://www.nuget.org/packages/SoloX.CodeQuality.Test.Helpers.XUnit)
|**SoloX.CodeQuality.Test.Helpers.NUnit** |[![NuGet Beta](https://img.shields.io/nuget/v/SoloX.CodeQuality.Test.Helpers.NUnit.svg)](https://www.nuget.org/packages/SoloX.CodeQuality.Test.Helpers.NUnit) |[![NuGet Beta](https://img.shields.io/nuget/vpre/SoloX.CodeQuality.Test.Helpers.NUnit.svg)](https://www.nuget.org/packages/SoloX.CodeQuality.Test.Helpers.NUnit)
|**SoloX.CodeQuality.WebHost**            |[![NuGet Beta](https://img.shields.io/nuget/v/SoloX.CodeQuality.WebHost.svg)](https://www.nuget.org/packages/SoloX.CodeQuality.WebHost)                       |[![NuGet Beta](https://img.shields.io/nuget/vpre/SoloX.CodeQuality.WebHost.svg)](https://www.nuget.org/packages/SoloX.CodeQuality.WebHost)

## License and credits

CodeQuality project is written by Xavier Solau. It's licensed under the MIT license.

* * *

## Coding style and Analysis

It basically provides two Nuget packages:
* one Prod Nuget package to enable a very high code quality: `SoloX.CodeQuality.Prod`;
* and one Test Nuget package to enable a high code quality with some rules customized for the tests `SoloX.CodeQuality.Test`;

This project is not implementing analysis by itself, it is using existing analysis packages like:
* Microsoft.CodeAnalysis.*.NetAnalyzers
* Microsoft.CodeAnalysis.*.CodeStyle


**Note that before version 2.1.0 the AnalysisLevel property was set to "latest" resulting in breaking the build just on framework update.**
**So from version 2.1.0 the analysis level is fixed to a specific version (in our case 8.0)**

* * *

### Installation

You can checkout this Github repository or you can use the NuGet package:

**Install using the command line from the Package Manager:**
```bash
Install-Package SoloX.CodeQuality.Prod -version 2.3.0
or
Install-Package SoloX.CodeQuality.Test -version 2.3.0
```

**Install using the .Net CLI:**
```bash
dotnet add package SoloX.CodeQuality.Prod --version 2.3.0
or
dotnet add package SoloX.CodeQuality.Test --version 2.3.0
```

**Install editing your project file (csproj):**
```xml
<PackageReference Include="SoloX.CodeQuality.Prod" Version="2.3.0">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; contentfiles; analyzers</IncludeAssets>
</PackageReference>
or
<PackageReference Include="SoloX.CodeQuality.Test" Version="2.3.0">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; contentfiles; analyzers</IncludeAssets>
</PackageReference>
```

 * * *

### Options

#### .editorconfig file generation

By default, using the Nuget in your project will generate a `.editorconfig` file in your project root folder.
Note that it will override the file if is already exists in your project folder. You can disable the override
by setting the property `CodeQualityOverrideEditorConfig` to `false` in the `csproj` file:

```xml
  <PropertyGroup>
    <CodeQualityOverrideEditorConfig>false</CodeQualityOverrideEditorConfig>
  </PropertyGroup>
```

#### Fields coding style rules

The default coding style rules of the fields are defined as **camelCase** and the field access must be write with the
`this.` prefix. But on this point this is also common to use the `_` as prefix with **camelCase** naming and
to forbid the use of `this.` to access the fields.
In order to enable this underscore style you can set the property in the `csproj` file:

```xml
  <PropertyGroup>
    <CodeQualityFieldsUseUnderscorStyle>true</CodeQualityFieldsUseUnderscorStyle>
  </PropertyGroup>
```

#### File header options

File header are required in the C# files by default. The header is defined with the following pattern:

```csharp
// ----------------------------------------------------------------------
// <copyright file="{FileName}" company="{Authors}">
// {Copyright}.
// Licensed under the {LicenseName} license.
// See {LicenseFile} file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

```

| Variable    | Description                                                                   |
|-------------|-------------------------------------------------------------------------------|
| FileName    | is basically the C# file name.                                                |
| Authors     | is the Authors list and can be defined with the property `CodeQualityHeaderCompanyName`. |
| Copyright   | is the copyright message list and can be defined with the property `CodeQualityHeaderCopyright`. |
| LicenseName | is the name of the license you use. It can be defined with the property `CodeQualityHeaderLicense`. |
| LicenseFile | is the file path of the license you use. It can be defined with the property `CodeQualityHeaderLicenseFile`. |

Here is a full header configuration example:

```xml
  <PropertyGroup>
    <CodeQualityHeaderEnable>true</CodeQualityHeaderEnable>
    <CodeQualityHeaderCompanyName>MyComany</CodeQualityHeaderCompanyName>
    <CodeQualityHeaderCopyright>Copyright © 2021 MyComany</CodeQualityHeaderCopyright>
    <CodeQualityHeaderLicense>MIT</CodeQualityHeaderLicense>
    <CodeQualityHeaderLicenseFile>LICENSE</CodeQualityHeaderLicenseFile>
  </PropertyGroup>
```

Resulting in this header:

```csharp
// ----------------------------------------------------------------------
// <copyright file="MyFile.cs" company="MyComany">
// Copyright © 2021 MyComany.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

```

Note that you can also reuse the Nuget package properties if those are defined in your csproj file:

```xml
  <PropertyGroup>
    <CodeQualityHeaderEnable>true</CodeQualityHeaderEnable>
    <CodeQualityHeaderCompanyName>$(Authors)</CodeQualityHeaderCompanyName>
    <CodeQualityHeaderCopyright>$(Copyright)</CodeQualityHeaderCopyright>
    <CodeQualityHeaderLicense>$(PackageLicenseExpression)</CodeQualityHeaderLicense>
    <CodeQualityHeaderLicenseFile>LICENSE</CodeQualityHeaderLicenseFile>
  </PropertyGroup>
```

Note that you can disable this header file rule by setting the property
`CodeQualityHeaderEnable` to `false` in the `csproj` file:

```xml
  <PropertyGroup>
    <CodeQualityHeaderEnable>false</CodeQualityHeaderEnable>
  </PropertyGroup>
```

#### Add/Update .gitignore file

Since .editorconfig and XML project document files are generated automatically you can optionally
enable the project .gitignore file update with the generated files. You just have to set the
`CodeQualityUpdateGitIgnore` property to `true`.

```xml
  <PropertyGroup>
    <CodeQualityUpdateGitIgnore>true</CodeQualityUpdateGitIgnore>
  </PropertyGroup>
```

#### ILogger use analyzer (CA1848, CA2254)

By default some rules about the use of the ILogger are reported as errors in Prod configuration.
* CA1848: Use the LoggerMessage delegates instead of calling LoggerExtensions methods
* CA2254: The logging message template should not vary between calls

You may need to disable those rules. To do so, you just have to set the
`CodeQualityLoggerUseDisabled` property to `true`.

```xml
  <PropertyGroup>
    <CodeQualityLoggerUseDisabled>true</CodeQualityLoggerUseDisabled>
  </PropertyGroup>
```


* * *

## Playwright Test Builder

Whenever you have a web application to test, you can use the `PlaywrightTestBuilder`!
The builder helps set up your host application and [Playwright](https://playwright.dev/dotnet/) to process your tests.

 * * *

### Installation

You can checkout this Github repository or use the NuGet package:

**Install using the command line from the Package Manager:**
```bash
Install-Package SoloX.CodeQuality.Playwright -version 2.3.0
```

**Install using the .Net CLI:**
```bash
dotnet add package SoloX.CodeQuality.Playwright --version 2.3.0
```

**Install editing your project file (csproj):**
```xml
<PackageReference Include="SoloX.CodeQuality.Playwright" Version="2.3.0" />
```

* * *

### Testing with Playwright

Setting up your test with Playwright and your web application host is simple. The `PlaywrightTestBuilder` helps
you create a `PlaywrightTest` by setting up your local host and configuring [Playwright](https://playwright.dev/dotnet/) to target this host.

Additionally, the builder automatically installs all Playwright dependencies. This means all you need to do is
install the NuGet package, write your test, and run it.

You can find an example [here](src/tests/SoloX.CodeQuality.Playwright.E2ETest/PlaywrightTestBuilderLocalTest.cs) of
a test using the `PlaywrightTestBuilder`.

* * *

### Features of the PlaywrightTestBuilder

The builder allows you to:
* Mock the application services you may need to mock.
* Set up the host on a specific network port (which is helpful for running tests in parallel).
* Configure [Playwright](https://playwright.dev/dotnet/) to target the test host.
* Enable Playwright traces.

You can use Playwright Test Builder to test any web application hosted by a .NET Core host. This includes
[Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
Web applications and SPA applications (such as [Angular](https://angular.dev/), [React](https://react.dev/), etc.) hosted on .NET Core.

It is also possible to test a static web application. The Playwright Test Builder will configure an embedded web
host to serve your static files.

* * *

### How to build a Playwright test

#### Get the builder

```csharp
using SoloX.CodeQuality.Playwright;

var builder = PlaywrightTestBuilder.Create();
```

#### Set up the Host

.Net Core host:

This is the most common use case. You need to provide a Type defined in you host entry-point assembly
(usually Program or Startup or any other type in the assembly).

```csharp
builder.WithLocalHost(localHostBuilder =>
    {
        localHostBuilder
            // Program is a type from your host entry-point assembly.
            .UseApplication<Program>()
            // Optionally you can specify the network port range to use.
            .UsePortRange(new PortRange(5000, 6000))
            // Optionally you can set up some service mocks or some configurations.
            // As you would do with the WebApplicationFactory.
            .UseWebHostBuilder(builder =>
            {
                builder
                    .ConfigureServices(services =>
                    {
                        // Add service mock
                        services.AddTransient<IService, ServiceMock>();
                    })
                    .ConfigureAppConfiguration((app, conf) =>
                    {
                        // Add configuration file
                        conf.AddJsonFile("appsettings.Test.json");
                    });
                    // Or just add a configuration key value
                    .UseSetting("SomeKey", "SomeValue");
            });
    });
```

> Note that your host must be based on .Net 8.0 and it must use the `WebApplication.CreateBuilder(args)` method to build your Web host. If
> you still use the hold way with `Host.CreateDefaultBuilder(args)` and a `Startup.cs` file, you will get an exception like this
> `System.InvalidOperationException : Build can only be called once.` and you won't be able to start your test.

On-line host:

In the case where your host is on-line, use:

```csharp
builder.WithOnLineHost("https://www.google.com/");
```

Static HTML files:

For applications based on static HTML files, you just need to provide the `wwwroot` folder location:

```csharp
var path = "Path to your 'wwwroot' folder";

builder.WithLocalHost(localHostBuilder =>
    {
        localHostBuilder.UseWebHostWithWwwRoot(path, "index.html");
    });
```

#### Set up Traces

Once your host is configured, you can enable and configure traces options:

```csharp
builder
    // It tells that you want to generate traces.
    .WithTraces(tracesBuilder =>
    {
        tracesBuilder
            // Set up traces options as you would using directly Playwright.
            .UseTraceOptions(opt =>
            {
                opt.Snapshots = true;
                opt.Screenshots = true;
                opt.Sources = true;
            })
            // Tells where the traces must be stored.
            .UseOutputFile(tracesFile);
    });
```

> Note: you can check on the [Playwright](https://playwright.dev/dotnet/) web site to find out more about the
> [Trace Viewer](https://playwright.dev/dotnet/docs/trace-viewer-intro).

#### Playwright options

It is also possible to specify the Playwright options:

```csharp
builder
    .WithPlaywrightOptions(opt =>
    {
        // Display the browser screen.
        opt.Headless = false;
        // Slow down browser actions.
        opt.SlowMo = 1000;
    })
    .WithPlaywrightNewContextOptions(opt =>
    {
        // Set up the browser view port size.
        opt.ViewportSize = new ViewportSize() { Height = 800, Width = 1000 };
        // Initialize browser storage.
        opt.StorageStatePath = "path to the state Json file that will be loaded in the new context.";
    });
```



#### Create and use the PlaywrightTest

Finally you can build the `PlaywrightTest` and use it like this:

```csharp
// Select the browser you want to test against. (Chromium, Firefox or Webkit)
var browser = Browser.Chromium;

// Note that the IPlaywrightTest is a IAsyncDisposable so don't forget the 'await using'.
await using var playwrightTest = await builder.BuildAsync(browser);

// Run the test.
await playwrightTest.GotoPageAsync("index.html", async (page) =>
    {
        var body = page.Locator("body");

        await body.WaitForAsync();
    });
```

> Note: you can check on the [Playwright](https://playwright.dev/dotnet/) web site to find out more about the
> [Codegen Tool](https://playwright.dev/dotnet/docs/codegen-intro).

* * *

## Test Helpers

This aspect of the project helps to write your tests providing useful classes to handle test logging, HttpClient mocking....

* * *

### Installation

You can checkout this Github repository or you can use the NuGet package:

**Install using the command line from the Package Manager:**
```bash
Install-Package SoloX.CodeQuality.Test.Helpers -version 2.3.0

Install-Package SoloX.CodeQuality.Test.Helpers.XUnit -version 2.3.0

Install-Package SoloX.CodeQuality.Test.Helpers.NUnit -version 2.3.0
```

**Install using the .Net CLI:**
```bash
dotnet add package SoloX.CodeQuality.Test.Helpers --version 2.3.0

dotnet add package SoloX.CodeQuality.Test.Helpers.XUnit --version 2.3.0

dotnet add package SoloX.CodeQuality.Test.Helpers.NUnit --version 2.3.0
```

**Install editing your project file (csproj):**
```xml
<PackageReference Include="SoloX.CodeQuality.Test.Helpers" Version="2.3.0" />

<PackageReference Include="SoloX.CodeQuality.Test.Helpers.XUnit" Version="2.3.0" />

<PackageReference Include="SoloX.CodeQuality.Test.Helpers.NUnit" Version="2.3.0" />
```

 * * *

### HttpClient mocking

It is really easy to mock a HttpClient with the `HttpClientMockBuilder` provided in
`SoloX.CodeQuality.Test.Helpers` nuget.

Here is an example of how you can use it in a Fluent way:

```csharp
// First we need the using statement.
using SoloX.CodeQuality.Test.Helpers.Http;

// Then we get the builder.
var builder = new HttpClientMockBuilder();

// Some data object.
var data = new Person(/*...*/);

// And build the mock with a request configured and responding with a JSON data content and a status OK.
var httpClient = builder
    .WithBaseAddress(new Uri("http://host/api/test"))
    .WithRequest("/api/test/target").RespondingJsonContent(data, HttpStatusCode.OK)
    .Build();

// Then you can just use the client to get your data back.
var response = await httpClient.GetFromJsonAsync<Person>("target");

```

### ILogger\<T> mocking

We often need to provide a ILogger\<T> instance to the class we want to test. This is easy to provide a
mock of this interface but sometime it is actually useful to be able to write the logs into the test
output. In order to write the logs in the test console output you can use the TestLogger available for
XUnit and NUnit.

#### With XUnit

The XUnit TestLogger is provided in the package `SoloX.CodeQuality.Test.Helpers.XUnit` and it can be
used like this:

```csharp
using Microsoft.Extensions.Logging;
using SoloX.CodeQuality.Test.Helpers.XUnit.Logger;
using Xunit;
using Xunit.Abstractions;

public class LoggerTest
{
    private readonly ITestOutputHelper testOutputHelper;

    public LoggerTest(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void IsShouldLogThoughTestLogger()
    {
        var logger = new TestLogger<LoggerTest>(this.testOutputHelper);

        logger.LogError("This is an error log message!");

        Assert.True(true);
    }
}
```

Or it is also possible to use it through Dependency Injection in the case where you would like to build
some integration tests using a service provider:

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SoloX.CodeQuality.Test.Helpers.XUnit.Logger;
using Xunit;
using Xunit.Abstractions;

public class LoggerTest
{
    private readonly ITestOutputHelper testOutputHelper;

    public LoggerTest(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void IsShouldRegisterXUnitLoggerFactory()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddTestLogging(this.testOutputHelper);

        using var serviceProvider = serviceCollection.BuildServiceProvider();

        var logger = serviceProvider.GetRequiredService<ILogger<LoggerTest>>();

        logger.LogError("This is an error log message!");

        Assert.True(true);
    }
}
```


#### With NUnit

The NUnit TestLogger is provided in the package `SoloX.CodeQuality.Test.Helpers.NUnit` and it can be
used like this:

```csharp
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using SoloX.CodeQuality.Test.Helpers.NUnit.Logger;

public class LoggerTest
{
    [Test]
    public void IsShouldLogThoughTestLogger()
    {
        var logger = new TestLogger<LoggerTest>();

        logger.LogError("This is an error log message!");

        Assert.Pass();
    }
}
```

Or it is also possible to use it through Dependency Injection in the case where you would like to build
some integration tests using a service provider:

```csharp
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using SoloX.CodeQuality.Test.Helpers.NUnit.Logger;

public class LoggerTest
{
    [Test]
    public void IsShouldRegisterNUnitLoggerFactory()
    {
        var serviceCollection = new ServiceCollection();
 
        serviceCollection.AddTestLogging();
 
        using var serviceProvider = serviceCollection.BuildServiceProvider();
 
        var logger = serviceProvider.GetRequiredService<ILogger<LoggerTest>>();
 
        logger.LogError("This is an error log message!");
 
        Assert.Pass();
    }
}
```

