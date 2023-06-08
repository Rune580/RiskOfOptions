using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.AddressableAssets;

namespace RoOToolkit.Editor
{
    public static class AddressableUtils
    {
        private static Dictionary<string, List<string>> _contents;

        private static void LoadContents()
        {
            if (_contents != null)
            {
                if (_contents.Count > 0)
                    return;
            }
            
            var allKeys = Addressables.ResourceLocators.SelectMany(locator => locator.Keys).Select(key => key.ToString());

            var allGroups = allKeys.GroupBy(Path.GetDirectoryName);
            _contents = allGroups.ToDictionary(g => g.Key, g => g.OrderBy(k => k).ToList());
        }
        
        public static string GetAddressablePath(string assetName)
        {
            LoadContents();

            try
            {
                return _contents.First(kvp => kvp.Value.Any(v => v.ToLowerInvariant().Contains(assetName.ToLowerInvariant()))).Value.First(v => v.ToLowerInvariant().Contains(assetName.ToLowerInvariant()));
            }
            catch
            {
                return "";
            }
        }
    }
}