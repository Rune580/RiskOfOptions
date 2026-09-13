using System;
using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.Config;
using RiskOfOptions.OptionConfigs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RiskOfOptions.Options;

public class StepSliderOption : BaseOption, IConfigItemOption<float>
{
    public IConfigItem<float> ConfigItem { get; }
    
    protected readonly StepSliderConfig config;
    
    [Obsolete]
    public StepSliderOption(ConfigEntry<float> configEntry) : this(configEntry, new StepSliderConfig()) { }
    
    [Obsolete]
    public StepSliderOption(ConfigEntry<float> configEntry, bool restartRequired) : this(configEntry, new StepSliderConfig { restartRequired = restartRequired }) { }
    
    [Obsolete]
    public StepSliderOption(ConfigEntry<float> configEntry, StepSliderConfig config) : this(new BepInExConfigItem<float>(configEntry), config) { }
    
    public StepSliderOption(IConfigItem<float> configItem) : this(configItem, new StepSliderConfig()) { }

    public StepSliderOption(IConfigItem<float> configItem, bool restartRequired) : this(configItem, new StepSliderConfig { restartRequired = restartRequired }) { }
    
    public StepSliderOption(IConfigItem<float> configItem, StepSliderConfig config)
    {
        ConfigItem = configItem;
        this.config = config;
    }

    public override IConfigItem BaseConfigItem => ConfigItem;

    public override GameObject CreateOptionGameObject(GameObject prefab, Transform parent)
    {
        GameObject stepSlider = Object.Instantiate(prefab, parent);
            
        ModSettingsStepSlider settingsSlider = stepSlider.GetComponentInChildren<ModSettingsStepSlider>();
            
        settingsSlider.nameToken = GetNameToken();
        settingsSlider.settingToken = Identifier;
            
        settingsSlider.increment = config.increment;
        settingsSlider.minValue = config.min;
        settingsSlider.maxValue = config.max;
        settingsSlider.remapManualInputToStep = config.remapManualInputToStep;
        settingsSlider.formatString = config.FormatString;
            
        double stepsHighAccuracy = Math.Abs(config.min - config.max) / config.increment;
            
        int steps = (int)Math.Round(stepsHighAccuracy);
            
        settingsSlider.slider.minValue = 0;
        settingsSlider.slider.maxValue = steps;
        settingsSlider.slider.wholeNumbers = true;
            
        stepSlider.name = $"Mod Option Step Slider, {Name}";

        return stepSlider;
    }
        
    public override BaseOptionConfig GetConfig() => config;

    public float DefaultValue => ConfigItem.DefaultValue;
    
    public virtual float Value
    {
        get => ConfigItem.Value;
        set => ConfigItem.Value = value;
    }
}