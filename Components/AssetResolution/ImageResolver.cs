using System;
using System.Collections.Generic;
using System.Text;
using RiskOfOptions.Components.AssetResolution.Data;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace RiskOfOptions.Components.AssetResolution
{
    public class ImageResolver : MultiAssetResolver<ImageAssetEntry>
    {
        protected override void Resolve()
        {
            base.Resolve();
            
            foreach (var entry in entries)
            {
                Texture2D texture = Addressables.LoadAssetAsync<Texture2D>(entry.addressablePath).WaitForCompletion();
                
                Sprite asset = Sprite.Create(texture, entry.rect, entry.pivot, entry.pixelsPerUnit, entry.extrude, entry.meshType, entry.border);

                asset.name = string.IsNullOrEmpty(entry.name) ? texture.name : entry.name;

                entry.GetTarget(transform).sprite = asset;
            }
        }
    }
}