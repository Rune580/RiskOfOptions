using System;
using System.Text;
using RiskOfOptions.Components.AssetResolution;
using RiskOfOptions.Components.AssetResolution.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace RoOToolkit.Editor.CustomEditors
{
    [CustomEditor(typeof(ImageResolver))]
    public class ImageResolverEditor : BaseResolverEditor<ImageResolver, ImageAssetEntry, Image>
    {
        private AssetType _assetType = AssetType.Sprite;
        
        protected override void DrawEntry(ImageResolver resolver, ImageAssetEntry entry, int index)
        {
            var lastName = entry.name;
            var lastAddressable = entry.addressablePath;
            var lastTarget = entry.targetPath;
            
            GUILayout.Label(entry.assetType.ToString());
            
            entry.name = EditorGUILayout.TextField("Name", entry.name);
            entry.addressablePath = EditorGUILayout.TextField("Addressable Path", entry.addressablePath);
            entry.targetPath = EditorGUILayout.TextField("Target Path", entry.targetPath);
            
            bool modified = lastName != entry.name || lastAddressable != entry.addressablePath || lastTarget != entry.targetPath;

            if (entry.assetType == ImageAssetEntry.ImageAssetType.Sprite)
            {
                var lastRect = entry.rect;

                entry.rect = EditorGUILayout.RectField("Rect", entry.rect);

                modified = modified || lastRect != entry.rect;
            }

            if (modified) 
            {
                Undo.RecordObject(resolver, "Serialized data modified");
                
                resolver.entries[index] = entry;
                
                PrefabUtility.RecordPrefabInstancePropertyModifications(resolver);
            }

            if (entry.assetType != ImageAssetEntry.ImageAssetType.Sprite)
                return;

            bool copyCode = GUILayout.Button("Copy sprite code");
            
            if (copyCode)
            {
                StringBuilder code = new StringBuilder();
                
                code.Append($"Assets[\"{entry.name}\"] = Sprite.Create(await LoadTextureAsync(\"{entry.addressablePath}\"), ");
                code.Append($"new Rect({entry.rect.x}, {entry.rect.y}, {entry.rect.width}, {entry.rect.height}), ");
                code.Append($"new Vector2({entry.pivot.x}, {entry.pivot.y}), ");
                code.Append($"{entry.pixelsPerUnit}, ");
                code.Append($"{entry.extrude}, ");
                code.Append($"SpriteMeshType.{entry.meshType}, ");
                code.AppendLine($"new Vector4({entry.border.x}, {entry.border.y}, {entry.border.z}, {entry.border.w}));");

                EditorGUIUtility.systemCopyBuffer = code.ToString();
            }
        }

        protected override void DrawBeforeAddElements(ImageResolver resolver)
        {
            _assetType = (AssetType)EditorGUILayout.EnumFlagsField("Asset Type", _assetType);
        }

        protected override void AddEntry(ImageResolver resolver)
        {
            if ((_assetType & AssetType.Sprite) == AssetType.Sprite)
            {
                AddSprite(resolver);
            }

            if ((_assetType & AssetType.Material) == AssetType.Material)
            {
                AddMaterial(resolver);
            }
        }

        private void AddSprite(ImageResolver resolver)
        {
            var refSprite = RefObj.sprite;
            
            var entry = new ImageAssetEntry
            {
                assetType = ImageAssetEntry.ImageAssetType.Sprite,
                rect = refSprite.rect,
                pivot = refSprite.pivot,
                border = refSprite.border,
                name = refSprite.name,
                targetPath = GetTransformPath(resolver.gameObject.transform, TargetObj),
                pixelsPerUnit = 100f,
                extrude = 0,
                meshType = SpriteMeshType.Tight,
                addressablePath = AttemptResolveAddressablePath(refSprite.name)
            };

            resolver.entries.Add(entry);
        }

        private void AddMaterial(ImageResolver resolver)
        {
            var refMat = RefObj.material;

            var entry = new ImageAssetEntry
            {
                assetType = ImageAssetEntry.ImageAssetType.Material,
                name = refMat.name,
                targetPath = GetTransformPath(resolver.gameObject.transform, TargetObj),
                addressablePath = AddressableUtils.GetAddressablePath(refMat.name)
            };
            
            resolver.entries.Add(entry);
        }

        protected override void ResolveAssets(ImageResolver resolver)
        {
            foreach (var entry in resolver.entries)
            {
                switch (entry.assetType)
                {
                    case ImageAssetEntry.ImageAssetType.Sprite:
                        ResolveSprite(resolver.transform, entry);
                        break;
                    case ImageAssetEntry.ImageAssetType.Material:
                        ResolveMaterial(resolver.transform, entry);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void ResolveSprite(Transform root, ImageAssetEntry entry)
        {
            Texture2D texture = Addressables.LoadAssetAsync<Texture2D>(entry.addressablePath).WaitForCompletion();
            
            Sprite asset = Sprite.Create(texture, entry.rect, entry.pivot, entry.pixelsPerUnit, entry.extrude, entry.meshType, entry.border);
            
            asset.name = string.IsNullOrEmpty(entry.name) ? texture.name : entry.name;
            
            entry.GetTarget(root).sprite = asset;
        }

        private void ResolveMaterial(Transform root, ImageAssetEntry entry)
        {
            Material material = Addressables.LoadAssetAsync<Material>(entry.addressablePath).WaitForCompletion();

            entry.GetTarget(root).material = material;
        }

        protected override void ResetAssets(ImageResolver resolver)
        {
            foreach (var entry in resolver.entries)
            {
                switch (entry.assetType)
                {
                    case ImageAssetEntry.ImageAssetType.Sprite:
                        entry.GetTarget(resolver.transform).sprite = null;
                        break;
                    case ImageAssetEntry.ImageAssetType.Material:
                        entry.GetTarget(resolver.transform).material = null;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private string AttemptResolveAddressablePath(string spriteName)
        {
            int indexOf = spriteName.IndexOf('_');
            if (indexOf > -1)
                spriteName = spriteName.Remove(indexOf);

            return AddressableUtils.GetAddressablePath(spriteName);
        }
        
        [Flags]
        private enum AssetType : byte
        {
            None = 0,
            Sprite = 1,
            Material = 2,
        }
    }
}