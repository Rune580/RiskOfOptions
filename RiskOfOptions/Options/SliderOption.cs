using System;
using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.Config;
using RiskOfOptions.OptionConfigs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RiskOfOptions.Options;

public class SliderOption : BaseOption, IConfigItemOption<float>
{
    public IConfigItem<float> ConfigItem { get; }
    
    protected readonly SliderConfig config;
    
    [Obsolete]
    public SliderOption(ConfigEntry<float> configEntry) : this(configEntry, new SliderConfig()) { }
    
    [Obsolete]
    public SliderOption(ConfigEntry<float> configEntry, bool restartRequired) : this(configEntry, new SliderConfig { restartRequired = restartRequired }) { }

    [Obsolete]
    public SliderOption(ConfigEntry<float> configEntry, SliderConfig config) : this(new BepInExConfigItem<float>(configEntry), config) { }
    
    public SliderOption(IConfigItem<float> configItem) : this(configItem, new SliderConfig()) { }
    
    public SliderOption(IConfigItem<float> configItem, bool restartRequired) : this(configItem, new SliderConfig { restartRequired = restartRequired }) { }
    
    public SliderOption(IConfigItem<float> configItem, SliderConfig config)
    {
        ConfigItem = configItem;
        this.config = config;
    }

    public override IConfigItem BaseConfigItem => ConfigItem;

    public override GameObject CreateOptionGameObject(GameObject prefab, Transform parent)
    {
        GameObject slider = Object.Instantiate(prefab, parent);

        ModSettingsSlider settingsSlider = slider.GetComponentInChildren<ModSettingsSlider>();

        settingsSlider.nameToken = GetNameToken();
        settingsSlider.settingToken = Identifier;
            
        settingsSlider.minValue = config.min;
        settingsSlider.maxValue = config.max;
        settingsSlider.formatString = config.FormatString;
            
        slider.name = $"Mod Option Slider, {Name}";

        return slider;
    }

    public override BaseOptionConfig GetConfig() => config;

    public float DefaultValue => ConfigItem.DefaultValue;
    
    public virtual float Value
    {
        get => ConfigItem.Value;
        set => ConfigItem.Value = value;
    }
}