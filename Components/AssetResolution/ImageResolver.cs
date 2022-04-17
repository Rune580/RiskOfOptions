using System;
using System.Collections.Generic;
using RiskOfOptions.Components.AssetResolution.Data;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace RiskOfOptions.Components.AssetResolution
{
    public class ImageResolver : AssetResolver, ISerializationCallbackReceiver
    {
        [HideInInspector]
        public List<ImageAssetEntry> entries;
        
        
        protected override void Resolve()
        {
            if (entries == null)
                throw new NullReferenceException("Attempted to resolve null entries!");
            
            foreach (var entry in entries)
            {
                Texture2D texture = Addressables.LoadAssetAsync<Texture2D>(entry.addressablePath).WaitForCompletion();
                
                Sprite asset = Sprite.Create(texture, entry.rect, entry.pivot, entry.pixelsPerUnit, entry.extrude, entry.meshType, entry.border);

                asset.name = string.IsNullOrEmpty(entry.name) ? texture.name : entry.name;

                entry.GetTarget(transform).sprite = asset;
            }
        }

        public void OnBeforeSerialize()
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

        public void OnAfterDeserialize()
        {
            if (serializedData is null)
                return;
            
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