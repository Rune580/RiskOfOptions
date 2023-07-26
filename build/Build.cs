using System.IO;
using System.IO.Compression;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace RoO.Build;

class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Compile);

    [Solution] readonly Solution Solution;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("Directory of RoR2")] readonly string? RoR2Dir;
    [Parameter("mod author")] readonly string Author;

    Project GetProject()
    {
        return Solution.GetProject("RiskOfOptions")!;
    }

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            DotNetTasks.DotNetClean(new DotNetCleanSettings().SetProject(GetProject()));
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetTasks.DotNetRestore(new DotNetRestoreSettings().SetProjectFile(GetProject().Path));
        });

    Target UpdateAssetBundles => _ => _
        .Executes(() =>
        {
            var srcDir = RootDirectory / "Unity-RoO-UI" / "AssetBundles" / "StandaloneWindows";
            var destDir = GetProject().Directory / "Resources" / "AssetBundles";

            void CopyBundle(string bundle)
            {
                File.Copy(srcDir / bundle, destDir / bundle, true);
            }

            CopyBundle("uielements");
        });

    Target Compile => _ => _
        .DependsOn(Restore, UpdateAssetBundles)
        .Executes(() =>
        {
            DotNetTasks.DotNetBuild(new DotNetBuildSettings().SetProjectFile(GetProject().Path).SetConfiguration(Configuration));
        });

    Target CopyToUnityProj => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            var buildDir = GetProject().Directory / "bin" / Configuration / "netstandard2.0";
            var destDir = RootDirectory / "Unity-RoO-UI" / "Packages" / "RiskOfOptions";
            
            buildDir.GlobFiles("*.dll", "*.pdb").ForEach(file =>
            {
                var destFile = destDir / file.Name;
                File.Copy(file, destFile, true);
            });
        });

    Target AllTargets => _ => _
        .DependsOn(Compile, CopyToUnityProj);

    Target DeployToRoR2 => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            RoR2Dir.NotNullOrEmpty("Make sure you set the property 'RoR2Dir' to the root folder of your RiskOfRain2 install.");
            var gameDir = (AbsolutePath)RoR2Dir;
            Assert.DirectoryExists(gameDir);
            
            var buildDir = GetProject().Directory / "bin" / Configuration / "netstandard2.0";
            var destDir = gameDir / "BepInEx" / "plugins" / "RiskOfOptions";

            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);
            
            buildDir.GlobFiles("*.dll", "*.pdb").ForEach(file =>
            {
                var destFile = destDir / file.Name;
                File.Copy(file, destFile, true);
            });
        });

    Target BuildThunderStorePackage => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            var buildDir = GetProject().Directory / "bin" / Configuration / "netstandard2.0";
            
            var manifestFile = RootDirectory / "manifest.json";
            var iconFile = RootDirectory / "icon.png";
            var readmeFile = RootDirectory / "README.md";

            var publishDir = buildDir / "publish";

            if (Directory.Exists(publishDir))
                Directory.Delete(publishDir, true);

            Directory.CreateDirectory(publishDir);

            var modDir = publishDir / "RiskOfOptions";
            Directory.CreateDirectory(modDir);
            
            buildDir.GlobFiles("*.dll").ForEach(file =>
            {
                File.Copy(file, modDir / file.Name);
            });
            
            File.Copy(manifestFile, publishDir / manifestFile.Name);
            File.Copy(iconFile, publishDir / iconFile.Name);
            File.Copy(readmeFile, publishDir / readmeFile.Name);

            var manifest = JsonSerializer.Deserialize<ThunderStoreManifest>(File.ReadAllText(manifestFile));
            manifest.NotNull();

            var destFile = buildDir / $"{Author}-{manifest!.name}-{manifest.version_number}.zip";

            if (destFile.Exists())
                destFile.DeleteFile();
            
            ZipFile.CreateFromDirectory(publishDir, destFile);
        });
}