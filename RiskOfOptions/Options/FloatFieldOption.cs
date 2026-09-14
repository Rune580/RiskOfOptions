using System;
using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.Config;
using RiskOfOptions.OptionConfigs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RiskOfOptions.Options;

public class FloatFieldOption : BaseOption, IConfigItemOption<float>
{
    public IConfigItem<float> ConfigItem { get; }

    public float InitialValue { get; }

    protected readonly FloatFieldConfig config;
    
    [Obsolete]
    public FloatFieldOption(ConfigEntry<float> configEntry) : this(configEntry, new FloatFieldConfig()) { }
    
    [Obsolete]
    public FloatFieldOption(ConfigEntry<float> configEntry, bool restartRequired) : this(configEntry, new FloatFieldConfig { restartRequired = restartRequired }) { }
    
    [Obsolete]
    public FloatFieldOption(ConfigEntry<float> configEntry, FloatFieldConfig config) : this(new BepInExConfigItem<float>(configEntry), config) { }
    
    public FloatFieldOption(IConfigItem<float> configItem) : this(configItem, new FloatFieldConfig()) { }
        
    public FloatFieldOption(IConfigItem<float> configItem, bool restartRequired) : this(configItem, new FloatFieldConfig { restartRequired = restartRequired }) { }
    
    public FloatFieldOption(IConfigItem<float> configItem, FloatFieldConfig config)
    {
        ConfigItem = configItem;
        this.config = config;

        InitialValue = ConfigItem.Value;
    }

    public override IConfigItem BaseConfigItem => ConfigItem;

    public override GameObject CreateOptionGameObject(GameObject prefab, Transform parent)
    {
        var floatField = Object.Instantiate(prefab, parent);

        var settingsField = floatField.GetComponentInChildren<ModSettingsFloatField>();

        settingsField.nameToken = GetNameToken();
        settingsField.optionId = Id;

        settingsField.min = config.Min;
        settingsField.max = config.Max;
        settingsField.formatString = config.FormatString;

        settingsField.name = $"Mod Options Float Field, {Name}";

        return floatField;
    }

    public override BaseOptionConfig GetConfig() => config;
    
    public float DefaultValue => ConfigItem.DefaultValue;
    
    public virtual float Value
    {
        get => ConfigItem.Value;
        set => ConfigItem.Value = value;
    }
}