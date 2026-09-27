using System;
using System.IO;
using BepInEx.Bootstrap;
using MonoMod.Utils;

namespace RiskOfOptions.Utils;

internal static class FsUtils
{
    public static string AssetBundlesDir
    {
        get
        {
            if (field is null)
            {
                if (CurrentPluginDir is null)
                    throw InvalidInstallException.Create(InvalidInstallException.RequiredDirectory.AssetBundles);
                
                var assetBundlesDir = Path.Combine(CurrentPluginDir, "AssetBundles");
                if (!Directory.Exists(assetBundlesDir))
                    throw InvalidInstallException.Create(InvalidInstallException.RequiredDirectory.AssetBundles);

                field = assetBundlesDir;
            }

            return field;
        }
    }

    private static string? CurrentPluginDir
    {
        get
        {
            if (!Chainloader.PluginInfos.TryGetValue(MyPluginInfo.PLUGIN_GUID, out var pluginInfo))
                return null;
            
            var dllLocation = pluginInfo.Location;
            var parentDir = Directory.GetParent(dllLocation);

            return parentDir?.FullName;
        }
    }

    public class InvalidInstallException : Exception
    {
        private InvalidInstallException(string message) : base(message) { }

        public static InvalidInstallException Create(RequiredDirectory directories)
        {
            var possibleDirectories = Enum.GetValues(typeof(RequiredDirectory));
            var directoriesString = "";
            foreach (var value in possibleDirectories)
            {
                var possibleDir = (RequiredDirectory)value;
                
                if (directories.HasFlag(possibleDir))
                {
                    if (!string.IsNullOrEmpty(directoriesString))
                        directoriesString += ", ";
                    
                    directoriesString += possibleDir.ToString().SpacedPascalCase();
                }
            }

            var msg = $"RiskOfOptions failed to find the required directories: [{directoriesString}]!" +
                $"\nIf you manually installed RiskOfOptions, verify that you installed it correctly; RiskOfOptions.dll and the required directories {directoriesString} should be in a subfolder under plugins: \"plugins/Rune580-RiskOfOptions/*\"." +
                $"\nIf you installed via a mod manager (Thunderstore Mod Manager, R2ModMan, or Gale), try clearing the download cache of the mod manager, then re-install RiskOfOptions or the profile itself." +
                $"\nSupport won't be provided for issues related to, or caused by, manual installs.";
            
            Debug.Error(msg);
            return new InvalidInstallException(msg);
        }
        
        [Flags]
        public enum RequiredDirectory
        {
            AssetBundles,
            Locale
        }
    }
}