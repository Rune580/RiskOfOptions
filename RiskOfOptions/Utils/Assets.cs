using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RiskOfOptions.Utils;

internal static class Assets
{
    private static readonly List<AssetBundle> AssetBundles = [];
    private static readonly Dictionary<string, int> AssetIndices = [];

    public static void AddBundle(string bundleName)
    {
        var bundlePath = Path.Combine(FsUtils.AssetBundlesDir, bundleName);
        var bundle = AssetBundle.LoadFromFile(bundlePath);

        var index = AssetBundles.Count;
        AssetBundles.Add(bundle);

        foreach (var assetName in bundle.GetAllAssetNames())
        {
            var path = assetName.ToLowerInvariant();

            if (path.StartsWith("assets/"))
                path = path["assets/".Length..];

            AssetIndices[path] = index;
        }
    }

    public static T Load<T>(string assetName) where T : Object
    {
        try
        {
            assetName = assetName.ToLowerInvariant();
            if (assetName.StartsWith("assets/"))
                assetName = assetName["assets/".Length..];

            var index = AssetIndices[assetName];
            return AssetBundles[index].LoadAsset<T>($"assets/{assetName}");
        }
        catch (Exception e)
        {
            Debug.Error($"Failed to load asset [{assetName}] exception: {e}");
            throw;
        }
    }
}