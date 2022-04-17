using System;
using UnityEngine;

namespace RiskOfOptions.Components.AssetResolution.Data
{
    [Serializable]
    public abstract class BaseAssetEntry<TTarget> where TTarget : Component
    {
        public string addressablePath;
        public string targetPath;
        
        public TTarget GetTarget(Transform root)
        {
            if (string.IsNullOrEmpty(targetPath))
                return root.GetComponent<TTarget>();
            
            return root.transform.Find(targetPath).gameObject.GetComponent<TTarget>();
        }
    }
}