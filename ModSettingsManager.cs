using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using EntityStates.AncientWispMonster;
using On.RoR2.UI;
using R2API;
using R2API.Utils;
using RiskOfOptions.Components.OptionComponents;
using RiskOfOptions.Components.Options;
using RiskOfOptions.Components.Panel;
using RiskOfOptions.Containers;
using RiskOfOptions.Legacy;
using RiskOfOptions.Options;
using RoR2;
using RoR2.ConVar;
using UnityEngine;

using static RiskOfOptions.ExtensionMethods;
using ConCommandArgs = RoR2.ConCommandArgs;
using Console = On.RoR2.Console;
using PauseManager = On.RoR2.PauseManager;

#pragma warning disable 618

namespace RiskOfOptions
{
    public static class ModSettingsManager
    {
        internal static readonly ModIndexedOptionCollection OptionCollection = new();

        private static List<UnityEngine.Events.UnityAction> Listeners = new();

        internal static readonly string StartingText = "risk_of_options";

        internal static bool DoingKeybind = false;

        private static bool _initialized = false;

        internal static ModOptionPanelController InstanceModOptionPanelController;

        public static void Init()
        {
            On.RoR2.Console.InitConVars += AddRooConVarsToConsole;

            On.RoR2.Console.Awake += AwakeListeners;

            Resources.Assets.LoadAssets();

            LanguageTokens.Register();

            Thunderstore.Init();
            
            SettingsModifier.Init();

            On.RoR2.PauseManager.CCTogglePause += PauseManagerOnCCTogglePause;
        }

        private static void AwakeListeners(Console.orig_Awake orig, RoR2.Console self)
        {
            orig(self);

            //foreach (var option in OptionContainers.SelectMany(container => container.GetModOptionsCached()))
            //{
            //    option.SetValue(RoR2.Console.instance.FindConVar(option.ConsoleToken).GetString());
            //}

            foreach (var item in Listeners)
            {
                item.Invoke();
            }
        }

        private static void AddRooConVarsToConsole(Console.orig_InitConVars orig, RoR2.Console self)
        {
            orig(self);

            Debug.Log($"Registering options to console.");

            // foreach (var option in OptionContainers.SelectMany(container => container.GetModOptionsCached()))
            // {
            //     option.ConVar.SetString(option.GetInternalValueAsString());
            //     
            //     self.InvokeMethod("RegisterConVarInternal", new object[] { option.ConVar });
            //
            //     if (option.invokeValueChangedEventOnStart)
            //     {
            //         option.Invoke();
            //     }
            // }

            Debug.Log($"Finished registering to console!");
            
            Thunderstore.GrabIcons();

            _initialized = true;
        }

        private static void PauseManagerOnCCTogglePause(PauseManager.orig_CCTogglePause orig, ConCommandArgs args)
        {
            if (DoingKeybind)
                return;

            orig(args);
        }

        public static void SetPanelDescription(string description)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetModInfo();

            SetPanelDescription(modInfo, new object[] { description });
        }

        public static void SetPanelDescription(object[] description)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetModInfo();

            SetPanelDescription(modInfo, description);
        }

        private static void SetPanelDescription(ModInfo modInfo, object[] description)
        {
            //OptionContainers[OptionContainers.GetContainerIndex(modInfo.ModGuid, modInfo.ModName, true)].Description = description;
        }

        public static void SetPanelTitle(string title)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetModInfo();

            //OptionContainers[OptionContainers.GetContainerIndex(modInfo.ModGuid, modInfo.ModName, true)].Title = title;
        }

        public static void SetModIcon(Sprite iconSprite)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetModInfo();

            Thunderstore.AddIcon(modInfo.ModGuid, iconSprite);
        }

        public static void SetVisibility(string name, string categoryName, bool visibility)
        {
            // ModInfo modInfo = Assembly.GetCallingAssembly().GetModInfo();
            // Indexes indexes = OptionContainers.GetIndexes(modInfo.ModGuid, name, categoryName);
            //
            // OptionContainers[indexes.ContainerIndex].GetModOptionsCached()[indexes.OptionIndexInContainer].Visibility = visibility;
            //
            // if (InstanceModOptionPanelController)
            // {
            //     InstanceModOptionPanelController.UpdateVisibility(OptionContainers[indexes.ContainerIndex].GetModOptionsCached()[indexes.OptionIndexInContainer].OptionToken, visibility);
            // }
        }

        public static void AddOption(BaseOption option)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetModInfo();

            option.ModGuid = modInfo.ModGuid;
            option.ModName = modInfo.ModName;
            option.Identifier = $"{modInfo.ModGuid}.{option.Category}.{option.Name}.{option.OptionTypeName}".Replace(" ", "_").ToUpper();
            
            LanguageAPI.Add(option.GetNameToken(), option.Name);
            
            OptionCollection.AddOption(ref option);
        }

        internal static Thunderstore.ModSearchEntry[] GetIconSearchEntries()
        {
            List<Thunderstore.ModSearchEntry> modSearchEntries = new List<Thunderstore.ModSearchEntry>();

            // foreach (var container in OptionContainers)
            // {
            //     modSearchEntries.Add(new Thunderstore.ModSearchEntry()
            //     {
            //         fullName = $"{container.ModGuid.Split('.')[1]}-{container.ModGuid.Split('.')[2]}",
            //         fullNameWithUnderscores = $"{container.ModGuid.Split('.')[1]}-{container.ModName.Replace(" ", "_")}",
            //         fullNameWithoutSpaces = $"{container.ModGuid.Split('.')[1]}-{container.ModName.Replace(" ", "")}",
            //         nameWithUnderscores = $"{container.ModName.Replace(" ", "_")}",
            //         nameWithoutSpaces = $"{container.ModName.Replace(" ", "")}",
            //         modGuid = container.ModGuid,
            //         modName = container.ModName
            //     });
            // }

            return modSearchEntries.ToArray();
        }

        #region ModOption Legacy Stuff

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        [Obsolete("There is no longer a need to have a startup listener.\n" +
                  " Since option values are available immediately after adding them, there is no longer a need to wait until RiskOfOptions is finished initializing.")]
        public static void addStartupListener(UnityEngine.Events.UnityAction unityAction)
        {
            Listeners.Add(unityAction);
        }

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        [Obsolete()]
        public static void setPanelTitle(string title)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetModInfo();

            //OptionContainers[OptionContainers.GetContainerIndex(modInfo.ModGuid, modInfo.ModName, true)].Title = title;
        }

        // ReSharper disable once InconsistentNaming
        [Obsolete()]
        public static void setPanelDescription(string description)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetModInfo();

            SetPanelDescription(modInfo, new object[] { description });
        }

        // [Obsolete("Usage of ModOption is depreciated, use RiskOfOption instead.")]
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        // public static void addListener(ModOption modOption, UnityEngine.Events.UnityAction<float> unityAction)
        // {
            // Indexes indexes = OptionContainers.GetIndexes(modOption.owner, modOption.name);
            //
            // if (OptionContainers[indexes.ContainerIndex].GetModOptionsCached()[indexes.OptionIndexInContainer] is IFloatProvider floatProvider)
            //     floatProvider.OnValueChanged.AddListener(unityAction);
        // }

        // [Obsolete("Usage of ModOption is depreciated, use RiskOfOption instead.")]
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        // public static void addListener(ModOption modOption, UnityEngine.Events.UnityAction<bool> unityAction)
        // {
            // Indexes indexes = OptionContainers.GetIndexes(modOption.owner, modOption.name);
            //
            // if (OptionContainers[indexes.ContainerIndex].GetModOptionsCached()[indexes.OptionIndexInContainer] is IBoolProvider boolProvider)
            //     boolProvider.OnValueChanged.AddListener(unityAction);
        // }

        // [Obsolete("ModOptions are handled internally now. Please use AddCheckBox, AddSlider, etc", false)]
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        // public static void addOption(ModOption mo)
        // {
            // Debug.Log($"Legacy ModOption {mo.name} constructed, converting to RiskOfOption...");
            //
            // ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();
            //
            // RiskOfOption newOption = mo.optionType switch
            // {
            //     ModOption.OptionType.Bool => new CheckBoxOption(modInfo.ModGuid, modInfo.ModName, mo.name,
            //         new object[] {mo.description}, mo.defaultValue, "Main", null, true, false, null, false),
            //     ModOption.OptionType.Slider => new SliderOption(modInfo.ModGuid, modInfo.ModName, mo.name,
            //         new object[] {mo.description}, mo.defaultValue, 0, 100, "Main", null, true, null, false),
            //     ModOption.OptionType.Keybinding => throw new NotImplementedException("KeyBinds are not supported with the legacy ModOptions! use the new AddKeyBinding() method instead."),
            //     _ => throw new ArgumentOutOfRangeException()
            // };
            //
            // RegisterOption(newOption);
        // }

        // [Obsolete("ModOption is obsolete, please use RiskOfOption instead.")]
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        // public static ModOption getOption(string name)
        // {
            // ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();
            //
            // foreach (var item in OptionContainers[OptionContainers.GetContainerIndex(modInfo.ModGuid, modInfo.ModName)].GetModOptionsCached())
            // {
            //     if (!string.Equals(item.Name, name, StringComparison.InvariantCultureIgnoreCase))
            //         continue;
            //
            //     ModOption temp = item switch
            //     {
            //         IBoolProvider checkBoxOption => new ModOption(ModOption.OptionType.Bool, item.Name, item.GetDescriptionAsString(), item.DefaultValue) { conVar = item.ConVar },
            //         IFloatProvider sliderOption => new ModOption(ModOption.OptionType.Slider, item.Name, item.GetDescriptionAsString(), item.DefaultValue) { conVar = item.ConVar },
            //         _ => throw new ArgumentOutOfRangeException()
            //     };
            //
            //     temp.SetOwner(modInfo.ModGuid);
            //
            //     return temp;
            // }
            //
            // Debug.Log($"ModOption {name} not found!", BepInEx.Logging.LogLevel.Error);
            // return null;
        // }


        //[Obsolete("Use GetOption(Name, Category).GetBool() / .GetFloat() / .GetKeyCode() instead")]
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        // public static string getOptionValue(string name)
        // {
        //     ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();
        //
        //     BaseConVar conVar = (from item in OptionContainers[OptionContainers.GetContainerIndex(modInfo.ModGuid, modInfo.ModName)].GetModOptionsCached() where string.Equals(item.Name, name, StringComparison.InvariantCultureIgnoreCase) select RoR2.Console.instance.FindConVar(item.ConsoleToken)).FirstOrDefault();
        //
        //     if (conVar != null)
        //         return conVar.GetString();
        //
        //     Debug.Log($"Convar {name} not found in convars.", BepInEx.Logging.LogLevel.Error);
        //     return "";
        // }
        #endregion
    }
}
