using System;
using UnityEngine;
using UnityEngine.UI;

namespace RiskOfOptions.Components.AssetResolution.Data
{
    [Serializable]
    public class ImageAssetEntry : BaseAssetEntry<Image>
    {
        public string name;
        public Rect rect;
        public Vector2 pivot;
        public float pixelsPerUnit;
        public uint extrude;
        public SpriteMeshType meshType;
        public Vector4 border;
    }
}