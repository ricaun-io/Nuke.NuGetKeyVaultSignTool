using Nuke.Common;
using Nuke.Common.IO;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using static Nuke.CodeGeneration.CodeGenerator;

namespace Nuke.NuGetKeyVaultSignTool
{
    public class GenerationToolsTests
    {
        const string namespaceProvider = "Nuke.Common.Tools.{0}";
        private static string ToolsDirectory => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", "..", "Tools");
        private static string[] GetToolsFiles()
        {
            var toolsDirectory = ToolsDirectory;
            return Directory.GetFiles(toolsDirectory, "*.json")
                .Select(e => Path.GetFileName(e))
                .ToArray();
        }

        [TestCaseSource(nameof(GetToolsFiles))]
        public void GenerateCodeTest(string jsonFileName)
        {
            var fullPath = Path.Combine(ToolsDirectory, jsonFileName);
            GenerateCode(fullPath, namespaceProvider: x => string.Format(namespaceProvider, x.Name));
        }
    }
}