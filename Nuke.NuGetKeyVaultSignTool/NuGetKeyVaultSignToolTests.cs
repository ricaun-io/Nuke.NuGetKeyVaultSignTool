using Nuke.Common.IO;
using NUnit.Framework;
using System.IO;
using System.Reflection;
using Nuke.Common.Tools.NuGetKeyVaultSignTool;
using Nuke.Common.Tools.AzureSignTool;
using Nuke.CodeGeneration;
using Newtonsoft.Json;
using System;
using Nuke.Common.Tooling;
using System.Collections.Generic;

namespace Nuke.NuGetKeyVaultSignTool
{
    public class NuGetKeyVaultSignToolTests
    {
        private static string AZURE_KEY_VAULT_FILE => Environment.GetEnvironmentVariable("AZURE_KEY_VAULT_FILE");
        private static string AZURE_KEY_VAULT_PASSWORD => Environment.GetEnvironmentVariable("AZURE_KEY_VAULT_PASSWORD");

        Dictionary<string, string> EnvironmentToolPath = new Dictionary<string, string>
        {
            { "NUGETKEYVAULTSIGNTOOL_EXE", ".nuget\\packages\\nugetkeyvaultsigntool\\3.2.3\\tools\\net6.0\\any\\NuGetKeyVaultSignTool.dll" }
        };

        [OneTimeSetUp]
        public void SetupEnvironmentToolPath()
        {
            var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            foreach (var item in EnvironmentToolPath)
            {
                Environment.SetEnvironmentVariable(item.Key, Path.Combine(userFolder, item.Value));
            }
        }

        public static void DefaultConsole(OutputType type, string output)
        {
            if (type == OutputType.Std)
                Console.WriteLine(output);
            else
                Console.WriteLine(output);
        }

        [Test]
        public void SetUpNuGetKeyVaultSignToolTasks()
        {
            // Change the logger to print the output to the console
            NuGetKeyVaultSignToolTasks.NuGetKeyVaultSignToolLogger = DefaultConsole;

            // Show the path of the NuGetKeyVaultSignTool, without NUGETKEYVAULTSIGNTOOL_EXE environment variable a exception will be thrown
            Console.WriteLine(NuGetKeyVaultSignToolTasks.NuGetKeyVaultSignToolPath);
        }

        [TestCase("Files/package.nupkg")]
        public void SignPackage(string fileName)
        {
            if (string.IsNullOrEmpty(AZURE_KEY_VAULT_FILE))
                Assert.Ignore("AZURE_KEY_VAULT_FILE is null.");

            if (string.IsNullOrEmpty(AZURE_KEY_VAULT_PASSWORD))
                Assert.Ignore("AZURE_KEY_VAULT_PASSWORD is null.");

            var azureKeyVaultFile = JsonConvert.DeserializeObject<AzureKeyVaultConfig>(AZURE_KEY_VAULT_FILE);
            var azureKeyVaultClientSecret = AZURE_KEY_VAULT_PASSWORD;

            if (azureKeyVaultFile.IsValid() == false)
                Assert.Ignore($"{azureKeyVaultFile} is not valid.");

            var fileLength = new FileInfo(fileName).Length;

            NuGetKeyVaultSignToolTasks.NuGetKeyVaultSignTool(x => x
                .SetFile(fileName)
                .SetKeyVaultCertificateName(azureKeyVaultFile.AzureKeyVaultCertificate)
                .SetKeyVaultUrl(azureKeyVaultFile.AzureKeyVaultUrl)
                .SetKeyVaultClientId(azureKeyVaultFile.AzureKeyVaultClientId)
                .SetKeyVaultTenantId(azureKeyVaultFile.AzureKeyVaultTenantId)
                .SetKeyVaultClientSecret(azureKeyVaultClientSecret)
                .SetTimestampRfc3161Url(azureKeyVaultFile.TimestampUrl ?? "http://timestamp.digicert.com")
                .SetTimestampDigest(azureKeyVaultFile.TimestampDigest ?? NuGetKeyVaultSignToolDigestAlgorithm.sha256)
                .SetForce(true)
            );

            var fileLengthSigned = new FileInfo(fileName).Length;

            Console.WriteLine($"{fileName} - {fileLengthSigned} {fileLength}");

            Assert.Greater(fileLengthSigned, fileLength);
        }
    }
}