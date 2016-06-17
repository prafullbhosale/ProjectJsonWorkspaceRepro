using Microsoft.CodeAnalysis;
using Microsoft.DotNet.ProjectModel;
using Microsoft.DotNet.ProjectModel.Workspaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var projectFile = ProjectReader.GetProject(@"..\ConsoleApp2\project.json");
            ProjectContext context = new ProjectContextBuilder()
                .WithProject(projectFile)
                .WithTargetFramework(NuGet.Frameworks.FrameworkConstants.CommonFrameworks.NetCoreApp10)
                .Build();

            var workspace = context.CreateRoslynWorkspace();
            var compilation = workspace.CurrentSolution.Projects.First().GetCompilationAsync().Result;

            using (var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms, pdbStream: null);
                if (!result.Success)
                {
                    var formatter = new DiagnosticFormatter();
                    var errorMessages = result.Diagnostics
                                            .Where(IsError)
                                            .Select(d => formatter.Format(d));
                    Console.WriteLine("ErrorMessage:: ");
                    foreach(var e in errorMessages)
                    {
                        Console.WriteLine($"    {e}");
                    }
                }
                else
                {
                    Console.WriteLine("Compilation succeeded");
                }
            }
        }

        private static bool IsError(Diagnostic diagnostic)
        {
            return diagnostic.Severity == DiagnosticSeverity.Error;
        }
    }
}
