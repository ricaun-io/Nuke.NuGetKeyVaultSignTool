//using Nuke.Common;
//using ricaun.Nuke.Components;
//using Nuke.Common.Tools.AzureSignTool;
//using System;
//using Newtonsoft.Json;
//using Nuke.Common.IO;
//using System.IO;
//using System.Reflection;

//public interface IShowNuGetKeyVaultSign : IClean, ICompile
//{
//    private static string AZURE_KEY_VAULT_FILE => Environment.GetEnvironmentVariable("AZURE_KEY_VAULT_FILE");
//    private static string AZURE_KEY_VAULT_PASSWORD => Environment.GetEnvironmentVariable("AZURE_KEY_VAULT_PASSWORD");

//    Target ShowNuGetKeyVaultSign => _ => _
//        .TriggeredBy(Clean)
//        .Before(Compile)
//        .Executes(() =>
//        {
//            Serilog.Log.Information(NuGetKeyVaultSignToolTasks.NuGetKeyVaultSignToolPath);

//            if (string.IsNullOrEmpty(AZURE_KEY_VAULT_FILE))
//            {
//                Serilog.Log.Warning("AZURE_KEY_VAULT_FILE is null");
//                return;
//            }

//            if (string.IsNullOrEmpty(AZURE_KEY_VAULT_PASSWORD))
//            {
//                Serilog.Log.Warning("AZURE_KEY_VAULT_PASSWORD is null");
//                return;
//            }

//            var azureKeyVaultFile = JsonConvert.DeserializeObject<AzureKeyVaultConfig>(AZURE_KEY_VAULT_FILE);
//            var azureKeyVaultClientSecret = AZURE_KEY_VAULT_PASSWORD;

//            if (azureKeyVaultFile is null)
//            {
//                Serilog.Log.Warning("AzureKeyVaultConfig is null");
//                return;
//            }

//            if (azureKeyVaultFile.IsValid() == false)
//            {
//                Serilog.Log.Warning($"{azureKeyVaultFile} is not valid");
//                return;
//            }

//            Serilog.Log.Information($"Sign package using AzureKeyVaultCertificate {azureKeyVaultFile.AzureKeyVaultCertificate}");

//            AbsolutePath rootAssembly = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
//            var fileNameToSign = "file.nupkg";
//            var fullPath = Path.Combine(rootAssembly, fileNameToSign);

//            var length = (double) new System.IO.FileInfo(fullPath).Length;

//            NuGetKeyVaultSignToolTasks.NuGetKeyVaultSignTool(x => x
//                .SetFile(fullPath)
//                .SetKeyVaultCertificateName(azureKeyVaultFile.AzureKeyVaultCertificate)
//                .SetKeyVaultUrl(azureKeyVaultFile.AzureKeyVaultUrl)
//                .SetKeyVaultClientId(azureKeyVaultFile.AzureKeyVaultClientId)
//                .SetKeyVaultTenantId(azureKeyVaultFile.AzureKeyVaultTenantId)
//                .SetKeyVaultClientSecret(azureKeyVaultClientSecret)
//                .SetTimestampRfc3161Url(azureKeyVaultFile.TimestampUrl ?? "http://timestamp.digicert.com")
//                .SetTimestampDigest(azureKeyVaultFile.TimestampDigest ?? NuGetKeyVaultSignToolDigestAlgorithm.sha256)
//                .SetForce(true)
//            );

//            var lengthAfter = (double) new System.IO.FileInfo(fullPath).Length;

//            Serilog.Log.Warning($"Sign package {fileNameToSign} - {lengthAfter} {length}");

//        });
//}
