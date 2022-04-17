using System;
using System.Collections.Generic;
using RiskOfOptions.Components.AssetResolution.Data;
using RoR2.UI;
using RoR2.UI.SkinControllers;
using UnityEngine;
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


        protected override void BeforeSerialize()
        {
            var buffer = new UnityByteBufWriter();
            
            buffer.WriteInt(entries.Count);

            foreach (var entry in entries)
            {
                var subBuf = new UnityByteBufWriter();
                
                subBuf.WriteString(entry.addressablePath);
                subBuf.WriteString(entry.targetPath);
                
                buffer.WriteBytes(subBuf.GetBytes());
            }

            serializedData = buffer.GetBytes();
        }

        protected override void AfterDeserialize()
        {
            var buffer = new UnityByteBufReader(serializedData);
            var length = buffer.ReadInt();

            entries = new List<SkinControllerAssetEntry>();

            for (int i = 0; i < length; i++)
            {
                var subBuf = new UnityByteBufReader(buffer.ReadByteArray());

                var entry = new SkinControllerAssetEntry
                {
                    addressablePath = subBuf.ReadString(),
                    targetPath = subBuf.ReadString()
                };
                
                entries.Add(entry);
            }
        }
    }
}