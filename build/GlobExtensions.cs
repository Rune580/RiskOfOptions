using System.Collections.Generic;
using Nuke.Common.IO;

namespace RoO.Build;

public static class GlobExtensions
{
    public static IReadOnlyCollection<AbsolutePath> GlobFilesRecursive(this AbsolutePath directory, params string[] patterns)
    {
        List<AbsolutePath> files = new List<AbsolutePath>();
        foreach (var dir in directory.GlobDirectories(patterns))
        {
            files.AddRange(dir.GlobFilesRecursive(patterns));
        }

        files.AddRange(directory.GlobFiles(patterns));
        return files;
    }
}