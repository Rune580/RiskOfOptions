using System;
using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

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

    Project GetProject()
    {
        return Solution.GetProject("RiskOfOptions");
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

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetTasks.DotNetBuild(new DotNetBuildSettings().SetProjectFile(GetProject().Path));
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
}
