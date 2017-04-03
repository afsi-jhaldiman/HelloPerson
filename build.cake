var solutionFile = "./HelloPerson.sln";
var projectFile = "./HelloPerson/HelloPerson.csproj";
var outputDirectory = "./.build";
var packageDirectory = outputDirectory + "/package";
var workDirectory = outputDirectory + "/work";

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

Task("Default").IsDependentOn("Build");

Task("Clean")
  .Does(() =>
  {
    CleanDirectories(new[] { packageDirectory, workDirectory });
  });

Task("PackageRestore")
  .Does(() =>
  {
    NuGetRestore(solutionFile);
  });

Task("Build")
  .IsDependentOn("PackageRestore")
  .IsDependentOn("Clean")
  .Does(() =>
  {
    MSBuild(projectFile, new MSBuildSettings()
      .SetConfiguration(configuration)
      .SetVerbosity(Verbosity.Minimal)
      .WithProperty("AllowedReferenceRelatedFileExtensions", "none")
	  .WithTarget("Package")
	  .WithProperty("PackageLocation", "../" + packageDirectory + "/HelloPerson.zip")
	  .WithProperty("PackageTempRootDir", "../" + workDirectory));
  });

RunTarget(target);