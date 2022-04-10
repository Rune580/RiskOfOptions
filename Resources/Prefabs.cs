using System.Reflection;
using UnityEngine;

namespace RiskOfOptions.Resources
{
    public static class Prefabs
    {
        private static AssetBundle _subPanel;
        public static GameObject SubPanel => _subPanel.LoadAsset<GameObject>("Assets/RiskOfOptions/prefabs/SubPanel.prefab");

        internal static void Init()
        {
            _subPanel = LoadBundle("subpanel");
        }

        private static AssetBundle LoadBundle(string name)
        {
            using var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"RiskOfOptions.Resources.AssetBundles.{name}");
            return AssetBundle.LoadFromStream(assetStream);
        }
    }
}