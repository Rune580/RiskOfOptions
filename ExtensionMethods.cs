using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using R2API;
using R2API.Utils;
using RiskOfOptions.Containers;
using RiskOfOptions.Options;
using RoR2.ConVar;
using RoR2.UI;
using RoR2.UI.SkinControllers;
using UnityEngine;

namespace RiskOfOptions
{
    internal static class ExtensionMethods
    {
        internal static void Add(this List<OptionContainer> containers, ref OptionBase option)
        {
            foreach (var container in containers)
            {
                if (container.ModGuid == option.ModGuid)
                {
                    Debug.Log($"Found Container for {option.Name}, under {option.ModGuid}", LogLevel.Debug);
                    container.Add(ref option);
                    return;
                }
            }

            Debug.Log($"Did not find Container for {option.Name}, creating a new one under {option.ModGuid}");

            OptionContainer newContainer = new OptionContainer(option.ModGuid, option.ModName);

            newContainer.Add(ref option);

            containers.Add(newContainer);
        }

        internal static void Add(this List<OptionContainer> containers, ref RiskOfOption mo)
        {
            foreach (var container in containers)
            {
                if (container.ModGuid != mo.ModGuid)
                    continue;

                Debug.Log($"Found Container for {mo.Name}, under {mo.ModGuid}");

                if (mo.CategoryName != "")
                {
                    bool foundCategory = false;

                    for (int x = 0; x < container.GetCategoriesCached().Count; x++)
                    {
                        if (container.GetCategoriesCached()[x].Name == mo.CategoryName)
                        {
                            Debug.Log($"Found Category for {mo.Name}, under {mo.CategoryName}");

                            foundCategory = true;
                            container.GetCategoriesCached()[x].Add(ref mo);
                        }
                            
                    }

                    if (!foundCategory)
                    {
                        Debug.Log($"Did not find a matching Category for {mo.Name}, creating a new one under {mo.CategoryName} \n This category will have no description, please create a category before assigning to it!", LogLevel.Warning);

                        OptionCategory newCategory = new OptionCategory(mo.CategoryName, mo.ModGuid);

                        container.Add(ref newCategory);

                        for (int x = 0; x < container.GetCategoriesCached().Count; x++)
                        {
                            if (container.GetCategoriesCached()[x].Name == mo.CategoryName)
                            {

                                container.GetCategoriesCached()[x].Add(ref mo);
                            }
                        }
                    }
                }

                container.Add(ref mo);
                return;
            }

            Debug.Log($"Did not find Container for {mo.Name}, creating a new one under {mo.ModGuid}");

            OptionContainer newContainer = new OptionContainer(mo.ModGuid, mo.ModName);

            newContainer.Add(ref mo);

            if (mo.CategoryName != "")
            {
                Debug.Log($"Did not find a matching Category for {mo.Name}, creating a new one under {mo.CategoryName} \n This category will have no description, please create a category before assigning to it!", LogLevel.Warning);

                OptionCategory newCategory = new OptionCategory(mo.CategoryName, mo.ModGuid);

                newCategory.Add(ref mo);

                newContainer.Add(ref newCategory);
            }

            containers.Add(newContainer);
        }


        internal static bool Contains(this List<OptionCategory> categories, string categoryName)
        {
            return categories.Any(category => category.Name == categoryName);
        }

        internal static bool Contains(this List<OptionContainer> containers, string modGuid)
        {
            return containers.Any(container => container.ModGuid == modGuid);
        }

        internal static int GetContainerIndex(this List<OptionContainer> containers, string modGuid)
        {
            for (int i = 0; i < containers.Count; i++)
            {
                if (containers[i].ModGuid == modGuid)
                {
                    return i;
                }
            }

            throw new Exception($"No container exists for this ModGUID: {modGuid} !");
        }

        internal static int GetOptionIndex(this List<RiskOfOption> options, string name, string categoryName)
        {
            for (int i = 0; i < options.Count; i++)
            {
                if (string.Equals(options[i].Name, name, StringComparison.InvariantCultureIgnoreCase) && string.Equals(options[i].CategoryName, categoryName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return i;
                }
            }

            throw new Exception($"ROO {name}, not found in category!");
        }

        internal static int GetContainerIndex(this List<OptionContainer> containers, string modGuid, string modName, bool generateIfNotFound = false)
        {
            while (true)
            {
                for (int i = 0; i < containers.Count; i++)
                {
                    if (containers[i].ModGuid == modGuid)
                    {
                        return i;
                    }
                }

                if (!generateIfNotFound)
                    throw new Exception($"No container exists for this ModGUID: {modGuid} !");


                Debug.Log($"Container not found for {modGuid}, Creating one.", LogLevel.Debug);
                containers.Add(new OptionContainer(modGuid, modName));

                generateIfNotFound = false;
            }
        }

        internal static ModInfo GetModInfo(this Type[] types)
        {
            ModInfo modInfo = default;

            foreach (var item in types)
            {
                BepInPlugin bepInPlugin = item.GetCustomAttribute<BepInPlugin>();

                if (bepInPlugin == null) continue;

                modInfo.ModGuid = bepInPlugin.GUID;
                modInfo.ModName = bepInPlugin.Name;
            }

            return modInfo;
        }

        internal static Indexes GetIndexes(this List<OptionContainer> containers, string modGuid, string optionName, string categoryName = "Main")
        {
            Indexes indexes = new Indexes()
            {
                ContainerIndex = -1,
                OptionIndexInContainer = -1
            };

            try
            {
                indexes.ContainerIndex = containers.GetContainerIndex(modGuid);
                indexes.OptionIndexInContainer = containers[indexes.ContainerIndex].GetModOptionsCached().GetOptionIndex(optionName, categoryName);
            }
            catch
            {
                // Not in containers;
            }

            return indexes;
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
            return Mathf.Abs(a.x - b.x) < 0.00001f && Mathf.Abs(a.y - b.y) < 0.00001f;
        }

        internal static bool CloseEnough(Color a, Color b)
        {
            return Mathf.Abs(a.r - b.r) < 0.0001f && Mathf.Abs(a.g - b.g) < 0.0001f && Mathf.Abs(a.b - b.b) < 0.0001f && Mathf.Abs(a.a - b.a) < 0.0001f;
        }

        internal static Vector2 SmoothStep(Vector2 a, Vector2 b, float t)
        {
            Vector2 c = Vector2.zero;

            c.x = Mathf.SmoothStep(a.x, b.x, t);
            c.y = Mathf.SmoothStep(a.y, b.y, t);

            return c;
        }

        internal struct ModInfo
        {
            internal string ModGuid;
            internal string ModName;
        }

        internal struct Indexes
        {
            internal int ContainerIndex;
            internal int OptionIndexInContainer;
        }
    }
}
