using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MonoMod.RuntimeDetour;
using RiskOfOptions.Config;
using RiskOfOptions.Config.OptionProviders;
using RiskOfOptions.Containers;
using RiskOfOptions.Lib;
using RiskOfOptions.Options;
using RoR2;
using UnityEngine;
using ConCommandArgs = RoR2.ConCommandArgs;
using Debug = RiskOfOptions.Utils.Debug;

namespace RiskOfOptions;

public static class ModSettingsManager
{
    private static Hook? _pauseHook;

    internal static readonly ModIndexedOptionCollection OptionCollection = new();

    private static readonly HashSet<IModConfigProvider> ModConfigProviders = [];
    private static readonly HashSet<ConfigItemOptionProvider> ConfigItemOptionProviders = [];

    private static readonly HashSet<string> AutoGenerateModGuidBlacklist = [];
    private static readonly HashSet<OptionId> AutoGenerateConfigEntryIdBlacklist = [];
    private static bool _autoGenerationComplete;

    internal const string StartingText = "RISK_OF_OPTIONS";
    internal const int StartingTextLength = 15;

    internal static bool disablePause = false;

    public static readonly HashSet<OptionId> RestartRequiredOptions = [];

    internal static void Init()
    {
        LanguageApi.Init();

        Resources.Assets.LoadAssets();
        Resources.Prefabs.Init();

        LanguageTokens.Register();

        SettingsModifier.Init();

        ModConfigProviders.Add(new BepInExModConfigProvider());
        
        ConfigItemOptionProviders.Add(new CheckBoxOptionProvider());
        ConfigItemOptionProviders.Add(new FloatFieldOptionProvider());
        ConfigItemOptionProviders.Add(new FloatSliderOptionProvider());
        ConfigItemOptionProviders.Add(new FloatStepSliderOptionProvider());
        ConfigItemOptionProviders.Add(new IntFieldOptionProvider());
        ConfigItemOptionProviders.Add(new IntSliderOptionProvider());
        ConfigItemOptionProviders.Add(new StringInputFieldOptionProvider());
        ConfigItemOptionProviders.Add(new ColorPickerOptionProvider());
        ConfigItemOptionProviders.Add(new KeyBindOptionProvider());
        ConfigItemOptionProviders.Add(new EnumDropDownOptionProvider());

        var targetMethod = typeof(PauseManager).GetMethod("CCTogglePause", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        var destMethod = typeof(ModSettingsManager).GetMethod(nameof(PauseManagerOnCCTogglePause), BindingFlags.NonPublic | BindingFlags.Static);
        _pauseHook ??= new Hook(targetMethod, destMethod);
    }

    internal static void AutoGenerateFromConfigs()
    {
        if (_autoGenerationComplete)
            return;
        
        Debug.Info("Starting config auto-generation");

        var optionProviders = ConfigItemOptionProviders.OrderByDescending(item => item.Specificity)
            .ToArray();

        foreach (var modConfigProvider in ModConfigProviders)
        {
            foreach (var modConfig in modConfigProvider.GetModConfigs())
            {
                if (AutoGenerateModGuidBlacklist.Contains(modConfig.ModGuid))
                    continue;

                foreach (var configItem in modConfig.GetConfigItems())
                {
                    var id = new OptionId(modConfig.ModGuid, configItem.Section, configItem.Name);

                    if (AutoGenerateConfigEntryIdBlacklist.Contains(id))
                        continue;

                    var handled = false;
                    foreach (var optionProvider in optionProviders)
                    {
                        if (!optionProvider.CanHandle(configItem))
                            continue;
                        
                        AddOption(
                            optionProvider.CreateOption(configItem),
                            modConfig.ModGuid,
                            modConfig.ModName
                        );
                        handled = true;
                    }

                    if (!handled)
                        Debug.Warn($"Unhandled ConfigItem: \"{id}\", Type: {configItem.GetType()}");
                }
                
                if (OptionCollection.TryGetCollection(modConfig.ModGuid, out var collection))
                {
                    // Set mod icon if not yet set.
                    if (collection.icon is null && collection.iconPrefab is null && modConfig.ModIcon)
                        collection.icon = modConfig.ModIcon;

                    // Set mod description it not yet set.
                    if (!collection.DescriptionSet)
                    {
                        if (modConfig.ModDescription.IsToken)
                        {
                            collection.DescriptionToken = modConfig.ModDescription;
                        }
                        else
                        {
                            collection.SetDescriptionText(modConfig.ModDescription);
                        }
                    }
                }
            }
        }

        _autoGenerationComplete = true;
    }

    private static void PauseManagerOnCCTogglePause(Action<ConCommandArgs> orig, ConCommandArgs args)
    {
        if (disablePause)
            return;

        orig(args);
    }

    public static void AddModConfigProvider(IModConfigProvider modConfigProvider)
    {
        ModConfigProviders.Add(modConfigProvider);
    }

    public static void AddConfigItemOptionProvider(ConfigItemOptionProvider configItemOptionProvider)
    {
        ConfigItemOptionProviders.Add(configItemOptionProvider);
    }

    public static void SetModDescription(string description)
    {
        ModMetaData modMetaData = Assembly.GetCallingAssembly().GetModMetaData();

        SetModDescription(description, modMetaData.Guid, modMetaData.Name);
    }

    public static void SetModDescription(string description, string modGuid, string modName)
    {
        EnsureContainerExists(modGuid, modName);

        OptionCollection[modGuid].SetDescriptionText(description);
    }

    public static void SetModDescriptionToken(string descriptionToken)
    {
        ModMetaData modMetaData = Assembly.GetCallingAssembly().GetModMetaData();

        SetModDescriptionToken(descriptionToken, modMetaData.Guid, modMetaData.Name);
    }

    public static void SetModDescriptionToken(string descriptionToken, string modGuid, string modName)
    {
        EnsureContainerExists(modGuid, modName);

        OptionCollection[modGuid].DescriptionToken = descriptionToken;
    }

    public static void SetModIcon(Sprite iconSprite)
    {
        var modMetaData = Assembly.GetCallingAssembly().GetModMetaData();

        SetModIcon(iconSprite, modMetaData.Guid, modMetaData.Name);
    }

    public static void SetModIcon(Sprite iconSprite, string modGuid, string modName)
    {
        EnsureContainerExists(modGuid, modName);

        OptionCollection[modGuid].icon = iconSprite;
    }

    public static void SetModIcon(GameObject iconPrefab)
    {
        var modMetaData = Assembly.GetCallingAssembly().GetModMetaData();

        SetModIcon(iconPrefab, modMetaData.Guid, modMetaData.Name);
    }

    public static void SetModIcon(GameObject iconPrefab, string modGuid, string modName)
    {
        EnsureContainerExists(modGuid, modName);

        OptionCollection[modGuid].iconPrefab = iconPrefab;
    }

    public static void AddOption(BaseOption option)
    {
        var modMetaData = Assembly.GetCallingAssembly().GetModMetaData();

        AddOption(option, modMetaData.Guid, modMetaData.Name);
    }

    public static void AddOption(BaseOption option, string modGuid, string modName)
    {
        option.SetProperties();

        option.ModGuid = modGuid;
        option.ModName = modName;
        option.Id = new OptionId(modGuid, option.Category, option.Name);

        option.RegisterTokens();

        if (option.BaseConfigItem is not null)
        {
            AutoGenerateConfigEntryIdBlacklist.Add(option.Id);
            Debug.Info($"Added {option.Id} to blacklist!");
        }
        
        OptionCollection.AddOption(ref option);
    }

    /// <summary>
    /// Creates an option with the option of custom name and description tokens.
    /// </summary>
    /// <param name="option">The base option to create</param>
    /// <param name="modGuid">GUID of the mod</param>
    /// <param name="modName">Name of the mod</param>
    /// <param name="nameToken">Token to use to localize the option name. Uses the config value if null/empty string is provided.</param>
    /// <param name="descriptionToken">Token to use to localize the description. Uses the config value if null/empty string is provided.</param>
    public static void AddOption(BaseOption option, string modGuid, string modName, string nameToken, string descriptionToken)
    {
        option.SetProperties();

        option.ModGuid = modGuid;
        option.ModName = modName;
        option.NameToken = nameToken;
        option.DescriptionToken = descriptionToken;
        option.Id = new OptionId(modGuid, option.Category, option.Name);

        // if (option is ChoiceOption choiceOption)
        // {
        //     choiceOption.RegisterChoiceTokens();
        // }
        option.RegisterTokens();
        
        if (option.BaseConfigItem is not null)
        {
            AutoGenerateConfigEntryIdBlacklist.Add(option.Id);
            Debug.Info($"Added {option.Id} to blacklist!");
        }
        
        OptionCollection.AddOption(ref option);
    }

    private static void EnsureContainerExists(string modGuid, string modName)
    {
        if (!OptionCollection.ContainsModGuid(modGuid))
            OptionCollection[modGuid] = new OptionCollection(modName, modGuid);
    }

    public static void SetCategoryNameToken(string modGuid, BaseOption option, string nameToken)
    {
        // We send in an option to get the category from it, as that is a good way to make sure the user doesn't
        // send in a string that does not exist.
        OptionCollection[modGuid][option.Category].SetNameToken(nameToken);
    }
}