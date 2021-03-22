using System;
using System.Collections.Generic;
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
            for (int i = 0; i < containers.Count; i++)
            {
                if (containers[i].ModGUID == option.ModGUID)
                {
                    Debug.Log($"Found Container for {option.Name}, under {option.ModGUID}", BepInEx.Logging.LogLevel.Debug);
                    containers[i].Add(ref option);
                    return;
                }
            }

            Debug.Log($"Did not find Container for {option.Name}, creating a new one under {option.ModGUID}");

            OptionContainer newContainer = new OptionContainer(option.ModGUID);

            newContainer.Add(ref option);

            containers.Add(newContainer);
        }

        internal static void Add(this List<OptionContainer> containers, ref RiskOfOption mo)
        {
            for (int i = 0; i < containers.Count; i++)
            {
                if (containers[i].ModGUID == mo.ModGUID)
                {
                    
                    Debug.Log($"Found Container for {mo.Name}, under {mo.ModGUID}");

                    if (mo.CategoryName != "")
                    {
                        bool foundCategory = false;

                        for (int x = 0; x < containers[i].GetCategoriesCached().Count; x++)
                        {
                            if (containers[i].GetCategoriesCached()[x].Name == mo.CategoryName)
                            {
                                Debug.Log($"Found Category for {mo.Name}, under {mo.CategoryName}");

                                foundCategory = true;
                                containers[i].GetCategoriesCached()[x].Add(ref mo);
                            }
                            
                        }

                        if (!foundCategory)
                        {
                            Debug.Log($"Did not find a matching Category for {mo.Name}, creating a new one under {mo.CategoryName} \n This category will have no description, please create a category before assigning to it!", BepInEx.Logging.LogLevel.Warning);

                            OptionCategory newCategory = new OptionCategory(mo.CategoryName, mo.ModGUID);

                            newCategory.Add(ref mo);

                            containers[i].Add(ref newCategory);
                        }
                    }

                    containers[i].Add(ref mo);
                    return;
                }
            }

            Debug.Log($"Did not find Container for {mo.Name}, creating a new one under {mo.ModGUID}");

            OptionContainer newContainer = new OptionContainer(mo.ModGUID);

            newContainer.Add(ref mo);

            if (mo.CategoryName != "")
            {
                Debug.Log($"Did not find a matching Category for {mo.Name}, creating a new one under {mo.CategoryName} \n This category will have no description, please create a category before assigning to it!", BepInEx.Logging.LogLevel.Warning);

                OptionCategory newCategory = new OptionCategory(mo.CategoryName, mo.ModGUID);

                newCategory.Add(ref mo);

                newContainer.Add(ref newCategory);
            }

            containers.Add(newContainer);
        }


        internal static bool Contains(this List<OptionCategory> categories, string CategoryName)
        {
            foreach (var category in categories)
            {
                if (category.Name == CategoryName)
                {
                    return true;
                }
            }

            return false;
        }

        internal static bool Contains(this List<OptionContainer> containers, string ModGUID)
        {
            foreach (var container in containers)
            {
                if (container.ModGUID == ModGUID)
                {
                    return true;
                }
            }

            return false;
        }

        internal static int GetContainerIndex(this List<OptionContainer> containers, string ModGUID)
        {
            for (int i = 0; i < containers.Count; i++)
            {
                if (containers[i].ModGUID == ModGUID)
                {
                    return i;
                }
            }

            throw new Exception($"No container exists for this ModGUID: {ModGUID} !");
        }
    }
}
