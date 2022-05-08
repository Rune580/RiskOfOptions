using System;
using RiskOfOptions.Components.AssetResolution.Data;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RiskOfOptions.Components.AssetResolution
{
    public class ImageResolver : MultiAssetResolver<ImageAssetEntry>
    {
        protected override void Resolve()
        {
            base.Resolve();
            
            foreach (var entry in entries)
            {
                switch (entry.assetType)
                {
                    case ImageAssetEntry.ImageAssetType.Sprite:
                        ResolveSprite(entry);
                        break;
                    case ImageAssetEntry.ImageAssetType.Material:
                        ResolveMaterial(entry);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void ResolveSprite(ImageAssetEntry entry)
        {
            Texture2D texture = Addressables.LoadAssetAsync<Texture2D>(entry.addressablePath).WaitForCompletion();
                
            Sprite asset = Sprite.Create(texture, entry.rect, entry.pivot, entry.pixelsPerUnit, entry.extrude, entry.meshType, entry.border);

            asset.name = string.IsNullOrEmpty(entry.name) ? texture.name : entry.name;

            entry.GetTarget(transform).sprite = asset;
        }

        private void ResolveMaterial(ImageAssetEntry entry)
        {
            Material material = Addressables.LoadAssetAsync<Material>(entry.addressablePath).WaitForCompletion();

            entry.GetTarget(transform).material = material;
        }
    }
}