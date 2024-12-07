using Nuke.Common;
using ricaun.Nuke.Components;
using Nuke.Common.Tools.AzureSignTool;
using System;
using Newtonsoft.Json;
using Nuke.Common.IO;
using System.IO;
using System.Reflection;
using Nuke.Common.Tools.NuGetKeyVaultSignTool;
using Nuke.NuGetKeyVaultSignTool;
using System.Linq;
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Tools.DotNet;

public interface INuGetKeyVaultSign : IClean, ICompile
{
    private static string AZURE_KEY_VAULT_FILE => Environment.GetEnvironmentVariable("AZURE_KEY_VAULT_FILE");
    private static string AZURE_KEY_VAULT_PASSWORD => Environment.GetEnvironmentVariable("AZURE_KEY_VAULT_PASSWORD");

    private static AbsolutePath GetToolInstallationPath()
    {
        AbsolutePath folder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        return folder / "Tools";
    }

    private static string PackageDownload(string packageId)
    {
        var toolFolder = GetToolInstallationPath();

        if (Globbing.GlobFiles(toolFolder, $"{packageId}.exe").FirstOrDefault() is AbsolutePath packageToolExeExists)
        {
            Serilog.Log.Information("AzureSignTool");
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

    Target NuGetKeyVaultSign => _ => _
        .TriggeredBy(Clean)
        .Before(Compile)
        .Executes(() =>
        {
            Serilog.Log.Information("NuGetKeyVaultSign");

            //new AzureSignToolTests().SetupEnvironmentToolPath();
            //new NuGetKeyVaultSignToolTests().SetupEnvironmentToolPath();

            ricaun.Nuke.Tools.AzureSignToolUtils.DownloadAzureSignTool();
            ricaun.Nuke.Tools.AzureSignToolUtils.DownloadNuGetKeyVaultSignTool();

            Serilog.Log.Information(AzureSignToolTasks.AzureSignToolPath);
            Serilog.Log.Information(NuGetKeyVaultSignToolTasks.NuGetKeyVaultSignToolPath);

            if (string.IsNullOrEmpty(AZURE_KEY_VAULT_FILE))
            {
                Serilog.Log.Warning("AZURE_KEY_VAULT_FILE is null");
                return;
            }

            if (string.IsNullOrEmpty(AZURE_KEY_VAULT_PASSWORD))
            {
                Serilog.Log.Warning("AZURE_KEY_VAULT_PASSWORD is null");
                return;
            }

            var azureKeyVaultFile = AzureKeyVaultConfig.Create(AZURE_KEY_VAULT_FILE);
            var azureKeyVaultClientSecret = AZURE_KEY_VAULT_PASSWORD;

            if (azureKeyVaultFile is null)
            {
                Serilog.Log.Warning("AzureKeyVaultConfig is null");
                return;
            }

            if (azureKeyVaultFile.IsValid() == false)
            {
                Serilog.Log.Warning($"{azureKeyVaultFile} is not valid");
                return;
            }

            Serilog.Log.Information($"Sign package using AzureKeyVaultCertificate {azureKeyVaultFile.AzureKeyVaultCertificate}");

            AbsolutePath rootAssembly = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var fileNameToSign = Globbing.GlobFiles(rootAssembly, "*/package.nupkg").FirstOrDefault();
            if (string.IsNullOrEmpty(fileNameToSign))
            {
                Serilog.Log.Warning("package.nupkg is null");
                return;
            }

            var fullPath = fileNameToSign.Copy(rootAssembly / "package-copy.nupkg", ExistsPolicy.FileOverwrite);

            var length = (double)new System.IO.FileInfo(fullPath).Length;

            NuGetKeyVaultSignToolTasks.NuGetKeyVaultSignTool(x => x
                .SetFile(fullPath)
                .SetKeyVaultCertificateName(azureKeyVaultFile.AzureKeyVaultCertificate)
                .SetKeyVaultUrl(azureKeyVaultFile.AzureKeyVaultUrl)
                .SetKeyVaultClientId(azureKeyVaultFile.AzureKeyVaultClientId)
                .SetKeyVaultTenantId(azureKeyVaultFile.AzureKeyVaultTenantId)
                .SetKeyVaultClientSecret(azureKeyVaultClientSecret)
                .SetTimestampRfc3161Url(azureKeyVaultFile.TimestampUrl ?? "http://timestamp.digicert.com")
                .SetTimestampDigest(azureKeyVaultFile.TimestampDigest ?? NuGetKeyVaultSignToolDigestAlgorithm.sha256)
                .SetForce(true)
            );

            var lengthAfter = (double)new System.IO.FileInfo(fullPath).Length;

            Serilog.Log.Warning($"Sign package {fullPath.Name} - {lengthAfter} {length}");

            fullPath = fileNameToSign.Copy(rootAssembly / "package-ricaun.nupkg", ExistsPolicy.FileOverwrite);
            length = (double)new System.IO.FileInfo(fullPath).Length;
            ricaun.Nuke.Tools.AzureSignToolUtils.Sign(fullPath, ricaun.Nuke.Tools.AzureKeyVaultConfig.Create(AZURE_KEY_VAULT_FILE), AZURE_KEY_VAULT_PASSWORD);
            lengthAfter = (double)new System.IO.FileInfo(fullPath).Length;
            Serilog.Log.Warning($"Sign package {fullPath.Name} - {lengthAfter} {length}");
        });
}
