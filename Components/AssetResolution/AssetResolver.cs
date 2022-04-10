using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace RiskOfOptions.Components.AssetResolution
{
    public abstract class AssetResolver : MonoBehaviour
    {
        public void Awake()
        {
            AttemptResolve();
        }

        public void Start()
        {
            AttemptResolve();
        }

        public void OnEnable()
        {
            AttemptResolve();
        }

        private void AttemptResolve()
        {
            Resolve();
            
            DestroyImmediate(this);
        }

        protected abstract void Resolve();
    }
}