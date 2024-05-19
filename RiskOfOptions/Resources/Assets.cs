using System.Reflection;
using UnityEngine;

namespace RiskOfOptions.Resources
{
    internal static class Assets
    {
        private static AssetBundle _mainAssetBundle;
        
        internal static T Load<T>(string asset) where T : Object
        {
            return _mainAssetBundle.LoadAsset<T>(asset);
        }

        internal static void LoadAssets()
        {
            using var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"RiskOfOptions.Resources.AssetBundles.riskofoptions");
            _mainAssetBundle = AssetBundle.LoadFromStream(assetStream);
        }
    }
}