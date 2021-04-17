using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using BepInEx;
using BepInEx.Logging;
using EntityStates.AncientWispMonster;
using On.RoR2;
using On.RoR2.UI;
using R2API;
using R2API.Utils;
using RiskOfOptions.Containers;
using RiskOfOptions.Interfaces;
using RiskOfOptions.Legacy;
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


        public static void Init()
        {
            On.RoR2.Console.InitConVars += AddRooConVarsToConsole;

            On.RoR2.Console.Awake += AwakeListeners;

            LoadAssets();

            Thunderstore.Init();

            BaseSettingsControlOverride.Init();

            SettingsMenu.Init();

            On.RoR2.PauseManager.CCTogglePause += PauseManagerOnCCTogglePause;
        }


        private static void LoadAssets()
        {
            using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"RiskOfOptions.Resources.riskofoptions"))
            {
                var MainAssetBundle = AssetBundle.LoadFromStream(assetStream);

                ResourcesAPI.AddProvider(new AssetBundleResourcesProvider($"@RiskOfOptions", MainAssetBundle));
            }
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

                //Debug.Log($"{option.ConVar.name} Option registered to console.");
            }

            Debug.Log($"Finished registering to console!");

            _initilized = true;
        }

        private static void PauseManagerOnCCTogglePause(PauseManager.orig_CCTogglePause orig, ConCommandArgs args)
        {
            if (doingKeybind)
                return;

            orig(args);
        }

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        public static void addStartupListener(UnityEngine.Events.UnityAction unityAction)
        {
            Listeners.Add(unityAction);
        }

        public static void AddListener(UnityEngine.Events.UnityAction<bool> unityAction, string name, string categoryName = "Main")
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();
            Indexes indexes = OptionContainers.GetIndexes(modInfo.ModGuid, name, categoryName);

            if (OptionContainers[indexes.ContainerIndex].GetModOptionsCached()[indexes.OptionIndexInContainer] is IBoolProvider boolProvider)
                boolProvider.OnValueChangedBool = unityAction;
        }

        public static void AddListener(UnityEngine.Events.UnityAction<float> unityAction, string name, string categoryName = "Main")
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();
            Indexes indexes = OptionContainers.GetIndexes(modInfo.ModGuid, name, categoryName);

            if (OptionContainers[indexes.ContainerIndex].GetModOptionsCached()[indexes.OptionIndexInContainer] is IFloatProvider floatProvider)
                floatProvider.OnValueChangedFloat = unityAction;
        }

        public static void AddListener(UnityEngine.Events.UnityAction<KeyCode> unityAction, string name, string categoryName = "Main")
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();
            Indexes indexes = OptionContainers.GetIndexes(modInfo.ModGuid, name, categoryName);

            if (OptionContainers[indexes.ContainerIndex].GetModOptionsCached()[indexes.OptionIndexInContainer] is IKeyCodeProvider keyCodeProvider)
                keyCodeProvider.OnValueChangedKeyCode = unityAction;
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
        }

        public static void RegisterOption(RiskOfOption option)
        {
            option.ConVar = option switch
            {
                IBoolProvider boolProvider => new BoolConVar(option.ConsoleToken, RoR2.ConVarFlags.None, option.DefaultValue, option.GetDescriptionAsString()),
                IFloatProvider floatProvider => new FloatConVar(option.ConsoleToken, RoR2.ConVarFlags.None, option.DefaultValue, option.GetDescriptionAsString()),
                IKeyCodeProvider keyCodeProvider => new KeyConVar(option.ConsoleToken, RoR2.ConVarFlags.None, option.DefaultValue, option.GetDescriptionAsString()),
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

        #region CheckBox with string descriptions
        public static void AddCheckBox(string name, string description, bool defaultValue, string categoryName, CheckBoxOverride checkBoxOverride, bool restartRequired = false, bool visibility = true)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            AddCheckBox(modInfo, name, new object[] { description }, defaultValue, categoryName, checkBoxOverride, restartRequired, visibility);
        }

        public static void AddCheckBox(string name, string description, bool defaultValue, string categoryName, bool restartRequired = false, bool visibility = true)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            AddCheckBox(modInfo, name, new object[] { description }, defaultValue, categoryName, null, restartRequired, visibility);
        }

        #endregion

        #region CheckBox with description array
        public static void AddCheckBox(string name, object[] description, bool defaultValue, string categoryName, CheckBoxOverride checkBoxOverride, bool restartRequired = false, bool visibility = true)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            AddCheckBox(modInfo, name, description, defaultValue, categoryName, checkBoxOverride, restartRequired, visibility);
        }

        public static void AddCheckBox(string name, object[] description, bool defaultValue, string categoryName, bool restartRequired = false, bool visibility = true)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            AddCheckBox(modInfo, name, description, defaultValue, categoryName, null, restartRequired, visibility);
        }

        #endregion

        #region Slider with string descriptions

        public static void AddSlider(string name, string description, float defaultValue, float min, float max, string categoryName, SliderOverride sliderOverride, bool restartRequired = false, bool visibility = true)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            AddSlider(modInfo, name, new object[] { description }, defaultValue, min, max, categoryName, sliderOverride, restartRequired, visibility);
        }

        public static void AddSlider(string name, string description, float defaultValue, float min, float max, string categoryName, bool restartRequired = false, bool visibility = true)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            AddSlider(modInfo, name, new object[] { description }, defaultValue, min, max, categoryName, null, restartRequired, visibility);
        }

        #endregion

        #region Slider with description array

        public static void AddSlider(string name, object[] description, float defaultValue, float min, float max, string categoryName, SliderOverride sliderOverride, bool restartRequired = false, bool visibility = true)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            AddSlider(modInfo, name, description , defaultValue, min, max, categoryName, sliderOverride, restartRequired, visibility);
        }

        public static void AddSlider(string name, object[] description, float defaultValue, float min, float max, string categoryName, bool restartRequired = false, bool visibility = true)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            AddSlider(modInfo, name, description, defaultValue, min, max, categoryName, null, restartRequired, visibility);
        }

        #endregion

        #region StepSlider with string descriptions

        public static void AddStepSlider(string name, string description, float defaultValue, float min, float max, float increment, string categoryName, SliderOverride sliderOverride, bool restartRequired = false, bool visibility = true)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            AddStepSlider(modInfo, name, new object[] { description }, defaultValue, min, max, increment, categoryName, sliderOverride, restartRequired, visibility);
        }

        public static void AddStepSlider(string name, string description, float defaultValue, float min, float max, float increment, string categoryName, bool restartRequired = false, bool visibility = true)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            AddStepSlider(modInfo, name, new object[] { description }, defaultValue, min, max, increment, categoryName, null, restartRequired, visibility);
        }

        #endregion

        #region StepSlider with description array

        public static void AddStepSlider(string name, object[] description, float defaultValue, float min, float max, float increment, string categoryName, SliderOverride sliderOverride, bool restartRequired = false, bool visibility = true)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            AddStepSlider(modInfo, name, description, defaultValue, min, max, increment, categoryName, sliderOverride, restartRequired, visibility);
        }

        public static void AddStepSlider(string name, object[] description, float defaultValue, float min, float max, float increment, string categoryName, bool restartRequired = false, bool visibility = true)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            AddStepSlider(modInfo, name, description, defaultValue, min, max, increment, categoryName, null, restartRequired, visibility);
        }

        #endregion


        public static void AddKeyBind(string name, string description, KeyCode defaultValue, string categoryName, bool visibility = true)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            AddKeyBind(modInfo, name, new object[] { description }, defaultValue, categoryName, visibility);
        }

        public static void AddKeyBind(string name, object[] description, KeyCode defaultValue, string categoryName, bool visibility = true)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            AddKeyBind(modInfo, name, description, defaultValue, categoryName, visibility);
        }


        private static void AddCheckBox(ModInfo modInfo, string name, object[] description, bool defaultValue,
            string categoryName, CheckBoxOverride checkBoxOverride, bool restartRequired, bool visibility)
        {
            if (_initilized)
                throw new Exception($"AddCheckBox {name}, under Category {categoryName}, was called after initialization of RiskOfOptions. \n This usually means you are calling this after Awake()");
            RegisterOption(new CheckBoxOption(modInfo.ModGuid, modInfo.ModName, name, description, $"{(defaultValue ? "1" : "0")}", categoryName, checkBoxOverride, visibility, restartRequired));
        }

        private static void AddSlider(ModInfo modInfo, string name, object[] description, float defaultValue,
            float min, float max, string categoryName, SliderOverride sliderOverride, bool restartRequired = false, bool visibility = true)
        {
            if (_initilized)
                throw new Exception($"AddSlider {name}, under Category {categoryName}, was called after initialization of RiskOfOptions. \n This usually means you are calling this after Awake()");

            RegisterOption(new SliderOption(modInfo.ModGuid, modInfo.ModName, name, description, defaultValue.ToString(CultureInfo.InvariantCulture), min, max, categoryName, sliderOverride, visibility, restartRequired));
        }

        private static void AddStepSlider(ModInfo modInfo, string name, object[] description, float defaultValue,
            float min, float max, float increment, string categoryName, SliderOverride sliderOverride, bool restartRequired = false, bool visibility = true)
        {
            if (_initilized)
                throw new Exception($"AddStepSlider {name}, under Category {categoryName}, was called after initialization of RiskOfOptions. \n This usually means you are calling this after Awake()");

            RegisterOption(new StepSliderOption(modInfo.ModGuid, modInfo.ModName, name, description, defaultValue.ToString(CultureInfo.InvariantCulture), min, max, increment, categoryName, sliderOverride, visibility, restartRequired));
        }

        private static void AddKeyBind(ModInfo modInfo, string name, object[] description, KeyCode defaultValue, string categoryName, bool visibility = true)
        {
            if (_initilized)
                throw new Exception($"AddKeyBind {name}, under Category {categoryName}, was called after initialization of RiskOfOptions. \n This usually means you are calling this after Awake()");

            RegisterOption(new KeyBindOption(modInfo.ModGuid, modInfo.ModName, name, description, $"{(int)defaultValue}", categoryName, null, visibility, false));
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
                floatProvider.OnValueChangedFloat = unityAction;
        }

        [Obsolete("Usage of ModOption is depreciated, use RiskOfOption instead.")]
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        public static void addListener(ModOption modOption, UnityEngine.Events.UnityAction<bool> unityAction)
        {
            Indexes indexes = OptionContainers.GetIndexes(modOption.owner, modOption.name);

            if (OptionContainers[indexes.ContainerIndex].GetModOptionsCached()[indexes.OptionIndexInContainer] is IBoolProvider boolProvider)
                boolProvider.OnValueChangedBool = unityAction;
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
                    new object[] {mo.description}, mo.defaultValue, "Main", null, true, false),
                ModOption.OptionType.Slider => new SliderOption(modInfo.ModGuid, modInfo.ModName, mo.name,
                    new object[] {mo.description}, mo.defaultValue, 0, 100, "Main", null, true, false),
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
