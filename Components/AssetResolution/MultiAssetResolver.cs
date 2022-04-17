using System;
using System.Collections.Generic;
using UnityEngine;

namespace RiskOfOptions.Components.AssetResolution
{
    [Serializable]
    public abstract class MultiAssetResolver<TEntry> : AssetResolver, ISerializationCallbackReceiver
    {
        [HideInInspector]
        public List<TEntry> entries;

        protected override void Resolve()
        {
            if (entries is null)
                throw new NullReferenceException("Attempted to resolve null entries!");
        }
        
        public void OnBeforeSerialize()
        {
            if (entries is null)
            {
                serializedData = Array.Empty<byte>();
                return;
            }
            
            BeforeSerialize();
        }

        public void OnAfterDeserialize()
        {
            if (serializedData is null || serializedData.Length == 0)
                return;
            
            AfterDeserialize();
        }
        
        protected abstract void BeforeSerialize();
        protected abstract void AfterDeserialize();
    }
}