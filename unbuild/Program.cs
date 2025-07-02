
using System.CommandLine;
using unbuild.Utils;

var projectFileOption = new Option<string>("--path")
{
    Description = "The path to your project's .csproj file"
};

RootCommand command = new RootCommand("unbuild - Ready to (un)build your project?");

command.Add(projectFileOption);

ParseResult parser = command.Parse(args);

var projectPath = parser.GetValue<string>(projectFileOption);

if (!string.IsNullOrEmpty(projectPath))
{
    try
    {
        Console.WriteLine($"(un)building {projectPath} ...");
        ProjectFileHandler.InjectFakeTargets(projectPath);
        Console.WriteLine($"Build {projectPath} ...");
        CommandLineHandler.RunDotnetBuild(projectPath);
        Console.WriteLine($"Finishing (un)building {projectPath} ...");
        ProjectFileHandler.RemoveFakeSource(projectPath);
        ProjectFileHandler.CheckAndDeleteFileIfSuccessful(projectPath);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR: {ex.Message}");
        ProjectFileHandler.RemoveFakeSource(projectPath);
    }
    
}
else
{
    Console.WriteLine("No path provided! Scared?");
}