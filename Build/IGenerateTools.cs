using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Utilities.Collections;
using ricaun.Nuke.Components;
using static Nuke.CodeGeneration.CodeGenerator;

using Nuke.Common.Tools.GitHub;

public interface IGenerateTools : IClean, ICompile, IHazMainProject, IHazGitRepository
{
    AbsolutePath SpecificationsDirectory => MainProject.Directory / "Tools";
    Target GenerateTools => _ => _
        .TriggeredBy(Clean)
        .Before(Compile)
        .Executes(() =>
        {
            SpecificationsDirectory.GlobFiles("*.json").ForEach(x =>
            {
                Serilog.Log.Information($"GenerateCode: {x.Name}");
                GenerateCode(
                    x,
                    namespaceProvider: x => $"Nuke.Common.Tools.{x.Name}",
                    sourceFileProvider: x => GitRepository.GetGitHubBrowseUrl(x.SpecificationFile)
                );
            });
        });
}
