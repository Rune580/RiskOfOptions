using System;
using System.Collections.Generic;

namespace RiskOfOptions.Components.AssetResolution.Data
{
    public abstract class UnityByteBuf
    {
        protected byte[] byteBuffer;
        
        
        /// <summary>
        /// The current length of the byte buffer
        /// </summary>
        internal int Length => byteBuffer.Length;
        
        /// <returns>The byte array stored internally by this byte buffer</returns>
        internal byte[] GetBytes()
        {
            return byteBuffer;
        }
    }
}