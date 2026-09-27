using System;
using RiskOfOptions.Components.AssetResolution;
using RiskOfOptions.Components.AssetResolution.Data;
using RoOToolkit.Editor.CustomEditors;
using UnityEditor;

namespace RoOToolkit.Editor.ContextActions
{
    public static class AssetResolverActions
    {
        [MenuItem("GameObject/RoO-Toolkit/ResolveAssetReferences", true)]
        [MenuItem("GameObject/RoO-Toolkit/ResetAssetReferences", true)]
        public static bool Validation()
        {
            return Selection.activeGameObject && Selection.activeGameObject.GetComponentInChildren<AssetResolver>();
        }

        [MenuItem("GameObject/RoO-Toolkit/ResolveAssetReferences")]
        public static void Resolve()
        {
            foreach (var resolver in Selection.activeGameObject.GetComponentsInChildren<AssetResolver>())
            {
                if (resolver is not ImageResolver imageResolver)
                    continue;
                
                foreach (var entry in imageResolver.entries)
                {
                    switch (entry.assetType)
                    {
                        case ImageAssetEntry.ImageAssetType.Sprite:
                            ImageResolverEditor.ResolveSprite(imageResolver.transform, entry);
                            break;
                        case ImageAssetEntry.ImageAssetType.Material:
                            ImageResolverEditor.ResolveMaterial(imageResolver.transform, entry);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
        
        [MenuItem("GameObject/RoO-Toolkit/ResetAssetReferences")]
        public static void Reset()
        {
            foreach (var resolver in Selection.activeGameObject.GetComponentsInChildren<AssetResolver>())
            {
                if (resolver is not ImageResolver imageResolver)
                    continue;
                
                foreach (var entry in imageResolver.entries)
                {
                    switch (entry.assetType)
                    {
                        case ImageAssetEntry.ImageAssetType.Sprite:
                            entry.GetTarget(resolver.transform).sprite = null;
                            break;
                        case ImageAssetEntry.ImageAssetType.Material:
                            entry.GetTarget(resolver.transform).material = null;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
    }
}