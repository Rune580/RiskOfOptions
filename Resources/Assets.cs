using System.Reflection;
using R2API;
using UnityEngine;

namespace RiskOfOptions.Resources
{
    internal static class Assets
    {
        private static AssetBundle _mainAssetBundle;
        internal static T Load<T>(string asset) where T : UnityEngine.Object
        {
            return _mainAssetBundle.LoadAsset<T>(asset);
        }

        internal static void LoadAssets()
        {
            using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"RiskOfOptions.Resources.riskofoptions"))
            {
                _mainAssetBundle = AssetBundle.LoadFromStream(assetStream);
            }
        }
    }
}