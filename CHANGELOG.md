# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [1.0.0] / 2024-12-04
### Build
- Add `INuGetKeyVaultSign` to test `NuGetKeyVaultSignTool` inside nuke project using `AzureKeyVaultConfig`.
- Add `IGenerateTools` to generate the `Generated.cs` files.
- Add `AZURE_KEY_VAULT_FILE` and `AZURE_KEY_VAULT_PASSWORD` environment variables in `ci`.
### Tests
- Test `AzureKeyVaultConfig` with `AZURE_KEY_VAULT_FILE` environment variable.
- Test `NuGetKeyVaultSignTool` in a package.
- Test `AzureSignTool` in a assembly file, show subject with `GetSignedFileSubject`.

[vNext]: ../../compare/1.0.0...HEAD
[1.0.0]: ../../compare/1.0.0