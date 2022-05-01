namespace RiskOfOptions.Components.AssetResolution.Data
{
    public interface ISerializableEntry
    {
        byte[] Serialize();

        void Deserialize(byte[] bytes);
    }
}