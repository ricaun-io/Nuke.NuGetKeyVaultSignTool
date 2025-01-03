
using JetBrains.Annotations;
using Newtonsoft.Json;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools;
using Nuke.Common.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

namespace Nuke.Local.Tools.DotNet;

/// <summary>
///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/dotnet/core/tools/">official website</a>.</p>
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
[PathToolRequirement(DotNetPathExecutable)]
public partial class DotNetTasks
    : IRequirePathTool
{
    public const string DotNetPathExecutable = "dotnet";
    /// <summary>
    ///   Path to the DotNet executable.
    /// </summary>
    public static string DotNetPath =>
        ToolPathResolver.TryGetEnvironmentExecutable("DOTNET_EXE") ??
        ToolPathResolver.GetPathExecutable("dotnet");
    public static Action<OutputType, string> DotNetLogger { get; set; } = ProcessTasks.DefaultLogger;
    public static Action<ToolSettings, IProcess> DotNetExitHandler { get; set; } = ProcessTasks.DefaultExitHandler;
    /// <summary>
    ///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/dotnet/core/tools/">official website</a>.</p>
    /// </summary>
    public static IReadOnlyCollection<Output> DotNet(ArgumentStringHandler arguments, string workingDirectory = null, IReadOnlyDictionary<string, string> environmentVariables = null, int? timeout = null, bool? logOutput = null, bool? logInvocation = null, Action<OutputType, string> logger = null, Action<IProcess> exitHandler = null)
    {
        using var process = ProcessTasks.StartProcess(DotNetPath, arguments, workingDirectory, environmentVariables, timeout, logOutput, logInvocation, logger ?? DotNetLogger);
        (exitHandler ?? (p => DotNetExitHandler.Invoke(null, p))).Invoke(process.AssertWaitForExit());
        return process.Output;
    }
    /// <summary>
    ///   <p>Pushes a package to the server and publishes it.</p>
    ///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/dotnet/core/tools/">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>&lt;targetPath&gt;</c> via <see cref="DotNetNuGetPushSettings.TargetPath"/></li>
    ///     <li><c>--api-key</c> via <see cref="DotNetNuGetPushSettings.ApiKey"/></li>
    ///     <li><c>--disable-buffering</c> via <see cref="DotNetNuGetPushSettings.DisableBuffering"/></li>
    ///     <li><c>--force-english-output</c> via <see cref="DotNetNuGetPushSettings.ForceEnglishOutput"/></li>
    ///     <li><c>--no-service-endpoint</c> via <see cref="DotNetNuGetPushSettings.NoServiceEndpoint"/></li>
    ///     <li><c>--no-symbols</c> via <see cref="DotNetNuGetPushSettings.NoSymbols"/></li>
    ///     <li><c>--skip-duplicate</c> via <see cref="DotNetNuGetPushSettings.SkipDuplicate"/></li>
    ///     <li><c>--source</c> via <see cref="DotNetNuGetPushSettings.Source"/></li>
    ///     <li><c>--symbol-api-key</c> via <see cref="DotNetNuGetPushSettings.SymbolApiKey"/></li>
    ///     <li><c>--symbol-source</c> via <see cref="DotNetNuGetPushSettings.SymbolSource"/></li>
    ///     <li><c>--timeout</c> via <see cref="DotNetNuGetPushSettings.Timeout"/></li>
    ///   </ul>
    /// </remarks>
    public static IReadOnlyCollection<Output> DotNetNuGetPush(DotNetNuGetPushSettings toolSettings = null)
    {
        toolSettings = toolSettings ?? new DotNetNuGetPushSettings();
        using var process = ProcessTasks.StartProcess(toolSettings);
        toolSettings.ProcessExitHandler.Invoke(toolSettings, process.AssertWaitForExit());
        return process.Output;
    }
    /// <summary>
    ///   <p>Pushes a package to the server and publishes it.</p>
    ///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/dotnet/core/tools/">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>&lt;targetPath&gt;</c> via <see cref="DotNetNuGetPushSettings.TargetPath"/></li>
    ///     <li><c>--api-key</c> via <see cref="DotNetNuGetPushSettings.ApiKey"/></li>
    ///     <li><c>--disable-buffering</c> via <see cref="DotNetNuGetPushSettings.DisableBuffering"/></li>
    ///     <li><c>--force-english-output</c> via <see cref="DotNetNuGetPushSettings.ForceEnglishOutput"/></li>
    ///     <li><c>--no-service-endpoint</c> via <see cref="DotNetNuGetPushSettings.NoServiceEndpoint"/></li>
    ///     <li><c>--no-symbols</c> via <see cref="DotNetNuGetPushSettings.NoSymbols"/></li>
    ///     <li><c>--skip-duplicate</c> via <see cref="DotNetNuGetPushSettings.SkipDuplicate"/></li>
    ///     <li><c>--source</c> via <see cref="DotNetNuGetPushSettings.Source"/></li>
    ///     <li><c>--symbol-api-key</c> via <see cref="DotNetNuGetPushSettings.SymbolApiKey"/></li>
    ///     <li><c>--symbol-source</c> via <see cref="DotNetNuGetPushSettings.SymbolSource"/></li>
    ///     <li><c>--timeout</c> via <see cref="DotNetNuGetPushSettings.Timeout"/></li>
    ///   </ul>
    /// </remarks>
    public static IReadOnlyCollection<Output> DotNetNuGetPush(Configure<DotNetNuGetPushSettings> configurator)
    {
        return DotNetNuGetPush(configurator(new DotNetNuGetPushSettings()));
    }
    /// <summary>
    ///   <p>Pushes a package to the server and publishes it.</p>
    ///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/dotnet/core/tools/">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>&lt;targetPath&gt;</c> via <see cref="DotNetNuGetPushSettings.TargetPath"/></li>
    ///     <li><c>--api-key</c> via <see cref="DotNetNuGetPushSettings.ApiKey"/></li>
    ///     <li><c>--disable-buffering</c> via <see cref="DotNetNuGetPushSettings.DisableBuffering"/></li>
    ///     <li><c>--force-english-output</c> via <see cref="DotNetNuGetPushSettings.ForceEnglishOutput"/></li>
    ///     <li><c>--no-service-endpoint</c> via <see cref="DotNetNuGetPushSettings.NoServiceEndpoint"/></li>
    ///     <li><c>--no-symbols</c> via <see cref="DotNetNuGetPushSettings.NoSymbols"/></li>
    ///     <li><c>--skip-duplicate</c> via <see cref="DotNetNuGetPushSettings.SkipDuplicate"/></li>
    ///     <li><c>--source</c> via <see cref="DotNetNuGetPushSettings.Source"/></li>
    ///     <li><c>--symbol-api-key</c> via <see cref="DotNetNuGetPushSettings.SymbolApiKey"/></li>
    ///     <li><c>--symbol-source</c> via <see cref="DotNetNuGetPushSettings.SymbolSource"/></li>
    ///     <li><c>--timeout</c> via <see cref="DotNetNuGetPushSettings.Timeout"/></li>
    ///   </ul>
    /// </remarks>
    public static IEnumerable<(DotNetNuGetPushSettings Settings, IReadOnlyCollection<Output> Output)> DotNetNuGetPush(CombinatorialConfigure<DotNetNuGetPushSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false)
    {
        return configurator.Invoke(DotNetNuGetPush, DotNetLogger, degreeOfParallelism, completeOnFailure);
    }
    /// <summary>
    ///   <p>The dotnet nuget delete command deletes or unlists a package from the server. For nuget.org, the action is to unlist the package.</p>
    ///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/dotnet/core/tools/">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>&lt;packageId&gt;</c> via <see cref="DotNetNuGetDeleteSettings.PackageId"/></li>
    ///     <li><c>&lt;packageVersion&gt;</c> via <see cref="DotNetNuGetDeleteSettings.PackageVersion"/></li>
    ///     <li><c>--api-key</c> via <see cref="DotNetNuGetDeleteSettings.ApiKey"/></li>
    ///     <li><c>--force-english-output</c> via <see cref="DotNetNuGetDeleteSettings.ForceEnglishOutput"/></li>
    ///     <li><c>--interactive</c> via <see cref="DotNetNuGetDeleteSettings.Interactive"/></li>
    ///     <li><c>--no-service-endpoint</c> via <see cref="DotNetNuGetDeleteSettings.NoServiceEndpoint"/></li>
    ///     <li><c>--non-interactive</c> via <see cref="DotNetNuGetDeleteSettings.NonInteractive"/></li>
    ///     <li><c>--source</c> via <see cref="DotNetNuGetDeleteSettings.Source"/></li>
    ///   </ul>
    /// </remarks>
    public static IReadOnlyCollection<Output> DotNetNuGetDelete(DotNetNuGetDeleteSettings toolSettings = null)
    {
        toolSettings = toolSettings ?? new DotNetNuGetDeleteSettings();
        using var process = ProcessTasks.StartProcess(toolSettings);
        toolSettings.ProcessExitHandler.Invoke(toolSettings, process.AssertWaitForExit());
        return process.Output;
    }
    /// <summary>
    ///   <p>The dotnet nuget delete command deletes or unlists a package from the server. For nuget.org, the action is to unlist the package.</p>
    ///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/dotnet/core/tools/">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>&lt;packageId&gt;</c> via <see cref="DotNetNuGetDeleteSettings.PackageId"/></li>
    ///     <li><c>&lt;packageVersion&gt;</c> via <see cref="DotNetNuGetDeleteSettings.PackageVersion"/></li>
    ///     <li><c>--api-key</c> via <see cref="DotNetNuGetDeleteSettings.ApiKey"/></li>
    ///     <li><c>--force-english-output</c> via <see cref="DotNetNuGetDeleteSettings.ForceEnglishOutput"/></li>
    ///     <li><c>--interactive</c> via <see cref="DotNetNuGetDeleteSettings.Interactive"/></li>
    ///     <li><c>--no-service-endpoint</c> via <see cref="DotNetNuGetDeleteSettings.NoServiceEndpoint"/></li>
    ///     <li><c>--non-interactive</c> via <see cref="DotNetNuGetDeleteSettings.NonInteractive"/></li>
    ///     <li><c>--source</c> via <see cref="DotNetNuGetDeleteSettings.Source"/></li>
    ///   </ul>
    /// </remarks>
    public static IReadOnlyCollection<Output> DotNetNuGetDelete(Configure<DotNetNuGetDeleteSettings> configurator)
    {
        return DotNetNuGetDelete(configurator(new DotNetNuGetDeleteSettings()));
    }
    /// <summary>
    ///   <p>The dotnet nuget delete command deletes or unlists a package from the server. For nuget.org, the action is to unlist the package.</p>
    ///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/dotnet/core/tools/">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>&lt;packageId&gt;</c> via <see cref="DotNetNuGetDeleteSettings.PackageId"/></li>
    ///     <li><c>&lt;packageVersion&gt;</c> via <see cref="DotNetNuGetDeleteSettings.PackageVersion"/></li>
    ///     <li><c>--api-key</c> via <see cref="DotNetNuGetDeleteSettings.ApiKey"/></li>
    ///     <li><c>--force-english-output</c> via <see cref="DotNetNuGetDeleteSettings.ForceEnglishOutput"/></li>
    ///     <li><c>--interactive</c> via <see cref="DotNetNuGetDeleteSettings.Interactive"/></li>
    ///     <li><c>--no-service-endpoint</c> via <see cref="DotNetNuGetDeleteSettings.NoServiceEndpoint"/></li>
    ///     <li><c>--non-interactive</c> via <see cref="DotNetNuGetDeleteSettings.NonInteractive"/></li>
    ///     <li><c>--source</c> via <see cref="DotNetNuGetDeleteSettings.Source"/></li>
    ///   </ul>
    /// </remarks>
    public static IEnumerable<(DotNetNuGetDeleteSettings Settings, IReadOnlyCollection<Output> Output)> DotNetNuGetDelete(CombinatorialConfigure<DotNetNuGetDeleteSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false)
    {
        return configurator.Invoke(DotNetNuGetDelete, DotNetLogger, degreeOfParallelism, completeOnFailure);
    }
    /// <summary>
    ///   <p>The dotnet nuget sign command signs all the packages matching the first argument with a certificate. The certificate with the private key can be obtained from a file or from a certificate installed in a certificate store by providing a subject name or a SHA-1 fingerprint.</p>
    ///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/dotnet/core/tools/">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>&lt;targetPath&gt;</c> via <see cref="DotNetNuGetSignSettings.TargetPath"/></li>
    ///     <li><c>--certificate-fingerprint</c> via <see cref="DotNetNuGetSignSettings.CertificateFingerprint"/></li>
    ///     <li><c>--certificate-password</c> via <see cref="DotNetNuGetSignSettings.CertificatePassword"/></li>
    ///     <li><c>--certificate-path</c> via <see cref="DotNetNuGetSignSettings.CertificatePath"/></li>
    ///     <li><c>--certificate-store-location</c> via <see cref="DotNetNuGetSignSettings.CertificateStoreLocation"/></li>
    ///     <li><c>--certificate-store-name</c> via <see cref="DotNetNuGetSignSettings.CertificateStoreName"/></li>
    ///     <li><c>--certificate-subject-name</c> via <see cref="DotNetNuGetSignSettings.CertificateSubjectName"/></li>
    ///     <li><c>--hash-algorithm</c> via <see cref="DotNetNuGetSignSettings.HashAlgorithm"/></li>
    ///     <li><c>--output</c> via <see cref="DotNetNuGetSignSettings.Output"/></li>
    ///     <li><c>--overwrite</c> via <see cref="DotNetNuGetSignSettings.Overwrite"/></li>
    ///     <li><c>--timestamp-hash-algorithm</c> via <see cref="DotNetNuGetSignSettings.TimestampHashAlgorithm"/></li>
    ///     <li><c>--timestamper</c> via <see cref="DotNetNuGetSignSettings.Timestamper"/></li>
    ///     <li><c>--verbosity</c> via <see cref="DotNetNuGetSignSettings.Verbosity"/></li>
    ///   </ul>
    /// </remarks>
    public static IReadOnlyCollection<Output> DotNetNuGetSign(DotNetNuGetSignSettings toolSettings = null)
    {
        toolSettings = toolSettings ?? new DotNetNuGetSignSettings();
        using var process = ProcessTasks.StartProcess(toolSettings);
        toolSettings.ProcessExitHandler.Invoke(toolSettings, process.AssertWaitForExit());
        return process.Output;
    }
    /// <summary>
    ///   <p>The dotnet nuget sign command signs all the packages matching the first argument with a certificate. The certificate with the private key can be obtained from a file or from a certificate installed in a certificate store by providing a subject name or a SHA-1 fingerprint.</p>
    ///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/dotnet/core/tools/">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>&lt;targetPath&gt;</c> via <see cref="DotNetNuGetSignSettings.TargetPath"/></li>
    ///     <li><c>--certificate-fingerprint</c> via <see cref="DotNetNuGetSignSettings.CertificateFingerprint"/></li>
    ///     <li><c>--certificate-password</c> via <see cref="DotNetNuGetSignSettings.CertificatePassword"/></li>
    ///     <li><c>--certificate-path</c> via <see cref="DotNetNuGetSignSettings.CertificatePath"/></li>
    ///     <li><c>--certificate-store-location</c> via <see cref="DotNetNuGetSignSettings.CertificateStoreLocation"/></li>
    ///     <li><c>--certificate-store-name</c> via <see cref="DotNetNuGetSignSettings.CertificateStoreName"/></li>
    ///     <li><c>--certificate-subject-name</c> via <see cref="DotNetNuGetSignSettings.CertificateSubjectName"/></li>
    ///     <li><c>--hash-algorithm</c> via <see cref="DotNetNuGetSignSettings.HashAlgorithm"/></li>
    ///     <li><c>--output</c> via <see cref="DotNetNuGetSignSettings.Output"/></li>
    ///     <li><c>--overwrite</c> via <see cref="DotNetNuGetSignSettings.Overwrite"/></li>
    ///     <li><c>--timestamp-hash-algorithm</c> via <see cref="DotNetNuGetSignSettings.TimestampHashAlgorithm"/></li>
    ///     <li><c>--timestamper</c> via <see cref="DotNetNuGetSignSettings.Timestamper"/></li>
    ///     <li><c>--verbosity</c> via <see cref="DotNetNuGetSignSettings.Verbosity"/></li>
    ///   </ul>
    /// </remarks>
    public static IReadOnlyCollection<Output> DotNetNuGetSign(Configure<DotNetNuGetSignSettings> configurator)
    {
        return DotNetNuGetSign(configurator(new DotNetNuGetSignSettings()));
    }
    /// <summary>
    ///   <p>The dotnet nuget sign command signs all the packages matching the first argument with a certificate. The certificate with the private key can be obtained from a file or from a certificate installed in a certificate store by providing a subject name or a SHA-1 fingerprint.</p>
    ///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/dotnet/core/tools/">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>&lt;targetPath&gt;</c> via <see cref="DotNetNuGetSignSettings.TargetPath"/></li>
    ///     <li><c>--certificate-fingerprint</c> via <see cref="DotNetNuGetSignSettings.CertificateFingerprint"/></li>
    ///     <li><c>--certificate-password</c> via <see cref="DotNetNuGetSignSettings.CertificatePassword"/></li>
    ///     <li><c>--certificate-path</c> via <see cref="DotNetNuGetSignSettings.CertificatePath"/></li>
    ///     <li><c>--certificate-store-location</c> via <see cref="DotNetNuGetSignSettings.CertificateStoreLocation"/></li>
    ///     <li><c>--certificate-store-name</c> via <see cref="DotNetNuGetSignSettings.CertificateStoreName"/></li>
    ///     <li><c>--certificate-subject-name</c> via <see cref="DotNetNuGetSignSettings.CertificateSubjectName"/></li>
    ///     <li><c>--hash-algorithm</c> via <see cref="DotNetNuGetSignSettings.HashAlgorithm"/></li>
    ///     <li><c>--output</c> via <see cref="DotNetNuGetSignSettings.Output"/></li>
    ///     <li><c>--overwrite</c> via <see cref="DotNetNuGetSignSettings.Overwrite"/></li>
    ///     <li><c>--timestamp-hash-algorithm</c> via <see cref="DotNetNuGetSignSettings.TimestampHashAlgorithm"/></li>
    ///     <li><c>--timestamper</c> via <see cref="DotNetNuGetSignSettings.Timestamper"/></li>
    ///     <li><c>--verbosity</c> via <see cref="DotNetNuGetSignSettings.Verbosity"/></li>
    ///   </ul>
    /// </remarks>
    public static IEnumerable<(DotNetNuGetSignSettings Settings, IReadOnlyCollection<Output> Output)> DotNetNuGetSign(CombinatorialConfigure<DotNetNuGetSignSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false)
    {
        return configurator.Invoke(DotNetNuGetSign, DotNetLogger, degreeOfParallelism, completeOnFailure);
    }
    /// <summary>
    ///   <p>The dotnet nuget verify command verifies a signed NuGet package.</p>
    ///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/dotnet/core/tools/">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>&lt;targetPath&gt;</c> via <see cref="DotNetNuGetVerifySettings.TargetPath"/></li>
    ///     <li><c>--certificate-fingerprint</c> via <see cref="DotNetNuGetVerifySettings.CertificateFingerprint"/></li>
    ///     <li><c>--configfile</c> via <see cref="DotNetNuGetVerifySettings.ConfigFile"/></li>
    ///     <li><c>--verbosity</c> via <see cref="DotNetNuGetVerifySettings.Verbosity"/></li>
    ///   </ul>
    /// </remarks>
    public static IReadOnlyCollection<Output> DotNetNuGetVerify(DotNetNuGetVerifySettings toolSettings = null)
    {
        toolSettings = toolSettings ?? new DotNetNuGetVerifySettings();
        using var process = ProcessTasks.StartProcess(toolSettings);
        toolSettings.ProcessExitHandler.Invoke(toolSettings, process.AssertWaitForExit());
        return process.Output;
    }
    /// <summary>
    ///   <p>The dotnet nuget verify command verifies a signed NuGet package.</p>
    ///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/dotnet/core/tools/">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>&lt;targetPath&gt;</c> via <see cref="DotNetNuGetVerifySettings.TargetPath"/></li>
    ///     <li><c>--certificate-fingerprint</c> via <see cref="DotNetNuGetVerifySettings.CertificateFingerprint"/></li>
    ///     <li><c>--configfile</c> via <see cref="DotNetNuGetVerifySettings.ConfigFile"/></li>
    ///     <li><c>--verbosity</c> via <see cref="DotNetNuGetVerifySettings.Verbosity"/></li>
    ///   </ul>
    /// </remarks>
    public static IReadOnlyCollection<Output> DotNetNuGetVerify(Configure<DotNetNuGetVerifySettings> configurator)
    {
        return DotNetNuGetVerify(configurator(new DotNetNuGetVerifySettings()));
    }
    /// <summary>
    ///   <p>The dotnet nuget verify command verifies a signed NuGet package.</p>
    ///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/dotnet/core/tools/">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>&lt;targetPath&gt;</c> via <see cref="DotNetNuGetVerifySettings.TargetPath"/></li>
    ///     <li><c>--certificate-fingerprint</c> via <see cref="DotNetNuGetVerifySettings.CertificateFingerprint"/></li>
    ///     <li><c>--configfile</c> via <see cref="DotNetNuGetVerifySettings.ConfigFile"/></li>
    ///     <li><c>--verbosity</c> via <see cref="DotNetNuGetVerifySettings.Verbosity"/></li>
    ///   </ul>
    /// </remarks>
    public static IEnumerable<(DotNetNuGetVerifySettings Settings, IReadOnlyCollection<Output> Output)> DotNetNuGetVerify(CombinatorialConfigure<DotNetNuGetVerifySettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false)
    {
        return configurator.Invoke(DotNetNuGetVerify, DotNetLogger, degreeOfParallelism, completeOnFailure);
    }
}
#region DotNetNuGetPushSettings
/// <summary>
///   Used within <see cref="DotNetTasks"/>.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Serializable]
public partial class DotNetNuGetPushSettings : ToolSettings
{
    /// <summary>
    ///   Path to the DotNet executable.
    /// </summary>
    public override string ProcessToolPath => base.ProcessToolPath ?? DotNetTasks.DotNetPath;
    public override Action<OutputType, string> ProcessLogger => base.ProcessLogger ?? DotNetTasks.DotNetLogger;
    public override Action<ToolSettings, IProcess> ProcessExitHandler => base.ProcessExitHandler ?? DotNetTasks.DotNetExitHandler;
    /// <summary>
    ///   Path of the package to push.
    /// </summary>
    public virtual string TargetPath { get; internal set; }
    /// <summary>
    ///   Specifies the server URL. This option is required unless <c>DefaultPushSource</c> config value is set in the NuGet config file.
    /// </summary>
    public virtual string Source { get; internal set; }
    /// <summary>
    ///   Specifies the symbol server URL.
    /// </summary>
    public virtual string SymbolSource { get; internal set; }
    /// <summary>
    ///   Specifies the timeout for pushing to a server in seconds. Defaults to 300 seconds (5 minutes). Specifying 0 (zero seconds) applies the default value.
    /// </summary>
    public virtual int? Timeout { get; internal set; }
    /// <summary>
    ///   The API key for the server.
    /// </summary>
    public virtual string ApiKey { get; internal set; }
    /// <summary>
    ///   The API key for the symbol server.
    /// </summary>
    public virtual string SymbolApiKey { get; internal set; }
    /// <summary>
    ///   Disables buffering when pushing to an HTTP(S) server to decrease memory usage.
    /// </summary>
    public virtual bool? DisableBuffering { get; internal set; }
    /// <summary>
    ///   Doesn't push symbols (even if present).
    /// </summary>
    public virtual bool? NoSymbols { get; internal set; }
    /// <summary>
    ///   Forces all logged output in English.
    /// </summary>
    public virtual bool? ForceEnglishOutput { get; internal set; }
    /// <summary>
    ///   When pushing multiple packages to an HTTP(S) server, treats any 409 Conflict response as a warning so that the push can continue. Available since .NET Core 3.1 SDK.
    /// </summary>
    public virtual bool? SkipDuplicate { get; internal set; }
    /// <summary>
    ///   Doesn't append <c>api/v2/package</c> to the source URL. Option available since .NET Core 2.1 SDK.
    /// </summary>
    public virtual bool? NoServiceEndpoint { get; internal set; }
    protected override Arguments ConfigureProcessArguments(Arguments arguments)
    {
        arguments
          .Add("nuget push")
          .Add("{value}", TargetPath)
          .Add("--source {value}", Source)
          .Add("--symbol-source {value}", SymbolSource)
          .Add("--timeout {value}", Timeout)
          .Add("--api-key {value}", ApiKey, secret: true)
          .Add("--symbol-api-key {value}", SymbolApiKey, secret: true)
          .Add("--disable-buffering", DisableBuffering)
          .Add("--no-symbols", NoSymbols)
          .Add("--force-english-output", ForceEnglishOutput)
          .Add("--skip-duplicate", SkipDuplicate)
          .Add("--no-service-endpoint", NoServiceEndpoint);
        return base.ConfigureProcessArguments(arguments);
    }
}
#endregion
#region DotNetNuGetDeleteSettings
/// <summary>
///   Used within <see cref="DotNetTasks"/>.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Serializable]
public partial class DotNetNuGetDeleteSettings : ToolSettings
{
    /// <summary>
    ///   Path to the DotNet executable.
    /// </summary>
    public override string ProcessToolPath => base.ProcessToolPath ?? DotNetTasks.DotNetPath;
    public override Action<OutputType, string> ProcessLogger => base.ProcessLogger ?? DotNetTasks.DotNetLogger;
    public override Action<ToolSettings, IProcess> ProcessExitHandler => base.ProcessExitHandler ?? DotNetTasks.DotNetExitHandler;
    /// <summary>
    ///   Package Id to delete. The exact behavior depends on the source. For local folders, for instance, the package is deleted; for nuget.org the package is unlisted.
    /// </summary>
    public virtual string PackageId { get; internal set; }
    /// <summary>
    ///   Package Version to delete. The exact behavior depends on the source. For local folders, for instance, the package is deleted; for nuget.org the package is unlisted.
    /// </summary>
    public virtual string PackageVersion { get; internal set; }
    /// <summary>
    ///   Specifies the server URL. This option is required unless <c>DefaultPushSource</c> config value is set in the NuGet config file.
    /// </summary>
    public virtual string Source { get; internal set; }
    /// <summary>
    ///   The API key for the server.
    /// </summary>
    public virtual string ApiKey { get; internal set; }
    /// <summary>
    ///   Forces all logged output in English.
    /// </summary>
    public virtual bool? ForceEnglishOutput { get; internal set; }
    /// <summary>
    ///   Doesn't append <c>api/v2/package</c> to the source URL. Option available since .NET Core 2.1 SDK.
    /// </summary>
    public virtual bool? NoServiceEndpoint { get; internal set; }
    /// <summary>
    ///   Allows the command to stop and wait for user input or action. For example, to complete authentication. Available since .NET Core 3.0 SDK.
    /// </summary>
    public virtual bool? Interactive { get; internal set; }
    /// <summary>
    ///   Doesn't prompt for user input or confirmations.
    /// </summary>
    public virtual bool? NonInteractive { get; internal set; }
    protected override Arguments ConfigureProcessArguments(Arguments arguments)
    {
        arguments
          .Add("nuget delete")
          .Add("{value}", PackageId)
          .Add("{value}", PackageVersion)
          .Add("--source {value}", Source)
          .Add("--api-key {value}", ApiKey, secret: true)
          .Add("--force-english-output", ForceEnglishOutput)
          .Add("--no-service-endpoint", NoServiceEndpoint)
          .Add("--interactive", Interactive)
          .Add("--non-interactive", NonInteractive);
        return base.ConfigureProcessArguments(arguments);
    }
}
#endregion
#region DotNetNuGetSignSettings
/// <summary>
///   Used within <see cref="DotNetTasks"/>.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Serializable]
public partial class DotNetNuGetSignSettings : ToolSettings
{
    /// <summary>
    ///   Path to the DotNet executable.
    /// </summary>
    public override string ProcessToolPath => base.ProcessToolPath ?? DotNetTasks.DotNetPath;
    public override Action<OutputType, string> ProcessLogger => base.ProcessLogger ?? DotNetTasks.DotNetLogger;
    public override Action<ToolSettings, IProcess> ProcessExitHandler => base.ProcessExitHandler ?? DotNetTasks.DotNetExitHandler;
    /// <summary>
    ///   Path of the package to sign.
    /// </summary>
    public virtual string TargetPath { get; internal set; }
    /// <summary>
    ///   Specifies the file path to the certificate to be used in signing the package.
    /// </summary>
    public virtual string CertificatePath { get; internal set; }
    /// <summary>
    ///   Specifies the name of the X.509 certificate store to use to search for the certificate. Defaults to 'My', the X.509 certificate store for personal certificates. This option should be used when specifying the certificate via --certificate-subject-name or --certificate-fingerprint options.
    /// </summary>
    public virtual string CertificateStoreName { get; internal set; }
    /// <summary>
    ///   Specifies the name of the X.509 certificate store use to search for the certificate. Defaults to 'CurrentUser', the X.509 certificate store used by the current user. This option should be used when specifying the certificate via --certificate-subject-name or --certificate-fingerprint options.
    /// </summary>
    public virtual string CertificateStoreLocation { get; internal set; }
    /// <summary>
    ///   Specifies the subject name of the certificate used to search a local certificate store for the certificate. The search is a case-insensitive string comparison using the supplied value, which finds all certificates with the subject name containing that string, regardless of other subject values. The certificate store can be specified by --certificate-store-name and --certificate-store-location options.
    /// </summary>
    public virtual string CertificateSubjectName { get; internal set; }
    /// <summary>
    ///   Specifies the fingerprint of the certificate used to search a local certificate store for the certificate. Starting with .NET 9, this option can be used to specify the SHA-1, SHA-256, SHA-384, or SHA-512 fingerprint of the certificate. However, a NU3043 warning is raised when a SHA-1 certificate fingerprint is used because it is no longer considered secure.
    /// </summary>
    public virtual string CertificateFingerprint { get; internal set; }
    /// <summary>
    ///   Specifies the certificate password, if needed. If a certificate is password protected but no password is provided, the sign command will fail.
    /// </summary>
    public virtual string CertificatePassword { get; internal set; }
    /// <summary>
    ///   Hash algorithm to be used to sign the package. Defaults to SHA256. Possible values are SHA256, SHA384, and SHA512.
    /// </summary>
    public virtual DotNetNuGetSignHashAlgorithm HashAlgorithm { get; internal set; }
    /// <summary>
    ///   Specifies the directory where the signed package should be saved. If this option isn't specified, by default the original package is overwritten by the signed package.
    /// </summary>
    public virtual string Output { get; internal set; }
    /// <summary>
    ///   Indicate that the current signature should be overwritten. By default the command will fail if the package already has a signature.
    /// </summary>
    public virtual bool? Overwrite { get; internal set; }
    /// <summary>
    ///   Hash algorithm to be used by the RFC 3161 timestamp server. Defaults to SHA256.
    /// </summary>
    public virtual DotNetNuGetSignHashAlgorithm TimestampHashAlgorithm { get; internal set; }
    /// <summary>
    ///   URL to an RFC 3161 timestamping server.
    /// </summary>
    public virtual string Timestamper { get; internal set; }
    /// <summary>
    ///   Sets the verbosity level of the command. Allowed values are <c>q[uiet]</c>, <c>m[inimal]</c>, <c>n[ormal]</c>, <c>d[etailed]</c>, and <c>diag[nostic]</c>.
    /// </summary>
    public virtual DotNetVerbosity Verbosity { get; internal set; }
    protected override Arguments ConfigureProcessArguments(Arguments arguments)
    {
        arguments
          .Add("nuget sign")
          .Add("{value}", TargetPath)
          .Add("--certificate-path {value}", CertificatePath)
          .Add("--certificate-store-name {value}", CertificateStoreName)
          .Add("--certificate-store-location {value}", CertificateStoreLocation)
          .Add("--certificate-subject-name {value}", CertificateSubjectName)
          .Add("--certificate-fingerprint {value}", CertificateFingerprint)
          .Add("--certificate-password {value}", CertificatePassword, secret: true)
          .Add("--hash-algorithm {value}", HashAlgorithm)
          .Add("--output {value}", Output)
          .Add("--overwrite", Overwrite)
          .Add("--timestamp-hash-algorithm {value}", TimestampHashAlgorithm)
          .Add("--timestamper {value}", Timestamper)
          .Add("--verbosity {value}", Verbosity);
        return base.ConfigureProcessArguments(arguments);
    }
}
#endregion
#region DotNetNuGetVerifySettings
/// <summary>
///   Used within <see cref="DotNetTasks"/>.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Serializable]
public partial class DotNetNuGetVerifySettings : ToolSettings
{
    /// <summary>
    ///   Path to the DotNet executable.
    /// </summary>
    public override string ProcessToolPath => base.ProcessToolPath ?? DotNetTasks.DotNetPath;
    public override Action<OutputType, string> ProcessLogger => base.ProcessLogger ?? DotNetTasks.DotNetLogger;
    public override Action<ToolSettings, IProcess> ProcessExitHandler => base.ProcessExitHandler ?? DotNetTasks.DotNetExitHandler;
    /// <summary>
    ///   Path of the package to verify.
    /// </summary>
    public virtual string TargetPath { get; internal set; }
    /// <summary>
    ///   Verify that the signer certificate matches with one of the specified SHA256 fingerprints. This option can be supplied multiple times to provide multiple fingerprints.
    /// </summary>
    public virtual string CertificateFingerprint { get; internal set; }
    /// <summary>
    ///   The NuGet configuration file (nuget.config) to use.
    /// </summary>
    public virtual string ConfigFile { get; internal set; }
    /// <summary>
    ///   Sets the verbosity level of the command. Allowed values are <c>q[uiet]</c>, <c>m[inimal]</c>, <c>n[ormal]</c>, <c>d[etailed]</c>, and <c>diag[nostic]</c>.
    /// </summary>
    public virtual DotNetVerbosity Verbosity { get; internal set; }
    protected override Arguments ConfigureProcessArguments(Arguments arguments)
    {
        arguments
          .Add("nuget verify")
          .Add("{value}", TargetPath)
          .Add("--certificate-fingerprint {value}", CertificateFingerprint)
          .Add("--configfile {value}", ConfigFile)
          .Add("--verbosity {value}", Verbosity);
        return base.ConfigureProcessArguments(arguments);
    }
}
#endregion
#region DotNetNuGetPushSettingsExtensions
/// <summary>
///   Used within <see cref="DotNetTasks"/>.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class DotNetNuGetPushSettingsExtensions
{
    #region TargetPath
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetPushSettings.TargetPath"/></em></p>
    ///   <p>Path of the package to push.</p>
    /// </summary>
    [Pure]
    public static T SetTargetPath<T>(this T toolSettings, string targetPath) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.TargetPath = targetPath;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetPushSettings.TargetPath"/></em></p>
    ///   <p>Path of the package to push.</p>
    /// </summary>
    [Pure]
    public static T ResetTargetPath<T>(this T toolSettings) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.TargetPath = null;
        return toolSettings;
    }
    #endregion
    #region Source
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetPushSettings.Source"/></em></p>
    ///   <p>Specifies the server URL. This option is required unless <c>DefaultPushSource</c> config value is set in the NuGet config file.</p>
    /// </summary>
    [Pure]
    public static T SetSource<T>(this T toolSettings, string source) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Source = source;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetPushSettings.Source"/></em></p>
    ///   <p>Specifies the server URL. This option is required unless <c>DefaultPushSource</c> config value is set in the NuGet config file.</p>
    /// </summary>
    [Pure]
    public static T ResetSource<T>(this T toolSettings) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Source = null;
        return toolSettings;
    }
    #endregion
    #region SymbolSource
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetPushSettings.SymbolSource"/></em></p>
    ///   <p>Specifies the symbol server URL.</p>
    /// </summary>
    [Pure]
    public static T SetSymbolSource<T>(this T toolSettings, string symbolSource) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.SymbolSource = symbolSource;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetPushSettings.SymbolSource"/></em></p>
    ///   <p>Specifies the symbol server URL.</p>
    /// </summary>
    [Pure]
    public static T ResetSymbolSource<T>(this T toolSettings) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.SymbolSource = null;
        return toolSettings;
    }
    #endregion
    #region Timeout
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetPushSettings.Timeout"/></em></p>
    ///   <p>Specifies the timeout for pushing to a server in seconds. Defaults to 300 seconds (5 minutes). Specifying 0 (zero seconds) applies the default value.</p>
    /// </summary>
    [Pure]
    public static T SetTimeout<T>(this T toolSettings, int? timeout) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Timeout = timeout;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetPushSettings.Timeout"/></em></p>
    ///   <p>Specifies the timeout for pushing to a server in seconds. Defaults to 300 seconds (5 minutes). Specifying 0 (zero seconds) applies the default value.</p>
    /// </summary>
    [Pure]
    public static T ResetTimeout<T>(this T toolSettings) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Timeout = null;
        return toolSettings;
    }
    #endregion
    #region ApiKey
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetPushSettings.ApiKey"/></em></p>
    ///   <p>The API key for the server.</p>
    /// </summary>
    [Pure]
    public static T SetApiKey<T>(this T toolSettings, [Secret] string apiKey) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ApiKey = apiKey;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetPushSettings.ApiKey"/></em></p>
    ///   <p>The API key for the server.</p>
    /// </summary>
    [Pure]
    public static T ResetApiKey<T>(this T toolSettings) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ApiKey = null;
        return toolSettings;
    }
    #endregion
    #region SymbolApiKey
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetPushSettings.SymbolApiKey"/></em></p>
    ///   <p>The API key for the symbol server.</p>
    /// </summary>
    [Pure]
    public static T SetSymbolApiKey<T>(this T toolSettings, [Secret] string symbolApiKey) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.SymbolApiKey = symbolApiKey;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetPushSettings.SymbolApiKey"/></em></p>
    ///   <p>The API key for the symbol server.</p>
    /// </summary>
    [Pure]
    public static T ResetSymbolApiKey<T>(this T toolSettings) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.SymbolApiKey = null;
        return toolSettings;
    }
    #endregion
    #region DisableBuffering
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetPushSettings.DisableBuffering"/></em></p>
    ///   <p>Disables buffering when pushing to an HTTP(S) server to decrease memory usage.</p>
    /// </summary>
    [Pure]
    public static T SetDisableBuffering<T>(this T toolSettings, bool? disableBuffering) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.DisableBuffering = disableBuffering;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetPushSettings.DisableBuffering"/></em></p>
    ///   <p>Disables buffering when pushing to an HTTP(S) server to decrease memory usage.</p>
    /// </summary>
    [Pure]
    public static T ResetDisableBuffering<T>(this T toolSettings) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.DisableBuffering = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="DotNetNuGetPushSettings.DisableBuffering"/></em></p>
    ///   <p>Disables buffering when pushing to an HTTP(S) server to decrease memory usage.</p>
    /// </summary>
    [Pure]
    public static T EnableDisableBuffering<T>(this T toolSettings) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.DisableBuffering = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="DotNetNuGetPushSettings.DisableBuffering"/></em></p>
    ///   <p>Disables buffering when pushing to an HTTP(S) server to decrease memory usage.</p>
    /// </summary>
    [Pure]
    public static T DisableDisableBuffering<T>(this T toolSettings) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.DisableBuffering = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="DotNetNuGetPushSettings.DisableBuffering"/></em></p>
    ///   <p>Disables buffering when pushing to an HTTP(S) server to decrease memory usage.</p>
    /// </summary>
    [Pure]
    public static T ToggleDisableBuffering<T>(this T toolSettings) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.DisableBuffering = !toolSettings.DisableBuffering;
        return toolSettings;
    }
    #endregion
    #region NoSymbols
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetPushSettings.NoSymbols"/></em></p>
    ///   <p>Doesn't push symbols (even if present).</p>
    /// </summary>
    [Pure]
    public static T SetNoSymbols<T>(this T toolSettings, bool? noSymbols) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NoSymbols = noSymbols;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetPushSettings.NoSymbols"/></em></p>
    ///   <p>Doesn't push symbols (even if present).</p>
    /// </summary>
    [Pure]
    public static T ResetNoSymbols<T>(this T toolSettings) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NoSymbols = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="DotNetNuGetPushSettings.NoSymbols"/></em></p>
    ///   <p>Doesn't push symbols (even if present).</p>
    /// </summary>
    [Pure]
    public static T EnableNoSymbols<T>(this T toolSettings) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NoSymbols = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="DotNetNuGetPushSettings.NoSymbols"/></em></p>
    ///   <p>Doesn't push symbols (even if present).</p>
    /// </summary>
    [Pure]
    public static T DisableNoSymbols<T>(this T toolSettings) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NoSymbols = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="DotNetNuGetPushSettings.NoSymbols"/></em></p>
    ///   <p>Doesn't push symbols (even if present).</p>
    /// </summary>
    [Pure]
    public static T ToggleNoSymbols<T>(this T toolSettings) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NoSymbols = !toolSettings.NoSymbols;
        return toolSettings;
    }
    #endregion
    #region ForceEnglishOutput
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetPushSettings.ForceEnglishOutput"/></em></p>
    ///   <p>Forces all logged output in English.</p>
    /// </summary>
    [Pure]
    public static T SetForceEnglishOutput<T>(this T toolSettings, bool? forceEnglishOutput) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ForceEnglishOutput = forceEnglishOutput;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetPushSettings.ForceEnglishOutput"/></em></p>
    ///   <p>Forces all logged output in English.</p>
    /// </summary>
    [Pure]
    public static T ResetForceEnglishOutput<T>(this T toolSettings) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ForceEnglishOutput = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="DotNetNuGetPushSettings.ForceEnglishOutput"/></em></p>
    ///   <p>Forces all logged output in English.</p>
    /// </summary>
    [Pure]
    public static T EnableForceEnglishOutput<T>(this T toolSettings) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ForceEnglishOutput = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="DotNetNuGetPushSettings.ForceEnglishOutput"/></em></p>
    ///   <p>Forces all logged output in English.</p>
    /// </summary>
    [Pure]
    public static T DisableForceEnglishOutput<T>(this T toolSettings) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ForceEnglishOutput = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="DotNetNuGetPushSettings.ForceEnglishOutput"/></em></p>
    ///   <p>Forces all logged output in English.</p>
    /// </summary>
    [Pure]
    public static T ToggleForceEnglishOutput<T>(this T toolSettings) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ForceEnglishOutput = !toolSettings.ForceEnglishOutput;
        return toolSettings;
    }
    #endregion
    #region SkipDuplicate
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetPushSettings.SkipDuplicate"/></em></p>
    ///   <p>When pushing multiple packages to an HTTP(S) server, treats any 409 Conflict response as a warning so that the push can continue. Available since .NET Core 3.1 SDK.</p>
    /// </summary>
    [Pure]
    public static T SetSkipDuplicate<T>(this T toolSettings, bool? skipDuplicate) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.SkipDuplicate = skipDuplicate;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetPushSettings.SkipDuplicate"/></em></p>
    ///   <p>When pushing multiple packages to an HTTP(S) server, treats any 409 Conflict response as a warning so that the push can continue. Available since .NET Core 3.1 SDK.</p>
    /// </summary>
    [Pure]
    public static T ResetSkipDuplicate<T>(this T toolSettings) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.SkipDuplicate = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="DotNetNuGetPushSettings.SkipDuplicate"/></em></p>
    ///   <p>When pushing multiple packages to an HTTP(S) server, treats any 409 Conflict response as a warning so that the push can continue. Available since .NET Core 3.1 SDK.</p>
    /// </summary>
    [Pure]
    public static T EnableSkipDuplicate<T>(this T toolSettings) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.SkipDuplicate = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="DotNetNuGetPushSettings.SkipDuplicate"/></em></p>
    ///   <p>When pushing multiple packages to an HTTP(S) server, treats any 409 Conflict response as a warning so that the push can continue. Available since .NET Core 3.1 SDK.</p>
    /// </summary>
    [Pure]
    public static T DisableSkipDuplicate<T>(this T toolSettings) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.SkipDuplicate = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="DotNetNuGetPushSettings.SkipDuplicate"/></em></p>
    ///   <p>When pushing multiple packages to an HTTP(S) server, treats any 409 Conflict response as a warning so that the push can continue. Available since .NET Core 3.1 SDK.</p>
    /// </summary>
    [Pure]
    public static T ToggleSkipDuplicate<T>(this T toolSettings) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.SkipDuplicate = !toolSettings.SkipDuplicate;
        return toolSettings;
    }
    #endregion
    #region NoServiceEndpoint
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetPushSettings.NoServiceEndpoint"/></em></p>
    ///   <p>Doesn't append <c>api/v2/package</c> to the source URL. Option available since .NET Core 2.1 SDK.</p>
    /// </summary>
    [Pure]
    public static T SetNoServiceEndpoint<T>(this T toolSettings, bool? noServiceEndpoint) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NoServiceEndpoint = noServiceEndpoint;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetPushSettings.NoServiceEndpoint"/></em></p>
    ///   <p>Doesn't append <c>api/v2/package</c> to the source URL. Option available since .NET Core 2.1 SDK.</p>
    /// </summary>
    [Pure]
    public static T ResetNoServiceEndpoint<T>(this T toolSettings) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NoServiceEndpoint = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="DotNetNuGetPushSettings.NoServiceEndpoint"/></em></p>
    ///   <p>Doesn't append <c>api/v2/package</c> to the source URL. Option available since .NET Core 2.1 SDK.</p>
    /// </summary>
    [Pure]
    public static T EnableNoServiceEndpoint<T>(this T toolSettings) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NoServiceEndpoint = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="DotNetNuGetPushSettings.NoServiceEndpoint"/></em></p>
    ///   <p>Doesn't append <c>api/v2/package</c> to the source URL. Option available since .NET Core 2.1 SDK.</p>
    /// </summary>
    [Pure]
    public static T DisableNoServiceEndpoint<T>(this T toolSettings) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NoServiceEndpoint = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="DotNetNuGetPushSettings.NoServiceEndpoint"/></em></p>
    ///   <p>Doesn't append <c>api/v2/package</c> to the source URL. Option available since .NET Core 2.1 SDK.</p>
    /// </summary>
    [Pure]
    public static T ToggleNoServiceEndpoint<T>(this T toolSettings) where T : DotNetNuGetPushSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NoServiceEndpoint = !toolSettings.NoServiceEndpoint;
        return toolSettings;
    }
    #endregion
}
#endregion
#region DotNetNuGetDeleteSettingsExtensions
/// <summary>
///   Used within <see cref="DotNetTasks"/>.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class DotNetNuGetDeleteSettingsExtensions
{
    #region PackageId
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetDeleteSettings.PackageId"/></em></p>
    ///   <p>Package Id to delete. The exact behavior depends on the source. For local folders, for instance, the package is deleted; for nuget.org the package is unlisted.</p>
    /// </summary>
    [Pure]
    public static T SetPackageId<T>(this T toolSettings, string packageId) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.PackageId = packageId;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetDeleteSettings.PackageId"/></em></p>
    ///   <p>Package Id to delete. The exact behavior depends on the source. For local folders, for instance, the package is deleted; for nuget.org the package is unlisted.</p>
    /// </summary>
    [Pure]
    public static T ResetPackageId<T>(this T toolSettings) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.PackageId = null;
        return toolSettings;
    }
    #endregion
    #region PackageVersion
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetDeleteSettings.PackageVersion"/></em></p>
    ///   <p>Package Version to delete. The exact behavior depends on the source. For local folders, for instance, the package is deleted; for nuget.org the package is unlisted.</p>
    /// </summary>
    [Pure]
    public static T SetPackageVersion<T>(this T toolSettings, string packageVersion) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.PackageVersion = packageVersion;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetDeleteSettings.PackageVersion"/></em></p>
    ///   <p>Package Version to delete. The exact behavior depends on the source. For local folders, for instance, the package is deleted; for nuget.org the package is unlisted.</p>
    /// </summary>
    [Pure]
    public static T ResetPackageVersion<T>(this T toolSettings) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.PackageVersion = null;
        return toolSettings;
    }
    #endregion
    #region Source
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetDeleteSettings.Source"/></em></p>
    ///   <p>Specifies the server URL. This option is required unless <c>DefaultPushSource</c> config value is set in the NuGet config file.</p>
    /// </summary>
    [Pure]
    public static T SetSource<T>(this T toolSettings, string source) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Source = source;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetDeleteSettings.Source"/></em></p>
    ///   <p>Specifies the server URL. This option is required unless <c>DefaultPushSource</c> config value is set in the NuGet config file.</p>
    /// </summary>
    [Pure]
    public static T ResetSource<T>(this T toolSettings) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Source = null;
        return toolSettings;
    }
    #endregion
    #region ApiKey
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetDeleteSettings.ApiKey"/></em></p>
    ///   <p>The API key for the server.</p>
    /// </summary>
    [Pure]
    public static T SetApiKey<T>(this T toolSettings, [Secret] string apiKey) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ApiKey = apiKey;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetDeleteSettings.ApiKey"/></em></p>
    ///   <p>The API key for the server.</p>
    /// </summary>
    [Pure]
    public static T ResetApiKey<T>(this T toolSettings) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ApiKey = null;
        return toolSettings;
    }
    #endregion
    #region ForceEnglishOutput
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetDeleteSettings.ForceEnglishOutput"/></em></p>
    ///   <p>Forces all logged output in English.</p>
    /// </summary>
    [Pure]
    public static T SetForceEnglishOutput<T>(this T toolSettings, bool? forceEnglishOutput) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ForceEnglishOutput = forceEnglishOutput;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetDeleteSettings.ForceEnglishOutput"/></em></p>
    ///   <p>Forces all logged output in English.</p>
    /// </summary>
    [Pure]
    public static T ResetForceEnglishOutput<T>(this T toolSettings) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ForceEnglishOutput = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="DotNetNuGetDeleteSettings.ForceEnglishOutput"/></em></p>
    ///   <p>Forces all logged output in English.</p>
    /// </summary>
    [Pure]
    public static T EnableForceEnglishOutput<T>(this T toolSettings) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ForceEnglishOutput = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="DotNetNuGetDeleteSettings.ForceEnglishOutput"/></em></p>
    ///   <p>Forces all logged output in English.</p>
    /// </summary>
    [Pure]
    public static T DisableForceEnglishOutput<T>(this T toolSettings) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ForceEnglishOutput = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="DotNetNuGetDeleteSettings.ForceEnglishOutput"/></em></p>
    ///   <p>Forces all logged output in English.</p>
    /// </summary>
    [Pure]
    public static T ToggleForceEnglishOutput<T>(this T toolSettings) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ForceEnglishOutput = !toolSettings.ForceEnglishOutput;
        return toolSettings;
    }
    #endregion
    #region NoServiceEndpoint
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetDeleteSettings.NoServiceEndpoint"/></em></p>
    ///   <p>Doesn't append <c>api/v2/package</c> to the source URL. Option available since .NET Core 2.1 SDK.</p>
    /// </summary>
    [Pure]
    public static T SetNoServiceEndpoint<T>(this T toolSettings, bool? noServiceEndpoint) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NoServiceEndpoint = noServiceEndpoint;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetDeleteSettings.NoServiceEndpoint"/></em></p>
    ///   <p>Doesn't append <c>api/v2/package</c> to the source URL. Option available since .NET Core 2.1 SDK.</p>
    /// </summary>
    [Pure]
    public static T ResetNoServiceEndpoint<T>(this T toolSettings) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NoServiceEndpoint = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="DotNetNuGetDeleteSettings.NoServiceEndpoint"/></em></p>
    ///   <p>Doesn't append <c>api/v2/package</c> to the source URL. Option available since .NET Core 2.1 SDK.</p>
    /// </summary>
    [Pure]
    public static T EnableNoServiceEndpoint<T>(this T toolSettings) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NoServiceEndpoint = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="DotNetNuGetDeleteSettings.NoServiceEndpoint"/></em></p>
    ///   <p>Doesn't append <c>api/v2/package</c> to the source URL. Option available since .NET Core 2.1 SDK.</p>
    /// </summary>
    [Pure]
    public static T DisableNoServiceEndpoint<T>(this T toolSettings) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NoServiceEndpoint = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="DotNetNuGetDeleteSettings.NoServiceEndpoint"/></em></p>
    ///   <p>Doesn't append <c>api/v2/package</c> to the source URL. Option available since .NET Core 2.1 SDK.</p>
    /// </summary>
    [Pure]
    public static T ToggleNoServiceEndpoint<T>(this T toolSettings) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NoServiceEndpoint = !toolSettings.NoServiceEndpoint;
        return toolSettings;
    }
    #endregion
    #region Interactive
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetDeleteSettings.Interactive"/></em></p>
    ///   <p>Allows the command to stop and wait for user input or action. For example, to complete authentication. Available since .NET Core 3.0 SDK.</p>
    /// </summary>
    [Pure]
    public static T SetInteractive<T>(this T toolSettings, bool? interactive) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Interactive = interactive;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetDeleteSettings.Interactive"/></em></p>
    ///   <p>Allows the command to stop and wait for user input or action. For example, to complete authentication. Available since .NET Core 3.0 SDK.</p>
    /// </summary>
    [Pure]
    public static T ResetInteractive<T>(this T toolSettings) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Interactive = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="DotNetNuGetDeleteSettings.Interactive"/></em></p>
    ///   <p>Allows the command to stop and wait for user input or action. For example, to complete authentication. Available since .NET Core 3.0 SDK.</p>
    /// </summary>
    [Pure]
    public static T EnableInteractive<T>(this T toolSettings) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Interactive = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="DotNetNuGetDeleteSettings.Interactive"/></em></p>
    ///   <p>Allows the command to stop and wait for user input or action. For example, to complete authentication. Available since .NET Core 3.0 SDK.</p>
    /// </summary>
    [Pure]
    public static T DisableInteractive<T>(this T toolSettings) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Interactive = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="DotNetNuGetDeleteSettings.Interactive"/></em></p>
    ///   <p>Allows the command to stop and wait for user input or action. For example, to complete authentication. Available since .NET Core 3.0 SDK.</p>
    /// </summary>
    [Pure]
    public static T ToggleInteractive<T>(this T toolSettings) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Interactive = !toolSettings.Interactive;
        return toolSettings;
    }
    #endregion
    #region NonInteractive
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetDeleteSettings.NonInteractive"/></em></p>
    ///   <p>Doesn't prompt for user input or confirmations.</p>
    /// </summary>
    [Pure]
    public static T SetNonInteractive<T>(this T toolSettings, bool? nonInteractive) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NonInteractive = nonInteractive;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetDeleteSettings.NonInteractive"/></em></p>
    ///   <p>Doesn't prompt for user input or confirmations.</p>
    /// </summary>
    [Pure]
    public static T ResetNonInteractive<T>(this T toolSettings) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NonInteractive = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="DotNetNuGetDeleteSettings.NonInteractive"/></em></p>
    ///   <p>Doesn't prompt for user input or confirmations.</p>
    /// </summary>
    [Pure]
    public static T EnableNonInteractive<T>(this T toolSettings) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NonInteractive = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="DotNetNuGetDeleteSettings.NonInteractive"/></em></p>
    ///   <p>Doesn't prompt for user input or confirmations.</p>
    /// </summary>
    [Pure]
    public static T DisableNonInteractive<T>(this T toolSettings) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NonInteractive = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="DotNetNuGetDeleteSettings.NonInteractive"/></em></p>
    ///   <p>Doesn't prompt for user input or confirmations.</p>
    /// </summary>
    [Pure]
    public static T ToggleNonInteractive<T>(this T toolSettings) where T : DotNetNuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NonInteractive = !toolSettings.NonInteractive;
        return toolSettings;
    }
    #endregion
}
#endregion
#region DotNetNuGetSignSettingsExtensions
/// <summary>
///   Used within <see cref="DotNetTasks"/>.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class DotNetNuGetSignSettingsExtensions
{
    #region TargetPath
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetSignSettings.TargetPath"/></em></p>
    ///   <p>Path of the package to sign.</p>
    /// </summary>
    [Pure]
    public static T SetTargetPath<T>(this T toolSettings, string targetPath) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.TargetPath = targetPath;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetSignSettings.TargetPath"/></em></p>
    ///   <p>Path of the package to sign.</p>
    /// </summary>
    [Pure]
    public static T ResetTargetPath<T>(this T toolSettings) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.TargetPath = null;
        return toolSettings;
    }
    #endregion
    #region CertificatePath
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetSignSettings.CertificatePath"/></em></p>
    ///   <p>Specifies the file path to the certificate to be used in signing the package.</p>
    /// </summary>
    [Pure]
    public static T SetCertificatePath<T>(this T toolSettings, string certificatePath) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificatePath = certificatePath;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetSignSettings.CertificatePath"/></em></p>
    ///   <p>Specifies the file path to the certificate to be used in signing the package.</p>
    /// </summary>
    [Pure]
    public static T ResetCertificatePath<T>(this T toolSettings) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificatePath = null;
        return toolSettings;
    }
    #endregion
    #region CertificateStoreName
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetSignSettings.CertificateStoreName"/></em></p>
    ///   <p>Specifies the name of the X.509 certificate store to use to search for the certificate. Defaults to 'My', the X.509 certificate store for personal certificates. This option should be used when specifying the certificate via --certificate-subject-name or --certificate-fingerprint options.</p>
    /// </summary>
    [Pure]
    public static T SetCertificateStoreName<T>(this T toolSettings, string certificateStoreName) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificateStoreName = certificateStoreName;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetSignSettings.CertificateStoreName"/></em></p>
    ///   <p>Specifies the name of the X.509 certificate store to use to search for the certificate. Defaults to 'My', the X.509 certificate store for personal certificates. This option should be used when specifying the certificate via --certificate-subject-name or --certificate-fingerprint options.</p>
    /// </summary>
    [Pure]
    public static T ResetCertificateStoreName<T>(this T toolSettings) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificateStoreName = null;
        return toolSettings;
    }
    #endregion
    #region CertificateStoreLocation
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetSignSettings.CertificateStoreLocation"/></em></p>
    ///   <p>Specifies the name of the X.509 certificate store use to search for the certificate. Defaults to 'CurrentUser', the X.509 certificate store used by the current user. This option should be used when specifying the certificate via --certificate-subject-name or --certificate-fingerprint options.</p>
    /// </summary>
    [Pure]
    public static T SetCertificateStoreLocation<T>(this T toolSettings, string certificateStoreLocation) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificateStoreLocation = certificateStoreLocation;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetSignSettings.CertificateStoreLocation"/></em></p>
    ///   <p>Specifies the name of the X.509 certificate store use to search for the certificate. Defaults to 'CurrentUser', the X.509 certificate store used by the current user. This option should be used when specifying the certificate via --certificate-subject-name or --certificate-fingerprint options.</p>
    /// </summary>
    [Pure]
    public static T ResetCertificateStoreLocation<T>(this T toolSettings) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificateStoreLocation = null;
        return toolSettings;
    }
    #endregion
    #region CertificateSubjectName
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetSignSettings.CertificateSubjectName"/></em></p>
    ///   <p>Specifies the subject name of the certificate used to search a local certificate store for the certificate. The search is a case-insensitive string comparison using the supplied value, which finds all certificates with the subject name containing that string, regardless of other subject values. The certificate store can be specified by --certificate-store-name and --certificate-store-location options.</p>
    /// </summary>
    [Pure]
    public static T SetCertificateSubjectName<T>(this T toolSettings, string certificateSubjectName) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificateSubjectName = certificateSubjectName;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetSignSettings.CertificateSubjectName"/></em></p>
    ///   <p>Specifies the subject name of the certificate used to search a local certificate store for the certificate. The search is a case-insensitive string comparison using the supplied value, which finds all certificates with the subject name containing that string, regardless of other subject values. The certificate store can be specified by --certificate-store-name and --certificate-store-location options.</p>
    /// </summary>
    [Pure]
    public static T ResetCertificateSubjectName<T>(this T toolSettings) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificateSubjectName = null;
        return toolSettings;
    }
    #endregion
    #region CertificateFingerprint
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetSignSettings.CertificateFingerprint"/></em></p>
    ///   <p>Specifies the fingerprint of the certificate used to search a local certificate store for the certificate. Starting with .NET 9, this option can be used to specify the SHA-1, SHA-256, SHA-384, or SHA-512 fingerprint of the certificate. However, a NU3043 warning is raised when a SHA-1 certificate fingerprint is used because it is no longer considered secure.</p>
    /// </summary>
    [Pure]
    public static T SetCertificateFingerprint<T>(this T toolSettings, string certificateFingerprint) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificateFingerprint = certificateFingerprint;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetSignSettings.CertificateFingerprint"/></em></p>
    ///   <p>Specifies the fingerprint of the certificate used to search a local certificate store for the certificate. Starting with .NET 9, this option can be used to specify the SHA-1, SHA-256, SHA-384, or SHA-512 fingerprint of the certificate. However, a NU3043 warning is raised when a SHA-1 certificate fingerprint is used because it is no longer considered secure.</p>
    /// </summary>
    [Pure]
    public static T ResetCertificateFingerprint<T>(this T toolSettings) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificateFingerprint = null;
        return toolSettings;
    }
    #endregion
    #region CertificatePassword
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetSignSettings.CertificatePassword"/></em></p>
    ///   <p>Specifies the certificate password, if needed. If a certificate is password protected but no password is provided, the sign command will fail.</p>
    /// </summary>
    [Pure]
    public static T SetCertificatePassword<T>(this T toolSettings, [Secret] string certificatePassword) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificatePassword = certificatePassword;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetSignSettings.CertificatePassword"/></em></p>
    ///   <p>Specifies the certificate password, if needed. If a certificate is password protected but no password is provided, the sign command will fail.</p>
    /// </summary>
    [Pure]
    public static T ResetCertificatePassword<T>(this T toolSettings) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificatePassword = null;
        return toolSettings;
    }
    #endregion
    #region HashAlgorithm
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetSignSettings.HashAlgorithm"/></em></p>
    ///   <p>Hash algorithm to be used to sign the package. Defaults to SHA256. Possible values are SHA256, SHA384, and SHA512.</p>
    /// </summary>
    [Pure]
    public static T SetHashAlgorithm<T>(this T toolSettings, DotNetNuGetSignHashAlgorithm hashAlgorithm) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.HashAlgorithm = hashAlgorithm;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetSignSettings.HashAlgorithm"/></em></p>
    ///   <p>Hash algorithm to be used to sign the package. Defaults to SHA256. Possible values are SHA256, SHA384, and SHA512.</p>
    /// </summary>
    [Pure]
    public static T ResetHashAlgorithm<T>(this T toolSettings) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.HashAlgorithm = null;
        return toolSettings;
    }
    #endregion
    #region Output
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetSignSettings.Output"/></em></p>
    ///   <p>Specifies the directory where the signed package should be saved. If this option isn't specified, by default the original package is overwritten by the signed package.</p>
    /// </summary>
    [Pure]
    public static T SetOutput<T>(this T toolSettings, string output) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Output = output;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetSignSettings.Output"/></em></p>
    ///   <p>Specifies the directory where the signed package should be saved. If this option isn't specified, by default the original package is overwritten by the signed package.</p>
    /// </summary>
    [Pure]
    public static T ResetOutput<T>(this T toolSettings) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Output = null;
        return toolSettings;
    }
    #endregion
    #region Overwrite
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetSignSettings.Overwrite"/></em></p>
    ///   <p>Indicate that the current signature should be overwritten. By default the command will fail if the package already has a signature.</p>
    /// </summary>
    [Pure]
    public static T SetOverwrite<T>(this T toolSettings, bool? overwrite) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Overwrite = overwrite;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetSignSettings.Overwrite"/></em></p>
    ///   <p>Indicate that the current signature should be overwritten. By default the command will fail if the package already has a signature.</p>
    /// </summary>
    [Pure]
    public static T ResetOverwrite<T>(this T toolSettings) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Overwrite = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="DotNetNuGetSignSettings.Overwrite"/></em></p>
    ///   <p>Indicate that the current signature should be overwritten. By default the command will fail if the package already has a signature.</p>
    /// </summary>
    [Pure]
    public static T EnableOverwrite<T>(this T toolSettings) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Overwrite = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="DotNetNuGetSignSettings.Overwrite"/></em></p>
    ///   <p>Indicate that the current signature should be overwritten. By default the command will fail if the package already has a signature.</p>
    /// </summary>
    [Pure]
    public static T DisableOverwrite<T>(this T toolSettings) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Overwrite = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="DotNetNuGetSignSettings.Overwrite"/></em></p>
    ///   <p>Indicate that the current signature should be overwritten. By default the command will fail if the package already has a signature.</p>
    /// </summary>
    [Pure]
    public static T ToggleOverwrite<T>(this T toolSettings) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Overwrite = !toolSettings.Overwrite;
        return toolSettings;
    }
    #endregion
    #region TimestampHashAlgorithm
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetSignSettings.TimestampHashAlgorithm"/></em></p>
    ///   <p>Hash algorithm to be used by the RFC 3161 timestamp server. Defaults to SHA256.</p>
    /// </summary>
    [Pure]
    public static T SetTimestampHashAlgorithm<T>(this T toolSettings, DotNetNuGetSignHashAlgorithm timestampHashAlgorithm) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.TimestampHashAlgorithm = timestampHashAlgorithm;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetSignSettings.TimestampHashAlgorithm"/></em></p>
    ///   <p>Hash algorithm to be used by the RFC 3161 timestamp server. Defaults to SHA256.</p>
    /// </summary>
    [Pure]
    public static T ResetTimestampHashAlgorithm<T>(this T toolSettings) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.TimestampHashAlgorithm = null;
        return toolSettings;
    }
    #endregion
    #region Timestamper
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetSignSettings.Timestamper"/></em></p>
    ///   <p>URL to an RFC 3161 timestamping server.</p>
    /// </summary>
    [Pure]
    public static T SetTimestamper<T>(this T toolSettings, string timestamper) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Timestamper = timestamper;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetSignSettings.Timestamper"/></em></p>
    ///   <p>URL to an RFC 3161 timestamping server.</p>
    /// </summary>
    [Pure]
    public static T ResetTimestamper<T>(this T toolSettings) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Timestamper = null;
        return toolSettings;
    }
    #endregion
    #region Verbosity
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetSignSettings.Verbosity"/></em></p>
    ///   <p>Sets the verbosity level of the command. Allowed values are <c>q[uiet]</c>, <c>m[inimal]</c>, <c>n[ormal]</c>, <c>d[etailed]</c>, and <c>diag[nostic]</c>.</p>
    /// </summary>
    [Pure]
    public static T SetVerbosity<T>(this T toolSettings, DotNetVerbosity verbosity) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Verbosity = verbosity;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetSignSettings.Verbosity"/></em></p>
    ///   <p>Sets the verbosity level of the command. Allowed values are <c>q[uiet]</c>, <c>m[inimal]</c>, <c>n[ormal]</c>, <c>d[etailed]</c>, and <c>diag[nostic]</c>.</p>
    /// </summary>
    [Pure]
    public static T ResetVerbosity<T>(this T toolSettings) where T : DotNetNuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Verbosity = null;
        return toolSettings;
    }
    #endregion
}
#endregion
#region DotNetNuGetVerifySettingsExtensions
/// <summary>
///   Used within <see cref="DotNetTasks"/>.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class DotNetNuGetVerifySettingsExtensions
{
    #region TargetPath
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetVerifySettings.TargetPath"/></em></p>
    ///   <p>Path of the package to verify.</p>
    /// </summary>
    [Pure]
    public static T SetTargetPath<T>(this T toolSettings, string targetPath) where T : DotNetNuGetVerifySettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.TargetPath = targetPath;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetVerifySettings.TargetPath"/></em></p>
    ///   <p>Path of the package to verify.</p>
    /// </summary>
    [Pure]
    public static T ResetTargetPath<T>(this T toolSettings) where T : DotNetNuGetVerifySettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.TargetPath = null;
        return toolSettings;
    }
    #endregion
    #region CertificateFingerprint
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetVerifySettings.CertificateFingerprint"/></em></p>
    ///   <p>Verify that the signer certificate matches with one of the specified SHA256 fingerprints. This option can be supplied multiple times to provide multiple fingerprints.</p>
    /// </summary>
    [Pure]
    public static T SetCertificateFingerprint<T>(this T toolSettings, string certificateFingerprint) where T : DotNetNuGetVerifySettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificateFingerprint = certificateFingerprint;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetVerifySettings.CertificateFingerprint"/></em></p>
    ///   <p>Verify that the signer certificate matches with one of the specified SHA256 fingerprints. This option can be supplied multiple times to provide multiple fingerprints.</p>
    /// </summary>
    [Pure]
    public static T ResetCertificateFingerprint<T>(this T toolSettings) where T : DotNetNuGetVerifySettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificateFingerprint = null;
        return toolSettings;
    }
    #endregion
    #region ConfigFile
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetVerifySettings.ConfigFile"/></em></p>
    ///   <p>The NuGet configuration file (nuget.config) to use.</p>
    /// </summary>
    [Pure]
    public static T SetConfigFile<T>(this T toolSettings, string configFile) where T : DotNetNuGetVerifySettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ConfigFile = configFile;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetVerifySettings.ConfigFile"/></em></p>
    ///   <p>The NuGet configuration file (nuget.config) to use.</p>
    /// </summary>
    [Pure]
    public static T ResetConfigFile<T>(this T toolSettings) where T : DotNetNuGetVerifySettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ConfigFile = null;
        return toolSettings;
    }
    #endregion
    #region Verbosity
    /// <summary>
    ///   <p><em>Sets <see cref="DotNetNuGetVerifySettings.Verbosity"/></em></p>
    ///   <p>Sets the verbosity level of the command. Allowed values are <c>q[uiet]</c>, <c>m[inimal]</c>, <c>n[ormal]</c>, <c>d[etailed]</c>, and <c>diag[nostic]</c>.</p>
    /// </summary>
    [Pure]
    public static T SetVerbosity<T>(this T toolSettings, DotNetVerbosity verbosity) where T : DotNetNuGetVerifySettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Verbosity = verbosity;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="DotNetNuGetVerifySettings.Verbosity"/></em></p>
    ///   <p>Sets the verbosity level of the command. Allowed values are <c>q[uiet]</c>, <c>m[inimal]</c>, <c>n[ormal]</c>, <c>d[etailed]</c>, and <c>diag[nostic]</c>.</p>
    /// </summary>
    [Pure]
    public static T ResetVerbosity<T>(this T toolSettings) where T : DotNetNuGetVerifySettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Verbosity = null;
        return toolSettings;
    }
    #endregion
}
#endregion
#region DotNetVerbosity
/// <summary>
///   Used within <see cref="DotNetTasks"/>.
/// </summary>
[PublicAPI]
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<DotNetVerbosity>))]
public partial class DotNetVerbosity : Enumeration
{
    public static DotNetVerbosity quiet = (DotNetVerbosity) "quiet";
    public static DotNetVerbosity minimal = (DotNetVerbosity) "minimal";
    public static DotNetVerbosity normal = (DotNetVerbosity) "normal";
    public static DotNetVerbosity detailed = (DotNetVerbosity) "detailed";
    public static DotNetVerbosity diagnostic = (DotNetVerbosity) "diagnostic";
    public static implicit operator DotNetVerbosity(string value)
    {
        return new DotNetVerbosity { Value = value };
    }
}
#endregion
#region DotNetSymbolPackageFormat
/// <summary>
///   Used within <see cref="DotNetTasks"/>.
/// </summary>
[PublicAPI]
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<DotNetSymbolPackageFormat>))]
public partial class DotNetSymbolPackageFormat : Enumeration
{
    public static DotNetSymbolPackageFormat symbols_nupkg = (DotNetSymbolPackageFormat) "symbols.nupkg";
    public static DotNetSymbolPackageFormat snupkg = (DotNetSymbolPackageFormat) "snupkg";
    public static implicit operator DotNetSymbolPackageFormat(string value)
    {
        return new DotNetSymbolPackageFormat { Value = value };
    }
}
#endregion
#region DotNetNuGetAuthentication
/// <summary>
///   Used within <see cref="DotNetTasks"/>.
/// </summary>
[PublicAPI]
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<DotNetNuGetAuthentication>))]
public partial class DotNetNuGetAuthentication : Enumeration
{
    public static DotNetNuGetAuthentication basic = (DotNetNuGetAuthentication) "basic";
    public static DotNetNuGetAuthentication negotiate = (DotNetNuGetAuthentication) "negotiate";
    public static DotNetNuGetAuthentication kerberos = (DotNetNuGetAuthentication) "kerberos";
    public static DotNetNuGetAuthentication ntlm = (DotNetNuGetAuthentication) "ntlm";
    public static DotNetNuGetAuthentication digest = (DotNetNuGetAuthentication) "digest";
    public static implicit operator DotNetNuGetAuthentication(string value)
    {
        return new DotNetNuGetAuthentication { Value = value };
    }
}
#endregion
#region DotNetFormatSeverity
/// <summary>
///   Used within <see cref="DotNetTasks"/>.
/// </summary>
[PublicAPI]
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<DotNetFormatSeverity>))]
public partial class DotNetFormatSeverity : Enumeration
{
    public static DotNetFormatSeverity info = (DotNetFormatSeverity) "info";
    public static DotNetFormatSeverity warn = (DotNetFormatSeverity) "warn";
    public static DotNetFormatSeverity error = (DotNetFormatSeverity) "error";
    public static implicit operator DotNetFormatSeverity(string value)
    {
        return new DotNetFormatSeverity { Value = value };
    }
}
#endregion
#region DotNetNuGetSignHashAlgorithm
/// <summary>
///   Used within <see cref="DotNetTasks"/>.
/// </summary>
[PublicAPI]
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<DotNetNuGetSignHashAlgorithm>))]
public partial class DotNetNuGetSignHashAlgorithm : Enumeration
{
    public static DotNetNuGetSignHashAlgorithm sha256 = (DotNetNuGetSignHashAlgorithm) "sha256";
    public static DotNetNuGetSignHashAlgorithm sha384 = (DotNetNuGetSignHashAlgorithm) "sha384";
    public static DotNetNuGetSignHashAlgorithm sha512 = (DotNetNuGetSignHashAlgorithm) "sha512";
    public static implicit operator DotNetNuGetSignHashAlgorithm(string value)
    {
        return new DotNetNuGetSignHashAlgorithm { Value = value };
    }
}
#endregion
