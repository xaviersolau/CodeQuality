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
Install-Package SoloX.CodeQuality.Prod -version 2.0.4
or
Install-Package SoloX.CodeQuality.Test -version 2.0.4

Install-Package SoloX.CodeQuality.Test.Helpers -version 2.0.4
```

**Install using the .Net CLI:**
```bash
dotnet add package SoloX.CodeQuality.Prod --version 2.0.4
or
dotnet add package SoloX.CodeQuality.Test --version 2.0.4

dotnet add package SoloX.CodeQuality.Test.Helpers --version 2.0.4
```

**Install editing your project file (csproj):**
```xml
<PackageReference Include="SoloX.CodeQuality.Prod" Version="2.0.4" />
or
<PackageReference Include="SoloX.CodeQuality.Test" Version="2.0.4" />

<PackageReference Include="SoloX.CodeQuality.Test.Helpers" Version="2.0.4" />
```

 * * *

## Options

### .editorconfig file generation

By default, using the Nuget in your project will generate a `.editorconfig` file in your project root folder.
Note that it will override the file if is already exists in your project folder. You can disable the override
by setting the property `CodeQualityOverrideEditorConfig` to `false` in the `csproj` file:

```xml
  <PropertyGroup>
    <CodeQualityOverrideEditorConfig>false</CodeQualityOverrideEditorConfig>
  </PropertyGroup>
```

### Fields coding style rules

The default coding style rules of the fields are defined as **camelCase** and the field access must be write with the
`this.` prefix. But on this point this is also common to use the `_` as prefix with **camelCase** naming and
to forbid the use of `this.` to access the fields.
In order to enable this underscore style you can set the property in the `csproj` file:

```xml
  <PropertyGroup>
    <CodeQualityFieldsUseUnderscorStyle>true</CodeQualityFieldsUseUnderscorStyle>
  </PropertyGroup>
```

### File header options

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

### Add/Update .gitignore file

Since .editorconfig and XML project document files are generated automatically you can optionally
enable the project .gitignore file update with the generated files. You just have to set the
`CodeQualityUpdateGitIgnore` property to `true`.

```xml
  <PropertyGroup>
    <CodeQualityUpdateGitIgnore>true</CodeQualityUpdateGitIgnore>
  </PropertyGroup>
```
