using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RiskOfOptions.Components.AssetResolution.Data
{
    /// <summary>
    /// Helper class for writing common value types into a byte buffer
    /// </summary>
    public class UnityByteBufWriter : UnityByteBuf
    {
        private readonly List<byte> _buffer;

        public UnityByteBufWriter()
        {
            _buffer = new List<byte>();
        }

        /// <summary>
        /// Writes a byte array and encodes the length of said array as the first 4 bytes.
        /// </summary>
        public void WriteBytes(byte[] bytes)
        {
            var lengthBytes = BitConverter.GetBytes((uint)bytes.Length);
            
            _buffer.AddRange(lengthBytes);
            _buffer.AddRange(bytes);
            
            UpdateByteArray();
        }

        public void WriteString(string text)
        {
            text ??= "";

            WriteBytes(Encoding.UTF8.GetBytes(text));
        }

        public void WriteByte(byte num)
        {
            _buffer.Add(num);
            
            UpdateByteArray();
        }

        public void WriteUInt(uint num)
        {
            _buffer.AddRange(BitConverter.GetBytes(num));
            
            UpdateByteArray();
        }

        public void WriteInt(int num)
        {
            _buffer.AddRange(BitConverter.GetBytes(num));
            
            UpdateByteArray();
        }

        public void WriteFloat(float num)
        {
            _buffer.AddRange(BitConverter.GetBytes(num));
            
            UpdateByteArray();
        }

        public void WriteDouble(double num)
        {
            _buffer.AddRange(BitConverter.GetBytes(num));
            
            UpdateByteArray();
        }

        public void WriteRect(Rect rect)
        {
            WriteFloats(rect.x, rect.y, rect.width, rect.height);
        }

        public void WriteVector2(Vector2 vector2)
        {
            WriteFloats(vector2.x, vector2.y);
        }

        public void WriteVector3(Vector3 vector3)
        {
            WriteFloats(vector3.x, vector3.y, vector3.z);
        }

        public void WriteVector4(Vector4 vector4)
        {
            WriteFloats(vector4.x, vector4.y, vector4.z, vector4.w);
        }

        public void WriteEnum<T>(object value)
        {
            WriteInt((int)Enum.Parse(typeof(T), value.ToString()));
        }

        public void WriteFloats(params float[] nums)
        {
            foreach (var num in nums)
            {
                WriteFloat(num);
            }
        }

        public void UpdateByteArray()
        {
            byteBuffer = _buffer.ToArray();
        }
    }
}