using NUnit.Framework;
using System.IO;
using System;
using Nuke.Local.Tools.DotNet;
using Nuke.Common.Tooling;
using Nuke.Local.Tools.NuGet;
using System.Linq;

namespace Nuke.NuGetKeyVaultSignTool
{
    public class DotNetNuGetTests
    {
        public static void DefaultConsole(OutputType type, string output)
        {
            if (type == OutputType.Std)
                Console.WriteLine(output);
            else
                Console.WriteLine(output);
        }

        [TestCase("Files/packageNone.nupkg")]
        public void SignPackage(string fileName)
        {
            // Copy File to temp folder
            var tempFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempFolder);
            var tempFile = Path.Combine(tempFolder, Path.GetFileName(fileName));
            File.Copy(fileName, tempFile);

            DotNetTasks.DotNetLogger = DefaultConsole;

            DotNetTasks.DotNetNuGetSign(x => x
                .SetTargetPath(tempFile)
                .SetCertificatePath("Files/signfile.pfx")
                .SetCertificatePassword("signfile")
                .SetTimestamper("http://timestamp.digicert.com")
            );

            try
            {
                DotNetTasks.DotNetNuGetVerify(x => x
                    .SetTargetPath(tempFile)
                    .SetCertificateFingerprint("E413994364668939A34B235D378FC8C2CA12C56C0BF9ECC56538FC6079576855")
            );
            }
            catch { }
        }
    }

    public class NuGetTests
    {
        [OneTimeSetUp]
        public void SetupEnvironmentToolPath()
        {
            var packageId = NuGetTasks.NuGetPackageId;

            var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var packagesFolder = Path.Combine(userFolder, ".nuget", "packages");

            var packageIdFolder = Path.Combine(packagesFolder, packageId);
            var packageExe = Directory.GetFiles(packageIdFolder, "*.exe", SearchOption.AllDirectories).FirstOrDefault();
            Environment.SetEnvironmentVariable(Path.GetFileNameWithoutExtension(packageExe).ToUpper() + "_EXE", packageExe);
        }

        public static void DefaultConsole(OutputType type, string output)
        {
            if (type == OutputType.Std)
                Console.WriteLine(output);
            else
                Console.WriteLine(output);
        }

        [TestCase("Files/packageNone.nupkg")]
        public void SignPackage(string fileName)
        {
            // Copy File to temp folder
            var tempFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempFolder);
            var tempFile = Path.Combine(tempFolder, Path.GetFileName(fileName));
            File.Copy(fileName, tempFile);

            NuGetTasks.NuGetLogger = DefaultConsole;

            NuGetTasks.NuGetSign(x => x
                .SetTargetPath(tempFile)
                .SetCertificatePath("Files/signfile.pfx")
                .SetCertificatePassword("signfile")
                .SetTimestamper("http://timestamp.digicert.com")
            );

            try
            {
                NuGetTasks.NuGetVerify(x => x
                    .SetTargetPath(tempFile)
                    .SetCertificateFingerprint("E413994364668939A34B235D378FC8C2CA12C56C0BF9ECC56538FC6079576855")
                );
            }
            catch { }
        }
    }
}