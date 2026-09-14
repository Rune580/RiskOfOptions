using System;
using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.Config;
using RiskOfOptions.OptionConfigs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RiskOfOptions.Options;

public class IntFieldOption : BaseOption, IConfigItemOption<int>
{
    public IConfigItem<int> ConfigItem { get; }

    public int InitialValue { get; }

    protected readonly IntFieldConfig config;

    [Obsolete]
    public IntFieldOption(ConfigEntry<int> configEntry) : this(configEntry, new IntFieldConfig()) { }
    
    [Obsolete]
    public IntFieldOption(ConfigEntry<int> configEntry, bool restartRequired) : this(configEntry, new IntFieldConfig { restartRequired = restartRequired }) { }
    
    [Obsolete]
    public IntFieldOption(ConfigEntry<int> configEntry, IntFieldConfig config) : this(new BepInExConfigItem<int>(configEntry), config) { }
    
    public IntFieldOption(IConfigItem<int> configItem) : this(configItem, new IntFieldConfig()) { }
        
    public IntFieldOption(IConfigItem<int> configItem, bool restartRequired) : this(configItem, new IntFieldConfig { restartRequired = restartRequired }) { }
    
    public IntFieldOption(IConfigItem<int> configItem, IntFieldConfig config)
    {
        ConfigItem = configItem;
        this.config = config;

        InitialValue = ConfigItem.Value;
    }

    public override IConfigItem BaseConfigItem => ConfigItem;

    public override GameObject CreateOptionGameObject(GameObject prefab, Transform parent)
    {
        var intField = Object.Instantiate(prefab, parent);

        var settingsField = intField.GetComponentInChildren<ModSettingsIntField>();

        settingsField.nameToken = GetNameToken();
        settingsField.optionId = Id;

        settingsField.min = config.Min;
        settingsField.max = config.Max;
        settingsField.formatString = config.FormatString;

        settingsField.name = $"Mod Options Int Field, {Name}";

        return intField;
    }

    public override BaseOptionConfig GetConfig() => config;
    
    public int DefaultValue => ConfigItem.DefaultValue;
    
    public virtual int Value
    {
        get => ConfigItem.Value;
        set => ConfigItem.Value = value;
    }
    
}