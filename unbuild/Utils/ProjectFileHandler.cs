using System.Xml.Linq;

namespace unbuild.Utils
{
    public static class ProjectFileHandler
    {
        public static void InjectFakeTargets(string projectPath)
        {
            if (Path.Exists(projectPath))
            {
                Console.WriteLine($"Adding a fake afterbuild target to {projectPath} ...");
                XDocument document = XDocument.Load(projectPath);
                if (document != null) 
                {
                    XNamespace ns = document.Root.Name.Namespace;

                    XElement fakeTarget = new XElement(ns + "Target",
                                                        new XAttribute("Name", Constants.Constants.FakeTargetName),
                                                        new XAttribute("AfterTargets", "Build"),
                                                        new XElement(ns + "Exec",
                                                            new XAttribute("Command",
                                                                "powershell -Command \"gci env:* | Sort-Object Name | Out-File -FilePath '$(ProjectDir)EnvVars.txt'\"")
                                                        ));
                    var existingTarget = document.Root.Element(ns + "Target");
                    if (existingTarget != null &&
                        existingTarget.Attribute("Name")?.Value == Constants.Constants.FakeTargetName)
                    {
                        Console.WriteLine("Target already exists in the project file.");
                        return;
                    }

                    document.Root.Add(fakeTarget);
                    document.Save(projectPath);

                }

            }
            else
            {
                Console.WriteLine("Path does not exist! Scared?");
            }
        }

        public static void RemoveFakeSource(string projectPath)
        {
            if (Path.Exists(projectPath))
            {
                Console.WriteLine($"Removing fake target from {projectPath} ...");
                XDocument document = XDocument.Load(projectPath);
                if (document != null)
                {
                    XNamespace ns = document.Root.Name.Namespace;

                   var fakeTarget = document.Descendants(ns + "Target").Where(x  => (string)x.Attribute("Name") ==  Constants.Constants.FakeTargetName).FirstOrDefault();
                    if (fakeTarget != null)
                    {
                        Console.WriteLine($"Removing {fakeTarget}");
                        fakeTarget.Remove();

                    }

                    document.Save(projectPath);

                }
            }
        }

        public static void CheckAndDeleteFileIfSuccessful(string projectPath)
        {
            var parentDirectory = Path.GetDirectoryName(projectPath);
            var filePath = Path.Combine(parentDirectory, Constants.Constants.FakeFileName);
            if (File.Exists(filePath)) { 

                Console.WriteLine("!!!POTENTIAL VULNERABILITY!!!");
                Console.WriteLine("Your project has been (un)built! You need to up your game!");

                Console.WriteLine("Clean up started!");
                File.Delete(filePath);
                Console.WriteLine("Clean up complete!");
            }
        }
    }
}
