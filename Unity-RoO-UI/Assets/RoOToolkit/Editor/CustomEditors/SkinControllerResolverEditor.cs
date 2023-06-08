using RiskOfOptions.Components.AssetResolution;
using RiskOfOptions.Components.AssetResolution.Data;
using RoR2.UI;
using RoR2.UI.SkinControllers;
using UnityEditor;
using UnityEngine.AddressableAssets;

namespace RoOToolkit.Editor.CustomEditors
{
    [CustomEditor(typeof(SkinControllerResolver))]
    public class SkinControllerResolverEditor : BaseResolverEditor<SkinControllerResolver, SkinControllerAssetEntry, BaseSkinController>
    {
        protected override void DrawEntry(SkinControllerResolver resolver, SkinControllerAssetEntry entry, int index)
        {
            entry.addressablePath = EditorGUILayout.TextField("Addressable Path", entry.addressablePath);
            entry.targetPath = EditorGUILayout.TextField("Target Path", entry.targetPath);
            
            resolver.entries[index] = entry;
        }

        protected override void AddEntry(SkinControllerResolver resolver)
        {
            var entry = new SkinControllerAssetEntry
            {
                addressablePath = AddressableUtils.GetAddressablePath(RefObj.skinData.name),
                targetPath = GetTransformPath(resolver.gameObject.transform, TargetObj)
            };
            
            resolver.entries.Add(entry);
        }

        protected override void ResolveAssets(SkinControllerResolver resolver)
        {
            foreach (var entry in resolver.entries)
            {
                UISkinData skinData = Addressables.LoadAssetAsync<UISkinData>(entry.addressablePath).WaitForCompletion();

                entry.GetTarget(resolver.transform).skinData = skinData;
            }
        }

        protected override void ResetAssets(SkinControllerResolver resolver)
        {
            foreach (var entry in resolver.entries)
                entry.GetTarget(resolver.transform).skinData = null;
        }
    }
}