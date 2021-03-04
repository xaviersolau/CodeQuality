# CodeQuality
[![Build - CI](https://github.com/xaviersolau/CodeQuality/actions/workflows/build-ci.yml/badge.svg)](https://github.com/xaviersolau/CodeQuality/actions/workflows/build-ci.yml)

## CodeQuality Prod
[![NuGet Beta](https://img.shields.io/nuget/vpre/SoloX.CodeQuality.Prod.svg)](https://www.nuget.org/packages/SoloX.CodeQuality.Prod)

## CodeQuality Test
[![NuGet Beta](https://img.shields.io/nuget/vpre/SoloX.CodeQuality.Test.svg)](https://www.nuget.org/packages/SoloX.CodeQuality.Test)

Allows to setup coding style and analysis rules for a high code quality in your C# projects!

It basically provides two Nuget packages that will setup and enable code quality checks (analysis and style)
on your C# projects:
* one Prod Nuget package to enable a very high code quality;
* and one Test Nuget package to enable a very high code quality with some rules customized for the tests;

This project is not implementing analysis by itself, it is using existing analysis packages like:
* Microsoft.CodeAnalysis.*.NetAnalyzers
* Microsoft.CodeAnalysis.*.CodeStyle

## License and credits

CodeQuality project is written by Xavier Solau. It's licensed under the MIT license.

 * * *

## Installation

You can checkout this Github repository or you can use the NuGet package:

**Install using the command line from the Package Manager:**
```bash
Install-Package SoloX.CodeQuality.Prod -version 2.0.0
or
Install-Package SoloX.CodeQuality.Test -version 2.0.0

Install-Package SoloX.CodeQuality.Test.Helpers -version 2.0.0
```

**Install using the .Net CLI:**
```bash
dotnet add package SoloX.CodeQuality.Prod --version 2.0.0
or
dotnet add package SoloX.CodeQuality.Test --version 2.0.0

dotnet add package SoloX.CodeQuality.Test.Helpers --version 2.0.0
```

**Install editing your project file (csproj):**
```xml
<PackageReference Include="SoloX.CodeQuality.Prod" Version="2.0.0" />
or
<PackageReference Include="SoloX.CodeQuality.Test" Version="2.0.0" />

<PackageReference Include="SoloX.CodeQuality.Test.Helpers" Version="2.0.0" />
```

## Options

By default, using the Nuget in your project will generate a `.editorconfig` file in your project root folder.
Note that it will override the file if is already exists in your project folder. You can disable the override
by setting the property `CodeQualityOverrideEditorConfig` to `false` in the `csproj` file:

```xml
  <PropertyGroup>
    <CodeQualityOverrideEditorConfig>false</CodeQualityOverrideEditorConfig>
  </PropertyGroup>
```

The default coding style rules of the fields are defined as **camelCase** and the field access must be write with the
`this.` prefix. But on this point this is also common to use the `_` as prefix with **camelCase** naming and
to forbid the use of `this.` to access the fields.
In order to enable this underscore style you can set the property in the `csproj` file:

```xml
  <PropertyGroup>
    <CodeQualityFieldsUseUnderscorStyle>true</CodeQualityFieldsUseUnderscorStyle>
  </PropertyGroup>
```

