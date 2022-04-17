using System;
using System.Collections.Generic;
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

        protected override void BeforeSerialize()
        {
            var buffer = new UnityByteBufWriter();
            
            buffer.WriteInt(entries.Count);

            foreach (var entry in entries)
            {
                var subBuf = new UnityByteBufWriter();

                subBuf.WriteString(entry.addressablePath);
                subBuf.WriteString(entry.name);
                subBuf.WriteString(entry.targetPath);
                subBuf.WriteRect(entry.rect);
                subBuf.WriteVector2(entry.pivot);
                subBuf.WriteFloat(entry.pixelsPerUnit);
                subBuf.WriteUInt(entry.extrude);
                subBuf.WriteEnum<SpriteMeshType>(entry.meshType);
                subBuf.WriteVector4(entry.border);
                
                buffer.WriteBytes(subBuf.GetBytes());
            }

            serializedData = buffer.GetBytes();
        }

        protected override void AfterDeserialize()
        { 
            var buffer = new UnityByteBufReader(serializedData);
            var length = buffer.ReadInt();

            entries = new List<ImageAssetEntry>();

            for (int i = 0; i < length; i++)
            {
                var subBuf = new UnityByteBufReader(buffer.ReadByteArray());

                var entry = new ImageAssetEntry
                {
                    addressablePath = subBuf.ReadString(),
                    name = subBuf.ReadString(),
                    targetPath = subBuf.ReadString(),
                    rect = subBuf.ReadRect(),
                    pivot = subBuf.ReadVector2(),
                    pixelsPerUnit = subBuf.ReadFloat(),
                    extrude = subBuf.ReadUInt(),
                    meshType = subBuf.ReadEnum<SpriteMeshType>(),
                    border = subBuf.ReadVector4()
                };
                
                entries.Add(entry);
            }
        }
    }
}