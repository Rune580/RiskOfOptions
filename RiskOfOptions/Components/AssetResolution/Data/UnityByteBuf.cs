namespace RiskOfOptions.Components.AssetResolution.Data
{
    public abstract class UnityByteBuf
    {
        protected byte[] byteBuffer;
        
        
        /// <summary>
        /// The current length of the byte buffer
        /// </summary>
        public int Length => byteBuffer.Length;
        
        /// <returns>The byte array stored internally by this byte buffer</returns>
        public byte[] GetBytes()
        {
            return byteBuffer;
        }
    }
}