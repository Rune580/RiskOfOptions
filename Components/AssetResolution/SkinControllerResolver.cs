using System;
using System.Collections.Generic;
using RoR2.UI;
using RoR2.UI.SkinControllers;
using UnityEngine.AddressableAssets;

namespace RiskOfOptions.Components.AssetResolution
{
    public class SkinControllerResolver : AssetResolver
    {
        public List<Entry> entries = new(); 
        
        protected override void Resolve()
        {
            foreach (var entry in entries)
            {
                entry.target.skinData = Addressables.LoadAssetAsync<UISkinData>(entry.addressablePath).WaitForCompletion();
            }
        }

        [Serializable]
        public class Entry
        {
            public string addressablePath;
            public BaseSkinController target;
        }
    }
}