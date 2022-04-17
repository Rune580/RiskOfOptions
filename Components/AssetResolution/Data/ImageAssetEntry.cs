using System;
using UnityEngine;
using UnityEngine.UI;

namespace RiskOfOptions.Components.AssetResolution.Data
{
    [Serializable]
    public class ImageAssetEntry
    {
        public string addressablePath;
        public string name;
        public string targetPath;
        public Rect rect;
        public Vector2 pivot;
        public float pixelsPerUnit;
        public uint extrude;
        public SpriteMeshType meshType;
        public Vector4 border;

        public Image GetTarget(Transform root)
        {
            if (string.IsNullOrEmpty(targetPath))
                return root.GetComponent<Image>();
            
            return root.transform.Find(targetPath).gameObject.GetComponent<Image>();
        }
    }
}