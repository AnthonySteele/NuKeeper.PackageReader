echo off
REM generate a coverage report locally
REM output in .\coveragereport
REM if this does not work, first install the tools:

REM dotnet tool install --global coverlet.console
REM dotnet tool install --global dotnet-reportgenerator-globaltool

dotnet build

coverlet .\tests\NuKeeper.PackageReader.IntegrationTests\bin\Debug\netcoreapp3.1\NuKeeper.PackageReader.IntegrationTests.dll --target "dotnet" --targetargs "test --no-build" --format opencover

reportgenerator -reports:.\coverage.opencover.xml -targetdir:.\coveragereport