using Nuke.Common.IO;
using System.IO;
using Nuke.Common.Tools.DotNet;
using System.Linq;

namespace Nuke.NuGetKeyVaultSignTool
{
    public class PackageDownloadUtils
    {
        private static AbsolutePath GetToolInstallationPath()
        {
            AbsolutePath folder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return folder / "Tools";
        }

        public static string PackageDownload(string packageId)
        {
            var toolFolder = GetToolInstallationPath();
            
            if (Globbing.GlobFiles(toolFolder, $"{packageId}.exe").FirstOrDefault() is AbsolutePath packageToolExeExists)
            {
                return packageToolExeExists;
            }
           
            DotNetTasks.DotNetToolInstall(x => x
                .SetPackageName(packageId)
                .SetToolInstallationPath(toolFolder)
            );

            if (Globbing.GlobFiles(toolFolder, $"{packageId}.exe").FirstOrDefault() is AbsolutePath packageToolExe)
            {
                return packageToolExe;
            }
            return null;
        }
    }
}