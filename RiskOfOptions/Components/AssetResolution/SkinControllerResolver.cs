using RiskOfOptions.Components.AssetResolution.Data;
using RoR2.UI;
using UnityEngine.AddressableAssets;

namespace RiskOfOptions.Components.AssetResolution
{
    public class SkinControllerResolver : MultiAssetResolver<SkinControllerAssetEntry>
    {
        // [HideInInspector]
        // public List<SkinControllerAssetEntry> entries; 
        
        protected override void Resolve()
        {
            base.Resolve();
            
            foreach (var entry in entries)
            {
                entry.GetTarget(transform).skinData = Addressables.LoadAssetAsync<UISkinData>(entry.addressablePath).WaitForCompletion();
            }
        }
    }
}