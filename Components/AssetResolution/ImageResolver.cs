using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace RiskOfOptions.Components.AssetResolution
{
    public class ImageResolver : AssetResolver
    {
        public List<Entry> entries = new();
        
        protected override void Resolve()
        {
            foreach (var entry in entries)
            {
                Texture2D texture = Addressables.LoadAssetAsync<Texture2D>(entry.addressablePath).WaitForCompletion();
                
                Sprite asset = Sprite.Create(texture, entry.rect, entry.pivot, entry.pixelsPerUnit, entry.extrude, entry.meshType, entry.border);

                asset.name = string.IsNullOrEmpty(entry.name) ? texture.name : entry.name;

                entry.target.sprite = asset;
            }
        }

        [Serializable]
        public class Entry
        {
            public string addressablePath;
            public string name;
            public Image target;
            public Rect rect;
            public Vector2 pivot;
            public float pixelsPerUnit = 100f;
            public uint extrude = 0;
            public SpriteMeshType meshType = SpriteMeshType.Tight;
            public Vector4 border;
        }
    }
}