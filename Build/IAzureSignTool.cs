using Nuke.Common;
using Nuke.Common.Tools.AzureSignTool;
using Nuke.Common.Tools.AzureKeyVault;
using ricaun.Nuke.Components;
using ricaun.Nuke.Tools.NuGetKeyVaultSignTool;
using Nuke.Common.Tools.ILRepack;
using ricaun.Nuke.Tools;

public interface IAzureSignTool : IClean, ICompile
{
    Target AzureSignTool => _ => _
        .TriggeredBy(Clean)
        .Before(Compile)
        //.Requires<NuGetKeyVaultSignToolTasks>()
        //.Requires<AzureSignToolTasks>()
        .Requires<ILRepackTasks>()
        .Executes(() =>
        {
            Serilog.Log.Information(AzureSignToolTasks.AzureSignToolPath);
            Serilog.Log.Information(NuGetKeyVaultSignToolTasks.NuGetKeyVaultSignToolPath);
        });
}
