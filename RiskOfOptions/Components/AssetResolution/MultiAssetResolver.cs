using System;
using System.Collections.Generic;
using RiskOfOptions.Components.AssetResolution.Data;
using UnityEngine;

namespace RiskOfOptions.Components.AssetResolution
{
    public abstract class MultiAssetResolver<TEntry> : AssetResolver, ISerializationCallbackReceiver 
        where TEntry : ISerializableEntry, new()
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

            var buffer = new UnityByteBufWriter();
            
            buffer.WriteInt(entries.Count);

            foreach (var entry in entries)
            {
                buffer.WriteBytes(entry.Serialize());
            }

            serializedData = buffer.GetBytes();
        }

        public void OnAfterDeserialize()
        {
            if (serializedData is null || serializedData.Length == 0)
                return;

            var buffer = new UnityByteBufReader(serializedData);
            int length = buffer.ReadInt();

            entries = new List<TEntry>();

            for (int i = 0; i < length; i++)
            {
                var entry = new TEntry();
                
                entry.Deserialize(buffer.ReadByteArray());
                
                entries.Add(entry);
            }
        }
    }
}