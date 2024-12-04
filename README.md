## Nuke.NuGetKeyVaultSignTool

[![Visual Studio 2022](https://img.shields.io/badge/Visual%20Studio-2022-blue)](../..)
[![Nuke](https://img.shields.io/badge/Nuke-Build-blue)](https://nuke.build/)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Build](../../actions/workflows/Build.yml/badge.svg)](../../actions)

Nuke Tool Generator for [NuGetKeyVaultSignTool](https://github.com/novotnyllc/NuGetKeyVaultSignTool).

## Tool CodeGenerator

To create a new tool, you need to use the `GenerateCode` method in the `Nuke.Tooling.Generator` package to convert the `.json` file configuration into a `.Generated.cs` file.

The [NuGetKeyVaultSignTool.json](Nuke.NuGetKeyVaultSignTool/Tools) file was based in the [AzureSignTool.json](https://github.com/nuke-build/nuke/tree/develop/source/Nuke.Common/Tools/AzureSignTool) and is used to autogenerate the `NuGetKeyVaultSignTool.Generated.cs`.

The [GenerationToolsTests.cs](Nuke.NuGetKeyVaultSignTool/GenerationToolsTests.cs) have a unit test to generate all the `.json` inside the `Tools` folder and generate the `.Generated.cs` files.

## NuGetKeyVaultSignToolTasks

The `NuGetKeyVaultSignTool.Generated.cs` file contain the `NuGetKeyVaultSignToolTasks` class with the following tasks:

```
NuGetKeyVaultSignToolTasks.NuGetKeyVaultSignTool(x => x
    .SetFile(fileName)
    .SetKeyVaultCertificateName(azureKeyVaultCertificate)
    .SetKeyVaultUrl(azureKeyVaultUrl)
    .SetKeyVaultClientId(azureKeyVaultClientId)
    .SetKeyVaultTenantId(azureKeyVaultTenantId)
    .SetKeyVaultClientSecret(azureKeyVaultClientSecret)
    .SetTimestampRfc3161Url("http://timestamp.digicert.com")
    .SetTimestampDigest(NuGetKeyVaultSignToolDigestAlgorithm.sha256)
    .SetForce(true)
);
```

### Nuke

To use the `NuGetKeyVaultSignToolTasks` inside a nuke project you need to install the `NuGetKeyVaultSignTool` package in your project.

```xml
<ItemGroup>
    <PackageDownload Include="NuGetKeyVaultSignTool" Version="[3.2.3]" />
</ItemGroup>
```

### Unit Test

To unit test the `NuGetKeyVaultSignToolTasks` still need to install the `PackageDownload` in the project, but is necessary to setup a environment variable to be able to find the `NuGetKeyVaultSignToolTasks.NuGetKeyVaultSignToolPath`.

```
string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
string nuGetKeyVaultSignToolPath = Path.Combine(userFolder, ".nuget\\packages\\nugetkeyvaultsigntool\\3.2.3\\tools\\net6.0\\any\\NuGetKeyVaultSignTool.dll");
Environment.SetEnvironmentVariable("NUGETKEYVAULTSIGNTOOL_EXE", nuGetKeyVaultSignToolPath);
```

## AzureKeyVaultConfig

To simplify the configuration to sign with `Azure Key Vault` two environment variables are used `AZURE_KEY_VAULT_FILE` and `AZURE_KEY_VAULT_PASSWORD`.

### AZURE_KEY_VAULT_FILE

The `AZURE_KEY_VAULT_FILE` is a `json` with the base configuration of the certificated in the `Azure Key Vault`:

```json
{
    "AzureKeyVaultCertificate": "AzureKeyVaultCertificate",
    "AzureKeyVaultUrl": "AzureKeyVaultUrl",
    "AzureKeyVaultClientId": "AzureKeyVaultClientId",
    "AzureKeyVaultTenantId": "AzureKeyVaultTenantId",
    "TimestampUrl" : "http://timestamp.digicert.com"
    "TimestampDigest" : "sha256"
}
```

The `TimestampUrl` and `TimestampDigest` are optional.

### AZURE_KEY_VAULT_PASSWORD

The `AZURE_KEY_VAULT_PASSWORD` is the `AzureKeyVaultClientSecret` of the certificate.

## License

This package is [licensed](LICENSE) under the [MIT License](https://en.wikipedia.org/wiki/MIT_License).

---

Do you like this package? Please [star this project on GitHub](../../stargazers)!