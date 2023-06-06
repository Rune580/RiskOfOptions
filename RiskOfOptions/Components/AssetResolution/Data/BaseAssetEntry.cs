using System;
using UnityEngine;

namespace RiskOfOptions.Components.AssetResolution.Data
{
    [Serializable]
    public abstract class BaseAssetEntry<TTarget> : ISerializableEntry where TTarget : Component
    {
        public string addressablePath;
        public string targetPath;

        protected virtual UnityByteBufWriter SerializeInternal()
        {
            var writer = new UnityByteBufWriter();
            
            writer.WriteString(addressablePath);
            writer.WriteString(targetPath);

            return writer;
        }

        protected virtual void DeserializeInternal(UnityByteBufReader reader)
        {
            addressablePath = reader.ReadString();
            targetPath = reader.ReadString();
        }

        public TTarget GetTarget(Transform root)
        {
            if (string.IsNullOrEmpty(targetPath))
                return root.GetComponent<TTarget>();
            
            return root.transform.Find(targetPath).gameObject.GetComponent<TTarget>();
        }

        public byte[] Serialize()
        {
            var writer = SerializeInternal();

            return writer.GetBytes();
        }

        public void Deserialize(byte[] bytes)
        {
            var reader = new UnityByteBufReader(bytes);
            
            DeserializeInternal(reader);
        }
    }
}