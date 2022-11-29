# CodeQuality

This project provides an easy way to set up .Net Core [static code analysis](#coding-style-and-analysis) with pre-configured quality and coding style
rules for your Test and your Prod C# projects.

It also provides [test helpers](#test-helpers) to use in your unit test projects helping with test logging, HttpClient mocking...

## Project dashboard
[![Build - CI](https://github.com/xaviersolau/CodeQuality/actions/workflows/build-ci.yml/badge.svg)](https://github.com/xaviersolau/CodeQuality/actions/workflows/build-ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

| Package                                 | Nuget.org |
|-----------------------------------------|-----------|
|**SoloX.CodeQuality.Prod**               |[![NuGet Beta](https://img.shields.io/nuget/vpre/SoloX.CodeQuality.Prod.svg)](https://www.nuget.org/packages/SoloX.CodeQuality.Prod)
|**SoloX.CodeQuality.Test**               |[![NuGet Beta](https://img.shields.io/nuget/vpre/SoloX.CodeQuality.Test.svg)](https://www.nuget.org/packages/SoloX.CodeQuality.Test)
|**SoloX.CodeQuality.Test.Helpers**       |[![NuGet Beta](https://img.shields.io/nuget/vpre/SoloX.CodeQuality.Test.Helpers.svg)](https://www.nuget.org/packages/SoloX.CodeQuality.Test.Helpers)
|**SoloX.CodeQuality.Test.Helpers.XUnit** |[![NuGet Beta](https://img.shields.io/nuget/vpre/SoloX.CodeQuality.Test.Helpers.XUnit.svg)](https://www.nuget.org/packages/SoloX.CodeQuality.Test.Helpers.XUnit)
|**SoloX.CodeQuality.Test.Helpers.NUnit** |[![NuGet Beta](https://img.shields.io/nuget/vpre/SoloX.CodeQuality.Test.Helpers.NUnit.svg)](https://www.nuget.org/packages/SoloX.CodeQuality.Test.Helpers.NUnit)

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

* * *

### Installation

You can checkout this Github repository or you can use the NuGet package:

**Install using the command line from the Package Manager:**
```bash
Install-Package SoloX.CodeQuality.Prod -version 2.0.12
or
Install-Package SoloX.CodeQuality.Test -version 2.0.12
```

**Install using the .Net CLI:**
```bash
dotnet add package SoloX.CodeQuality.Prod --version 2.0.12
or
dotnet add package SoloX.CodeQuality.Test --version 2.0.12
```

**Install editing your project file (csproj):**
```xml
<PackageReference Include="SoloX.CodeQuality.Prod" Version="2.0.12">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; contentfiles; analyzers</IncludeAssets>
</PackageReference>
or
<PackageReference Include="SoloX.CodeQuality.Test" Version="2.0.12">
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

## Test Helpers

This aspect of the project helps to write your tests providing useful classes to handle test logging, HttpClient mocking....

* * *

### Installation

You can checkout this Github repository or you can use the NuGet package:

**Install using the command line from the Package Manager:**
```bash
Install-Package SoloX.CodeQuality.Test.Helpers -version 2.0.12

Install-Package SoloX.CodeQuality.Test.Helpers.XUnit -version 2.0.12

Install-Package SoloX.CodeQuality.Test.Helpers.NUnit -version 2.0.12
```

**Install using the .Net CLI:**
```bash
dotnet add package SoloX.CodeQuality.Test.Helpers --version 2.0.12

dotnet add package SoloX.CodeQuality.Test.Helpers.XUnit --version 2.0.12

dotnet add package SoloX.CodeQuality.Test.Helpers.NUnit --version 2.0.12
```

**Install editing your project file (csproj):**
```xml
<PackageReference Include="SoloX.CodeQuality.Test.Helpers" Version="2.0.12" />

<PackageReference Include="SoloX.CodeQuality.Test.Helpers.XUnit" Version="2.0.12" />

<PackageReference Include="SoloX.CodeQuality.Test.Helpers.NUnit" Version="2.0.12" />
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

