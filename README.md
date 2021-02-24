# CodeQuality

## CodeQuality Prod
[![NuGet Beta](https://img.shields.io/nuget/vpre/SoloX.CodeQuality.Prod.svg)](https://www.nuget.org/packages/SoloX.CodeQuality.Prod)

## CodeQuality Test
[![NuGet Beta](https://img.shields.io/nuget/vpre/SoloX.CodeQuality.Test.svg)](https://www.nuget.org/packages/SoloX.CodeQuality.Test)

Allows to setup coding style and analysis rules for a high code quality in your C# projects!

It basically provides two Nuget packages that will setup and enable code quality checks on your C# projects:
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
Install-Package SoloX.CodeQuality.Prod -version 2.0.0-alpha.1
or
Install-Package SoloX.CodeQuality.Test -version 2.0.0-alpha.1

Install-Package SoloX.CodeQuality.Test.Helpers -version 2.0.0-alpha.1
```

**Install using the .Net CLI:**
```bash
dotnet add package SoloX.CodeQuality.Prod --version 2.0.0-alpha.1
or
dotnet add package SoloX.CodeQuality.Test --version 2.0.0-alpha.1

dotnet add package SoloX.CodeQuality.Test.Helpers --version 2.0.0-alpha.1
```

**Install editing your project file (csproj):**
```xml
<PackageReference Include="SoloX.CodeQuality.Prod" Version="2.0.0-alpha.1" />
or
<PackageReference Include="SoloX.CodeQuality.Test" Version="2.0.0-alpha.1" />

<PackageReference Include="SoloX.CodeQuality.Test.Helpers" Version="2.0.0-alpha.1" />
```
