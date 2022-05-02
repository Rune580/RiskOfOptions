using System;
using UnityEngine;
using UnityEngine.UI;

namespace RiskOfOptions.Components.AssetResolution.Data
{
    [Serializable]
    public class ImageAssetEntry : BaseAssetEntry<Image>
    {
        public string name;
        public Rect rect;
        public Vector2 pivot;
        public float pixelsPerUnit;
        public uint extrude;
        public SpriteMeshType meshType;
        public Vector4 border;

        protected override UnityByteBufWriter SerializeInternal()
        {
            var writer = new UnityByteBufWriter();
            
            writer.WriteString(addressablePath);
            writer.WriteString(name);
            writer.WriteString(targetPath);
            writer.WriteRect(rect);
            writer.WriteVector2(pivot);
            writer.WriteFloat(pixelsPerUnit);
            writer.WriteUInt(extrude);
            writer.WriteEnum<SpriteMeshType>(meshType);
            writer.WriteVector4(border);
            
            return writer;
        }

        protected override void DeserializeInternal(UnityByteBufReader reader)
        {
            addressablePath = reader.ReadString();
            name = reader.ReadString();
            targetPath = reader.ReadString();
            rect = reader.ReadRect();
            pivot = reader.ReadVector2();
            pixelsPerUnit = reader.ReadFloat();
            extrude = reader.ReadUInt();
            meshType = reader.ReadEnum<SpriteMeshType>();
            border = reader.ReadVector4();
        }
    }
}