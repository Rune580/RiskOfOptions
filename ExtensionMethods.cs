using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using R2API;
using R2API.Utils;
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
                    Debug.Log($"Found Container for {option.Name}, under {option.ModGuid}", BepInEx.Logging.LogLevel.Debug);
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
                        Debug.Log($"Did not find a matching Category for {mo.Name}, creating a new one under {mo.CategoryName} \n This category will have no description, please create a category before assigning to it!", BepInEx.Logging.LogLevel.Warning);

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
                Debug.Log($"Did not find a matching Category for {mo.Name}, creating a new one under {mo.CategoryName} \n This category will have no description, please create a category before assigning to it!", BepInEx.Logging.LogLevel.Warning);

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


                Debug.Log($"Container not found for {modGuid}, Creating one.", BepInEx.Logging.LogLevel.Debug);
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
                // Not in containers yet.
            }

            return indexes;
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
