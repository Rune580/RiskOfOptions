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
using On.RoR2;
using On.RoR2.UI;
using R2API;
using R2API.Utils;
using RiskOfOptions.Containers;
using RiskOfOptions.Interfaces;
using RiskOfOptions.Legacy;
using RiskOfOptions.OptionComponents;
using RiskOfOptions.OptionConstructors;
using RiskOfOptions.OptionOverrides;
using RiskOfOptions.Options;
using RoR2.ConVar;
using UnityEngine;

using static RiskOfOptions.ExtensionMethods;
using ConCommandArgs = RoR2.ConCommandArgs;
using Console = On.RoR2.Console;

#pragma warning disable 618

namespace RiskOfOptions
{
    public static class ModSettingsManager
    {
        internal static List<OptionContainer> OptionContainers = new List<OptionContainer>();

        private static List<UnityEngine.Events.UnityAction> Listeners = new List<UnityEngine.Events.UnityAction>();

        internal static readonly string StartingText = "risk_of_options";

        internal static bool doingKeybind = false;

        private static bool _initilized = false;

        internal static ModOptionPanelController instanceModOptionPanelController;

        public static void Init()
        {
            On.RoR2.Console.InitConVars += AddRooConVarsToConsole;

            On.RoR2.Console.Awake += AwakeListeners;

            Resources.Assets.LoadAssets();

            LanguageTokens.Register();

            Thunderstore.Init();

            BaseSettingsControlOverride.Init();

            SettingsMenu.Init();

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

            foreach (var option in OptionContainers.SelectMany(container => container.GetModOptionsCached()))
            {
                option.ConVar.SetString(option.GetInternalValueAsString());


                self.InvokeMethod("RegisterConVarInternal", new object[] { option.ConVar });

                if (option.invokeValueChangedEventOnStart)
                {
                    switch (option)
                    {
                        case CheckBoxOption checkBoxOption:
                            checkBoxOption.InvokeListeners(option.GetValue<bool>());
                            break;
                        case SliderOption sliderOption:
                            sliderOption.InvokeListeners(option.GetValue<float>());
                            break;
                        case KeyBindOption keyBindOption:
                            keyBindOption.InvokeListeners(option.GetValue<KeyCode>());
                            break;
                        case DropDownOption dropDownOption:
                            dropDownOption.InvokeListeners(option.GetValue<int>());
                            break;
                    }
                }
            }

            Debug.Log($"Finished registering to console!");
            
            Thunderstore.GrabIcons();

            _initilized = true;
        }

        private static void PauseManagerOnCCTogglePause(PauseManager.orig_CCTogglePause orig, ConCommandArgs args)
        {
            if (doingKeybind)
                return;

            orig(args);
        }

        public static void AddListener(UnityEngine.Events.UnityAction<bool> unityAction, string name, string categoryName = "Main")
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();
            Indexes indexes = OptionContainers.GetIndexes(modInfo.ModGuid, name, categoryName);

            if (OptionContainers[indexes.ContainerIndex].GetModOptionsCached()[indexes.OptionIndexInContainer] is IBoolProvider boolProvider)
                boolProvider.Events.Add(unityAction);
        }

        public static void AddListener(UnityEngine.Events.UnityAction<float> unityAction, string name, string categoryName = "Main")
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();
            Indexes indexes = OptionContainers.GetIndexes(modInfo.ModGuid, name, categoryName);

            if (OptionContainers[indexes.ContainerIndex].GetModOptionsCached()[indexes.OptionIndexInContainer] is IFloatProvider floatProvider)
                floatProvider.Events.Add(unityAction);
        }

        public static void AddListener(UnityEngine.Events.UnityAction<KeyCode> unityAction, string name, string categoryName = "Main")
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();
            Indexes indexes = OptionContainers.GetIndexes(modInfo.ModGuid, name, categoryName);

            if (OptionContainers[indexes.ContainerIndex].GetModOptionsCached()[indexes.OptionIndexInContainer] is IKeyCodeProvider keyCodeProvider)
                keyCodeProvider.Events.Add(unityAction);
        }

        public static void AddListener(UnityEngine.Events.UnityAction<int> unityAction, string name, string categoryName = "Main")
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();
            Indexes indexes = OptionContainers.GetIndexes(modInfo.ModGuid, name, categoryName);

            if (OptionContainers[indexes.ContainerIndex].GetModOptionsCached()[indexes.OptionIndexInContainer] is IIntProvider intProvider)
                intProvider.Events.Add(unityAction);
        }

        public static RiskOfOption GetOption(string name, string categoryName = "Main")
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();
            Indexes indexes = OptionContainers.GetIndexes(modInfo.ModGuid, name, categoryName);

            return OptionContainers[indexes.ContainerIndex].GetModOptionsCached()[indexes.OptionIndexInContainer];
        }

        internal static RiskOfOption GetOption(string name, string categoryName, string modGuid)
        {
            Indexes indexes = OptionContainers.GetIndexes(modGuid, name, categoryName);

            return OptionContainers[indexes.ContainerIndex].GetModOptionsCached()[indexes.OptionIndexInContainer];
        }


        public static void SetPanelDescription(string description)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            SetPanelDescription(modInfo, new object[] { description });
        }

        public static void SetPanelDescription(object[] description)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            SetPanelDescription(modInfo, description);
        }

        private static void SetPanelDescription(ModInfo modInfo, object[] description)
        {
            OptionContainers[OptionContainers.GetContainerIndex(modInfo.ModGuid, modInfo.ModName, true)].Description = description;
        }

        public static void SetPanelTitle(string title)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            OptionContainers[OptionContainers.GetContainerIndex(modInfo.ModGuid, modInfo.ModName, true)].Title = title;
        }

        public static void SetModIcon(Sprite iconSprite)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            Thunderstore.AddIcon(modInfo.ModGuid, iconSprite);
        }

        public static void SetVisibility(string name, string categoryName, bool visibility)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();
            Indexes indexes = OptionContainers.GetIndexes(modInfo.ModGuid, name, categoryName);

            OptionContainers[indexes.ContainerIndex].GetModOptionsCached()[indexes.OptionIndexInContainer].Visibility = visibility;

            if (instanceModOptionPanelController)
            {
                instanceModOptionPanelController.UpdateVisibility(OptionContainers[indexes.ContainerIndex].GetModOptionsCached()[indexes.OptionIndexInContainer].OptionToken, visibility);
            }
        }

        public static void RegisterOption(RiskOfOption option)
        {
            option.ConVar = option switch
            {
                IBoolProvider boolProvider => new BoolConVar(option.ConsoleToken, RoR2.ConVarFlags.None, option.DefaultValue, option.GetDescriptionAsString()),
                IFloatProvider floatProvider => new FloatConVar(option.ConsoleToken, RoR2.ConVarFlags.None, option.DefaultValue, option.GetDescriptionAsString()),
                IKeyCodeProvider keyCodeProvider => new KeyConVar(option.ConsoleToken, RoR2.ConVarFlags.None, option.DefaultValue, option.GetDescriptionAsString()),
                IIntProvider intProvider => new IntConVar(option.ConsoleToken, RoR2.ConVarFlags.None, option.DefaultValue, option.GetDescriptionAsString()),
                _ => throw new Exception($"Option {option.Name} somehow managed to not implement a provider interface! please contact me on github or discord.")
            };

            if (option.CategoryName == "Main")
            {
                CreateCategory(option.CategoryName, option.ModGuid, option.ModName);
            }

            string loadedValue = OptionSerializer.Load(option.ConsoleToken);


            if (!string.IsNullOrEmpty(loadedValue))
                option.SetValue(loadedValue);

            OptionContainers.Add(ref option);
        }

        public static void AddOption(OptionConstructorBase option)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            if (_initilized)
                throw new Exception($"An AddOption() was called with the option name: {option.Name} from the mod {modInfo.ModName}, after initialization of RiskOfOptions. \n This usually means you are calling this after Awake()");


            switch (option)
            {
                case CheckBox checkBox:
                    if (checkBox.ConfigEntry != null)
                    {
                        if (string.IsNullOrEmpty(checkBox.Name))
                            checkBox.Name = checkBox.ConfigEntry.Definition.Key;

                        if (string.IsNullOrEmpty(checkBox.CategoryName))
                            checkBox.CategoryName = checkBox.ConfigEntry.Definition.Section;

                        if (string.IsNullOrEmpty((string)checkBox.descriptionArray[0]))
                            checkBox.Description = checkBox.ConfigEntry.Description.Description;

                        checkBox.DefaultValue = checkBox.ConfigEntry.Value;
                    }
                    RegisterOption(new CheckBoxOption(modInfo.ModGuid, modInfo.ModName, checkBox.Name,
                        option.descriptionArray, checkBox.value, checkBox.CategoryName,
                        checkBox.Override, checkBox.IsVisible, checkBox.RestartRequired, checkBox.OnValueChanged,
                        checkBox.InvokeValueChangedEventOnStart)
                        {configEntry = checkBox.ConfigEntry});
                    break;
                case StepSlider stepSlider:
                    if (stepSlider.ConfigEntry != null)
                    {
                        if (string.IsNullOrEmpty(stepSlider.Name))
                            stepSlider.Name = stepSlider.ConfigEntry.Definition.Key;

                        if (string.IsNullOrEmpty(stepSlider.CategoryName))
                            stepSlider.CategoryName = stepSlider.ConfigEntry.Definition.Section;

                        if (string.IsNullOrEmpty((string)stepSlider.descriptionArray[0]))
                            stepSlider.Description = stepSlider.ConfigEntry.Description.Description;

                        stepSlider.DefaultValue = stepSlider.ConfigEntry.Value;
                    }
                    RegisterOption(new StepSliderOption(modInfo.ModGuid, modInfo.ModName, stepSlider.Name, stepSlider.descriptionArray,
                        stepSlider.value, stepSlider.Min, stepSlider.Max, stepSlider.Increment, stepSlider.CategoryName,
                        stepSlider.Override, stepSlider.IsVisible, stepSlider.OnValueChanged, stepSlider.InvokeValueChangedEventOnStart)
                        {configEntry = stepSlider.ConfigEntry});
                    break;
                case Slider slider:
                    if (slider.ConfigEntry != null)
                    {
                        if (string.IsNullOrEmpty(slider.Name))
                            slider.Name = slider.ConfigEntry.Definition.Key;

                        if (string.IsNullOrEmpty(slider.CategoryName))
                            slider.CategoryName = slider.ConfigEntry.Definition.Section;

                        if (string.IsNullOrEmpty((string)slider.descriptionArray[0]))
                            slider.Description = slider.ConfigEntry.Description.Description;

                        slider.DefaultValue = slider.ConfigEntry.Value;
                    }
                    RegisterOption(new SliderOption(modInfo.ModGuid, modInfo.ModName, slider.Name, slider.descriptionArray,
                        slider.value, slider.Min, slider.Max, slider.CategoryName, slider.Override,
                        slider.IsVisible, slider.OnValueChanged, slider.InvokeValueChangedEventOnStart)
                        {configEntry = slider.ConfigEntry});
                    break;
                case KeyBind keyBind:
                    if (keyBind.ConfigEntry != null)
                    {
                        if (string.IsNullOrEmpty(keyBind.Name))
                            keyBind.Name = keyBind.ConfigEntry.Definition.Key;

                        if (string.IsNullOrEmpty(keyBind.CategoryName))
                            keyBind.CategoryName = keyBind.ConfigEntry.Definition.Section;

                        if (string.IsNullOrEmpty((string)keyBind.descriptionArray[0]))
                            keyBind.Description = keyBind.ConfigEntry.Description.Description;

                        keyBind.DefaultValue = keyBind.ConfigEntry.Value.MainKey;

                        if (keyBind.ConfigEntry.Value.Modifiers.Any())
                            throw new WarningException($"The KeyBind {keyBind.Name} contains modifier keys! Currently Risk Of Options doesn't support modifier keys!");
                    }
                    RegisterOption(new KeyBindOption(modInfo.ModGuid, modInfo.ModName, keyBind.Name, keyBind.descriptionArray,
                        keyBind.value, keyBind.CategoryName, keyBind.IsVisible, keyBind.OnValueChanged, keyBind.InvokeValueChangedEventOnStart)
                        {configEntry = keyBind.ConfigEntry});
                    break;
                case DropDown dropDown:
                    RegisterOption(new DropDownOption(modInfo.ModGuid, modInfo.ModName, dropDown.Name,
                        dropDown.descriptionArray, dropDown.value, dropDown.CategoryName, dropDown.Choices,
                        dropDown.IsVisible, dropDown.RestartRequired, dropDown.OnValueChanged,
                        dropDown.InvokeValueChangedEventOnStart));
                    break;
            }
        }

        public static void CreateCategory(string name)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            if (!OptionContainers.Contains(modInfo.ModGuid))
                OptionContainers.Add(new OptionContainer(modInfo.ModGuid, modInfo.ModName));


            for (int i = 0; i < OptionContainers[OptionContainers.GetContainerIndex(modInfo.ModGuid, modInfo.ModName)].GetCategoriesCached().Count; i++)
            {
                if (OptionContainers[OptionContainers.GetContainerIndex(modInfo.ModGuid, modInfo.ModName)].GetCategoriesCached()[i].Name == name)
                {
                    Debug.Log($"Category {name} already exists!, please make sure you aren't assigning a category before creating one, or you aren't creating the same category twice!", BepInEx.Logging.LogLevel.Warning);
                    return;
                }
            }

            OptionCategory newCategory = new OptionCategory(name, modInfo.ModGuid);

            OptionContainers[OptionContainers.GetContainerIndex(modInfo.ModGuid, modInfo.ModName)].Add(ref newCategory);
        }


        internal static void CreateCategory(string name, string modGuid, string modName)
        {
            if (!OptionContainers.Contains(modGuid))
                OptionContainers.Add(new OptionContainer(modGuid, modName));

            for (int i = 0; i < OptionContainers[OptionContainers.GetContainerIndex(modGuid, modName)].GetCategoriesCached().Count; i++)
            {
                if (OptionContainers[OptionContainers.GetContainerIndex(modGuid, modName)].GetCategoriesCached()[i].Name == name)
                {
                    //Debug.Log($"Category {Name} already exists!, please make sure you aren't assigning a category before creating one, or you aren't creating the same category twice!", BepInEx.Logging.LogLevel.Warning);
                    return;
                }
            }

            OptionCategory newCategory = new OptionCategory(name, modGuid);


            OptionContainers[OptionContainers.GetContainerIndex(modGuid, modName)].Insert(ref newCategory);
        }

        internal static RiskOfOption GetOption(string consoleToken)
        {
            foreach (OptionContainer container in OptionContainers)
            {
                for (int i = 0; i < container.GetModOptionsCached().Count; i++)
                {
                    if (container.GetModOptionsCached()[i].ConsoleToken == consoleToken)
                    {
                        return container.GetModOptionsCached()[i];
                    }
                }
            }

            throw new Exception($"An ROO couldn't be found for {consoleToken}!");
        }

        internal static Thunderstore.ModSearchEntry[] GetIconSearchEntries()
        {
            List<Thunderstore.ModSearchEntry> modSearchEntries = new List<Thunderstore.ModSearchEntry>();

            foreach (var container in OptionContainers)
            {
                modSearchEntries.Add(new Thunderstore.ModSearchEntry()
                {
                    fullName = $"{container.ModGuid.Split('.')[1]}-{container.ModGuid.Split('.')[2]}",
                    fullNameWithUnderscores = $"{container.ModGuid.Split('.')[1]}-{container.ModName.Replace(" ", "_")}",
                    fullNameWithoutSpaces = $"{container.ModGuid.Split('.')[1]}-{container.ModName.Replace(" ", "")}",
                    nameWithUnderscores = $"{container.ModName.Replace(" ", "_")}",
                    nameWithoutSpaces = $"{container.ModName.Replace(" ", "")}",
                    modGuid = container.ModGuid,
                    modName = container.ModName
                });
                //Debug.Log($"Search terms for {container.ModGuid} are:" +
                //          $"\n {modSearchEntries[modSearchEntries.Count - 1].fullName}" +
                //          $"\n {modSearchEntries[modSearchEntries.Count - 1].fullNameWithUnderscores}" +
                //          $"\n {modSearchEntries[modSearchEntries.Count - 1].fullNameWithoutSpaces}" +
                //          $"\n {modSearchEntries[modSearchEntries.Count - 1].nameWithUnderscores}" +
                //          $"\n {modSearchEntries[modSearchEntries.Count - 1].nameWithoutSpaces}");
            }

            return modSearchEntries.ToArray();
        }

        //internal struct OptionOverrideInfo
        //{
        //    internal string name;
        //    internal string categoryName;
        //    internal string modGuid;
        //}

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
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            OptionContainers[OptionContainers.GetContainerIndex(modInfo.ModGuid, modInfo.ModName, true)].Title = title;
        }

        // ReSharper disable once InconsistentNaming
        [Obsolete()]
        public static void setPanelDescription(string description)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            SetPanelDescription(modInfo, new object[] { description });
        }

        [Obsolete("Usage of ModOption is depreciated, use RiskOfOption instead.")]
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        public static void addListener(ModOption modOption, UnityEngine.Events.UnityAction<float> unityAction)
        {
            Indexes indexes = OptionContainers.GetIndexes(modOption.owner, modOption.name);

            if (OptionContainers[indexes.ContainerIndex].GetModOptionsCached()[indexes.OptionIndexInContainer] is IFloatProvider floatProvider)
                floatProvider.Events.Add(unityAction);
        }

        [Obsolete("Usage of ModOption is depreciated, use RiskOfOption instead.")]
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        public static void addListener(ModOption modOption, UnityEngine.Events.UnityAction<bool> unityAction)
        {
            Indexes indexes = OptionContainers.GetIndexes(modOption.owner, modOption.name);

            if (OptionContainers[indexes.ContainerIndex].GetModOptionsCached()[indexes.OptionIndexInContainer] is IBoolProvider boolProvider)
                boolProvider.Events.Add(unityAction);
        }

        [Obsolete("ModOptions are handled internally now. Please use AddCheckBox, AddSlider, etc", false)]
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        public static void addOption(ModOption mo)
        {
            Debug.Log($"Legacy ModOption {mo.name} constructed, converting to RiskOfOption...");

            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            RiskOfOption newOption = mo.optionType switch
            {
                ModOption.OptionType.Bool => new CheckBoxOption(modInfo.ModGuid, modInfo.ModName, mo.name,
                    new object[] {mo.description}, mo.defaultValue, "Main", null, true, false, null, false),
                ModOption.OptionType.Slider => new SliderOption(modInfo.ModGuid, modInfo.ModName, mo.name,
                    new object[] {mo.description}, mo.defaultValue, 0, 100, "Main", null, true, null, false),
                ModOption.OptionType.Keybinding => throw new NotImplementedException("KeyBinds are not supported with the legacy ModOptions! use the new AddKeyBinding() method instead."),
                _ => throw new ArgumentOutOfRangeException()
            };

            RegisterOption(newOption);
        }

        [Obsolete("ModOption is obsolete, please use RiskOfOption instead.")]
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        public static ModOption getOption(string name)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            foreach (var item in OptionContainers[OptionContainers.GetContainerIndex(modInfo.ModGuid, modInfo.ModName)].GetModOptionsCached())
            {
                if (!string.Equals(item.Name, name, StringComparison.InvariantCultureIgnoreCase))
                    continue;

                ModOption temp = item switch
                {
                    IBoolProvider checkBoxOption => new ModOption(ModOption.OptionType.Bool, item.Name, item.GetDescriptionAsString(), item.DefaultValue) { conVar = item.ConVar },
                    IFloatProvider sliderOption => new ModOption(ModOption.OptionType.Slider, item.Name, item.GetDescriptionAsString(), item.DefaultValue) { conVar = item.ConVar },
                    _ => throw new ArgumentOutOfRangeException()
                };

                temp.SetOwner(modInfo.ModGuid);

                return temp;
            }

            Debug.Log($"ModOption {name} not found!", BepInEx.Logging.LogLevel.Error);
            return null;
        }


        //[Obsolete("Use GetOption(Name, Category).GetBool() / .GetFloat() / .GetKeyCode() instead")]
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        public static string getOptionValue(string name)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            BaseConVar conVar = (from item in OptionContainers[OptionContainers.GetContainerIndex(modInfo.ModGuid, modInfo.ModName)].GetModOptionsCached() where string.Equals(item.Name, name, StringComparison.InvariantCultureIgnoreCase) select RoR2.Console.instance.FindConVar(item.ConsoleToken)).FirstOrDefault();

            if (conVar != null)
                return conVar.GetString();

            Debug.Log($"Convar {name} not found in convars.", BepInEx.Logging.LogLevel.Error);
            return "";
        }
        #endregion
    }
}
