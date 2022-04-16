using System;
using System.Collections.Generic;

namespace RiskOfOptions.Components.AssetResolution.Data
{
    internal abstract class UnityByteBuf
    {
        protected const byte UnsignedByte = 0x00;
        protected const byte UnsignedShort = 0x01;
        protected const byte UnsignedInt = 0x02;

        protected const uint UnsignedByteLength = 255;
        protected const uint UnsignedShortLength = 65535;
        
        protected byte[] byteBuffer;
        
        internal int Length => byteBuffer.Length;

        protected byte GetByteTagForLength(uint length)
        {
            switch (length)
            {
                case < UnsignedByteLength:
                    return UnsignedByte;
                case < UnsignedShortLength:
                    return UnsignedShort;
            }

            return UnsignedInt;
        }

        internal byte[] GetBytes()
        {
            return byteBuffer;
        }
    }
}