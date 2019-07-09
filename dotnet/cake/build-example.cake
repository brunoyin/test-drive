// ref: https://rehansaeed.com/cross-platform-devops-net-core/

// Other bamboo related: https://www.inversionofcontrol.co.uk/an-approach-to-building-net-core-apps-using-bamboo-and-cake/
// => https://gist.github.com/Antaris/8ad52a96e0f2d9f682d1cd6342c44936

// Target - The task you want to start. Runs the Default task if not specified.
var target = Argument("Target", "Default");
// Configuration - The build configuration (Debug/Release) to use.
// 1. If command line parameter parameter passed, use that.
// 2. Otherwise if an Environment variable exists, use that.
var configuration = 
    HasArgument("Configuration") ? Argument("Configuration") :
    EnvironmentVariable("Configuration") != null ? EnvironmentVariable("Configuration") : "Release";
// The build number to use in the version number of the built NuGet packages.
// There are multiple ways this value can be passed, this is a common pattern.
// 1. If command line parameter parameter passed, use that.
// 2. Otherwise if running on AppVeyor, get it's build number.
// 3. Otherwise if running on Travis CI, get it's build number.
// 4. Otherwise if an Environment variable exists, use that.
// 5. Otherwise default the build number to 0.
var buildNumber =
    HasArgument("BuildNumber") ? Argument<int>("BuildNumber") :
    AppVeyor.IsRunningOnAppVeyor ? AppVeyor.Environment.Build.Number :
    TravisCI.IsRunningOnTravisCI ? TravisCI.Environment.Build.BuildNumber :
    EnvironmentVariable("BuildNumber") != null ? int.Parse(EnvironmentVariable("BuildNumber")) : 0;

// A directory path to an Artifacts directory.
var artifactsDirectory = Directory("./Artifacts");

// Deletes the contents of the Artifacts folder if it should contain anything from a previous build.
Task("Clean")
    .Does(() =>
    {
        CleanDirectory(artifactsDirectory);
    });

// Run dotnet restore to restore all package references.
Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        DotNetCoreRestore();
    });

// Find all csproj projects and build them using the build configuration specified as an argument.
 Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
    {
        var projects = GetFiles("./**/*.csproj");
        foreach(var project in projects)
        {
            DotNetCoreBuild(
                project.GetDirectory().FullPath,
                new DotNetCoreBuildSettings()
                {
                    Configuration = configuration
                });
        }
    });

// Look under a 'Tests' folder and run dotnet test against all of those projects.
// Then drop the XML test results file in the Artifacts folder at the root.
Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var projects = GetFiles("./Tests/**/*.csproj");
        foreach(var project in projects)
        {
            DotNetCoreTest(
                project.GetDirectory().FullPath,
                new DotNetCoreTestSettings()
                {
                    ArgumentCustomization = args => args
                        .Append("-xml")
                        .Append(artifactsDirectory.Path.CombineWithFilePath(project.GetFilenameWithoutExtension()).FullPath + ".xml"),
                    Configuration = configuration,
                    NoBuild = true
                });
        }
    });

// Run dotnet pack to produce NuGet packages from our projects. Versions the package
// using the build number argument on the script which is used as the revision number 
// (Last number in 1.0.0.0). The packages are dropped in the Artifacts directory.
Task("Pack")
    .IsDependentOn("Test")
    .Does(() =>
    {
        var revision = buildNumber.ToString("D4");
        foreach (var project in GetFiles("./Source/**/*.csproj"))
        {
            DotNetCorePack(
                project.GetDirectory().FullPath,
                new DotNetCorePackSettings()
                {
                    Configuration = configuration,
                    OutputDirectory = artifactsDirectory,
                    VersionSuffix = revision
                });
        }
    });

// The default task to run if none is explicitly specified. In this case, we want
// to run everything starting from Clean, all the way up to Pack.
Task("Default")
    .IsDependentOn("Pack");

// Executes the task specified in the target argument.
RunTarget(target);

// version: '{build}'

// pull_requests:
//   # Do not increment build number for pull requests
//   do_not_increment_build_number: true

// nuget:
//   # Do not publish NuGet packages for pull requests
//   disable_publish_on_pr: true

// environment:
//   # Set the DOTNET_SKIP_FIRST_TIME_EXPERIENCE environment variable to stop wasting time caching packages
//   DOTNET_SKIP_FIRST_TIME_EXPERIENCE: true
//   # Disable sending usage data to Microsoft
//   DOTNET_CLI_TELEMETRY_OPTOUT: true

// build_script:
// - ps: .\build.ps1

// test: off

// artifacts:
// # Store NuGet packages
// - path: .\Artifacts\**\*.nupkg
//   name: NuGet
// # Store xUnit Test Results
// - path: .\Artifacts\**\*.xml
//   name: xUnit Test Results

// deploy:

// # Publish NuGet packages
// - provider: NuGet
//   name: production
//   api_key:
//     secure: 73eFUWSfho6pxCy1VRP1H0AYh/SFiyEREV+/ATcoj0I+sSH9dec/WXs6H2Jy5vlS
//   on:
//     # Only publish from the master branch
//     branch: master
//     # Only publish if the trigger was a Git tag
//     # git tag v0.1.0-beta
//     # git push origin --tags
//     appveyor_repo_tag: true

