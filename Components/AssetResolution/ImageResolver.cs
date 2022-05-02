using System;
using System.Collections.Generic;
using System.Text;
using RiskOfOptions.Components.AssetResolution.Data;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace RiskOfOptions.Components.AssetResolution
{
    public class ImageResolver : MultiAssetResolver<ImageAssetEntry>
    {
        protected override void Resolve()
        {
            base.Resolve();
            
            foreach (var entry in entries)
            {
                Texture2D texture = Addressables.LoadAssetAsync<Texture2D>(entry.addressablePath).WaitForCompletion();
                
                Sprite asset = Sprite.Create(texture, entry.rect, entry.pivot, entry.pixelsPerUnit, entry.extrude, entry.meshType, entry.border);

                asset.name = string.IsNullOrEmpty(entry.name) ? texture.name : entry.name;

                entry.GetTarget(transform).sprite = asset;
            }
        }

        private void PrintData()
        {
            if (serializedData is null || serializedData.Length == 0)
                return;

            var reader = new UnityByteBufReader(serializedData);
            int length = reader.ReadInt();

            var writer = new UnityByteBufWriter();
            writer.WriteInt(length);

            entries = new List<ImageAssetEntry>();

            for (int i = 0; i < length; i++)
            {
                var subRead = new UnityByteBufReader(reader.ReadByteArray());
                
                var entry = new ImageAssetEntry
                {
                    addressablePath = subRead.ReadString(),
                    name = subRead.ReadString(),
                    targetPath = subRead.ReadString(),
                    rect = subRead.ReadRect(),
                    pivot = subRead.ReadVector2(),
                    pixelsPerUnit = subRead.ReadFloat(),
                    extrude = subRead.ReadUInt(),
                    meshType = subRead.ReadEnum<SpriteMeshType>(),
                    border = subRead.ReadVector4()
                };

                var data = entry.Serialize();

                writer.WriteBytes(data);
            }
            
            var bytes = writer.GetBytes();
            StringBuilder builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append($"{b}");
            }

            Debug.Log($"Prefab: {gameObject.name}\n" +
                      $"Data: '{builder}'");
        }
    }
}