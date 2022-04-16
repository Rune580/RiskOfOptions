using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RiskOfOptions.Components.AssetResolution.Data
{
    internal class UnityByteBufWriter : UnityByteBuf
    {
        private readonly List<byte> _buffer;

        internal UnityByteBufWriter()
        {
            _buffer = new List<byte>();
        }

        /// <summary>
        /// Writes an unknown length array of bytes
        /// </summary>
        internal void WriteBytes(byte[] bytes)
        {
            var lengthBytes = BitConverter.GetBytes((uint)bytes.Length);
            
            _buffer.AddRange(lengthBytes);
            _buffer.AddRange(bytes);
            
            UpdateByteArray();
        }

        internal void WriteString(string text)
        {
            text ??= "";

            WriteBytes(Encoding.UTF8.GetBytes(text));
        }

        internal void WriteUInt(uint num)
        {
            _buffer.AddRange(BitConverter.GetBytes(num));
            
            UpdateByteArray();
        }

        internal void WriteInt(int num)
        {
            _buffer.AddRange(BitConverter.GetBytes(num));
            
            UpdateByteArray();
        }

        internal void WriteFloat(float num)
        {
            _buffer.AddRange(BitConverter.GetBytes(num));
            
            UpdateByteArray();
        }

        internal void WriteDouble(double num)
        {
            _buffer.AddRange(BitConverter.GetBytes(num));
            
            UpdateByteArray();
        }

        internal void WriteRect(Rect rect)
        {
            WriteFloats(rect.x, rect.y, rect.width, rect.height);
        }

        internal void WriteVector2(Vector2 vector2)
        {
            WriteFloats(vector2.x, vector2.y);
        }

        internal void WriteVector3(Vector3 vector3)
        {
            WriteFloats(vector3.x, vector3.y, vector3.z);
        }

        internal void WriteVector4(Vector4 vector4)
        {
            WriteFloats(vector4.x, vector4.y, vector4.z, vector4.w);
        }

        internal void WriteEnum<T>(object value)
        {
            WriteInt((int)Enum.Parse(typeof(T), value.ToString()));
        }

        internal void WriteComponentReference<T>(Transform root, T comp) where T : Component
        {
            if (comp is null)
            {
                WriteString("");
                return;
            }
            
            var transform = comp.gameObject.transform;

            string path = transform.name;

            while (transform.name != root.name)
            {
                transform = transform.parent;

                path = $"{transform.name}/{path}";
            }
            
            WriteString(path);
        }

        private void WriteFloats(params float[] nums)
        {
            foreach (var num in nums)
            {
                WriteFloat(num);
            }
        }

        private void UpdateByteArray()
        {
            byteBuffer = _buffer.ToArray();
        }
    }
}