using System.Diagnostics;

namespace unbuild.Utils
{
    public static class CommandLineHandler
    {
        public static bool RunDotnetBuild(string projectPath)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"build \"{projectPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            if (!process.WaitForExit(1000 * 60))
            {
                Console.Error.WriteLine("Build timed out.");
                process.Kill();
                return false;
            }

            Console.WriteLine(output);
            if (!string.IsNullOrWhiteSpace(error))
                Console.Error.WriteLine(error);

            return process.ExitCode == 0;
        }
    }
}
