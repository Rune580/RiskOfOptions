using System;
using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.Config;
using RiskOfOptions.OptionConfigs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RiskOfOptions.Options;

public class IntSliderOption : BaseOption, IConfigItemOption<int>
{
    public IConfigItem<int> ConfigItem { get; }

    public int InitialValue { get; }

    protected readonly IntSliderConfig config;
    
    [Obsolete]
    public IntSliderOption(ConfigEntry<int> configEntry) : this(configEntry, new IntSliderConfig()) { }
    
    [Obsolete]
    public IntSliderOption(ConfigEntry<int> configEntry, bool restartRequired) : this(configEntry, new IntSliderConfig { restartRequired = restartRequired }) { }
    
    [Obsolete]
    public IntSliderOption(ConfigEntry<int> configEntry, IntSliderConfig config) : this(new BepInExConfigItem<int>(configEntry), config) { }
    
    public IntSliderOption(IConfigItem<int> configItem) : this(configItem, new IntSliderConfig()) { }
        
    public IntSliderOption(IConfigItem<int> configItem, bool restartRequired) : this(configItem, new IntSliderConfig { restartRequired = restartRequired }) { }
    
    public IntSliderOption(IConfigItem<int> configItem, IntSliderConfig config)
    {
        ConfigItem = configItem;
        this.config = config;

        InitialValue = ConfigItem.Value;
    }

    public override IConfigItem BaseConfigItem => ConfigItem;

    public override GameObject CreateOptionGameObject(GameObject prefab, Transform parent)
    {
        GameObject intSlider = Object.Instantiate(prefab, parent);

        ModSettingsIntSlider settingsSlider = intSlider.GetComponentInChildren<ModSettingsIntSlider>();

        settingsSlider.nameToken = GetNameToken();
        settingsSlider.optionId = Id;

        settingsSlider.minValue = config.min;
        settingsSlider.maxValue = config.max;
        settingsSlider.formatString = config.formatString;

        intSlider.name = $"Mod Options Int Slider, {Name}";

        return intSlider;
    }

    public override BaseOptionConfig GetConfig() => config;
    
    public int DefaultValue => ConfigItem.DefaultValue;
    
    public virtual int Value
    {
        get => ConfigItem.Value;
        set => ConfigItem.Value = value;
    }
}