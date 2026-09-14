using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using MonoMod.RuntimeDetour;
using RiskOfOptions.Containers;
using RiskOfOptions.Lib;
using RiskOfOptions.OptionConfigs;
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

    private static readonly HashSet<string> AutoGenerateModGuidBlacklist = [];
    private static readonly HashSet<string> AutoGenerateConfigEntryIdBlacklist = [];
    private static bool _autoGenerationComplete;

    internal const string StartingText = "RISK_OF_OPTIONS";
    internal const int StartingTextLength = 15;

    internal static bool disablePause = false;

    internal static readonly List<string> RestartRequiredOptions = [];

    internal static void Init()
    {
        LanguageApi.Init();

        Resources.Assets.LoadAssets();
        Resources.Prefabs.Init();

        LanguageTokens.Register();

        SettingsModifier.Init();

        var targetMethod = typeof(PauseManager).GetMethod("CCTogglePause", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        var destMethod = typeof(ModSettingsManager).GetMethod(nameof(PauseManagerOnCCTogglePause), BindingFlags.NonPublic | BindingFlags.Static);
        _pauseHook ??= new Hook(targetMethod, destMethod);
    }

    internal static void AutoGenerateFromConfigs()
    {
        if (_autoGenerationComplete)
            return;
        
        Debug.Info("Starting config auto-generation");
        
        foreach (var (modGuid, pluginInfo) in Chainloader.PluginInfos)
        {
            if (AutoGenerateModGuidBlacklist.Contains(modGuid))
                continue;

            foreach (var (_, configEntryBase) in pluginInfo.Instance.Config)
            {
                var uniqueId = $"{modGuid}.{configEntryBase.Definition.Section}.{configEntryBase.Definition.Key}"
                    .Replace(" ", "_")
                    .ToUpper();

                if (AutoGenerateConfigEntryIdBlacklist.Contains(uniqueId))
                    continue;

                var metadata = pluginInfo.Metadata;

                if (configEntryBase is ConfigEntry<bool> boolConfigEntry)
                {
                    AddOption(new CheckBoxOption(boolConfigEntry, true), metadata.GUID, metadata.Name);
                }
                else if (configEntryBase is ConfigEntry<float> floatConfigEntry)
                {
                    if (floatConfigEntry.Description.AcceptableValues is AcceptableValueRange<float> floatRange)
                    {
                        AddOption(new SliderOption(floatConfigEntry, new SliderConfig
                        {
                            min = floatRange.MinValue,
                            max = floatRange.MaxValue,
                            restartRequired = true
                        }), metadata.GUID, metadata.Name);
                    }
                    else
                    {
                        AddOption(new FloatFieldOption(floatConfigEntry, true), metadata.GUID, metadata.Name);
                    }
                }
                else if (configEntryBase is ConfigEntry<int> intConfigEntry)
                {
                    if (intConfigEntry.Description.AcceptableValues is AcceptableValueRange<int> intRange)
                    {
                        AddOption(new IntSliderOption(intConfigEntry, new IntSliderConfig
                        {
                            min = intRange.MinValue,
                            max = intRange.MaxValue,
                            restartRequired = true,
                        }), metadata.GUID, metadata.Name);
                    }
                    else
                    {
                        AddOption(new IntFieldOption(intConfigEntry, true), metadata.GUID, metadata.Name);
                    }
                }
                else if (configEntryBase is ConfigEntry<KeyboardShortcut> keyConfigEntry)
                {
                    AddOption(new KeyBindOption(keyConfigEntry, true), metadata.GUID, metadata.Name);
                }
                else if (configEntryBase is ConfigEntry<Color> colorConfigEntry)
                {
                    AddOption(new ColorOption(colorConfigEntry, true), metadata.GUID, metadata.Name);
                }
                else if (configEntryBase is ConfigEntry<string> stringConfigEntry)
                {
                    AddOption(new StringInputFieldOption(stringConfigEntry, true), metadata.GUID, metadata.Name);
                }
                else if (configEntryBase.SettingType.IsEnum)
                {
                    AddOption(new ChoiceOption(configEntryBase, true), metadata.GUID, metadata.Name);
                }
            }
            
            var searchDir = System.IO.Path.GetFullPath(pluginInfo.Location);
            var parent = Directory.GetParent(searchDir);
            while (parent is not null && !string.Equals(parent.Name, "plugins", StringComparison.OrdinalIgnoreCase))
            {
                searchDir = parent.FullName;
                parent = Directory.GetParent(searchDir);
            }
            
            if (OptionCollection.TryGetCollection(pluginInfo.Metadata.GUID, out var collection))
            {
                // Set mod icon if it has not been set yet.
                if (collection.icon is null && collection.iconPrefab is null)
                {
                    var iconPath = Directory.EnumerateFiles(searchDir, "icon.png", SearchOption.AllDirectories).FirstOrDefault();
                    if (iconPath is not null)
                    {
                        var texture = new Texture2D(256, 256);
                        if (texture.LoadImage(File.ReadAllBytes(iconPath)) && texture)
                        {
                            collection.icon = Sprite.Create(
                                texture,
                                new Rect(0, 0, texture.width, texture.height),
                                new Vector2(0.5f, 0.5f),
                                100
                            );
                        }
                    }
                }
                
                // Todo: Set description as well
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