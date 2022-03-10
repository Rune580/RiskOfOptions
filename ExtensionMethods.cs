using System;
using System.Reflection;
using BepInEx;

using UnityEngine;

namespace RiskOfOptions
{
    internal static class ExtensionMethods
    {
        internal static ModInfo GetModInfo(this Assembly assembly)
        {
            ModInfo modInfo = default;
            
            Type[] types = assembly.GetExportedTypes();
            
            foreach (var item in types)
            {
                BepInPlugin bepInPlugin = item.GetCustomAttribute<BepInPlugin>();

                if (bepInPlugin == null) continue;

                modInfo.ModGuid = bepInPlugin.GUID;
                modInfo.ModName = bepInPlugin.Name;
            }

            return modInfo;
        }

        internal static float Remap(this float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            var fromAbs = value - fromMin;
            var fromMaxAbs = fromMax - fromMin;

            var normal = fromAbs / fromMaxAbs;

            var toMaxAbs = toMax - toMin;
            var toAbs = toMaxAbs * normal;

            var to = toAbs + toMin;

            return Mathf.Clamp(to, toMin, toMax);
        }

        // https://answers.unity.com/questions/530178/how-to-get-a-component-from-an-object-and-add-it-t.html - thanks
        internal static T GetCopyOf<T>(this Component comp, T other) where T : Component
        {
            Type type = comp.GetType();
            if (type != other.GetType()) return null; // type mis-match
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default;
            PropertyInfo[] pinfos = type.GetProperties(flags);
            foreach (var pinfo in pinfos)
            {
                if (pinfo.CanWrite)
                {
                    try
                    {
                        pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                    }
                    catch
                    {
                        // In case of NotImplementedException being thrown.
                    }
                }
            }
            FieldInfo[] finfos = type.GetFields(flags);
            foreach (var finfo in finfos)
            {
                finfo.SetValue(comp, finfo.GetValue(other));
            }
            return comp as T;
        }

        // https://answers.unity.com/questions/530178/how-to-get-a-component-from-an-object-and-add-it-t.html - thanks
        internal static T AddComponent<T>(this GameObject go, T toAdd) where T : Component
        {
            return go.AddComponent<T>().GetCopyOf(toAdd) as T;
        }

        internal static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();

            if (!component)
                component = gameObject.AddComponent<T>();

            return component;
        }

        internal static bool CloseEnough(Vector2 a, Vector2 b)
        {
            return Mathf.Abs(a.x - b.x) < 0.0001f && Mathf.Abs(a.y - b.y) < 0.0001f;
        }

        internal static bool CloseEnough(Color a, Color b)
        {
            return Mathf.Abs(a.r - b.r) < 0.0001f && Mathf.Abs(a.g - b.g) < 0.0001f && Mathf.Abs(a.b - b.b) < 0.0001f && Mathf.Abs(a.a - b.a) < 0.0001f;
        }

        internal static bool CloseEnough(Color[] a, Color b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == null)
                    continue;
                
                if (!CloseEnough(a[i], b))
                {
                    return false;
                }
            }

            return true;
        }

        internal static Vector2 SmoothStep(Vector2 a, Vector2 b, float t)
        {
            Vector2 c = Vector2.zero;

            c.x = Mathf.SmoothStep(a.x, b.x, t);
            c.y = Mathf.SmoothStep(a.y, b.y, t);

            return c;
        }

        internal static double Abs(double num)
        {
            return num < 0 ? -1 * num : num;
        }

        internal static double RoundUpToDecimalPlace(this double num, int place)
        {
            var pow = Mathf.Pow(10, place);

            pow = pow == 0 ? 1 : pow;
            return Mathf.Ceil((float) num * pow) / pow;
        }

        internal struct ModInfo
        {
            internal string ModGuid;
            internal string ModName;
        }
    }
}
