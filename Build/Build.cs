using Nuke.Common;
using Nuke.Common.Execution;
using ricaun.Nuke;
using ricaun.Nuke.Components;

class Build : NukeBuild, IPublish, ITest //,IShowNuGetKeyVaultSign
{
    string ITest.TestProjectName => "Nuke.NuGetKeyVaultSignTool";
    public static int Main() => Execute<Build>(x => x.From<IPublish>().Build);
}
