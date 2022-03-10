using System;
using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.OptionConfigs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RiskOfOptions.Options
{
    public class StepSliderOption : BaseOption, ITypedValue<float>
    {
        private readonly ConfigEntry<float> _configEntry;
        internal StepSliderConfig Config { get; }

        public StepSliderOption(ConfigEntry<float> configEntry) : this(configEntry, new StepSliderConfig()) { }

        public StepSliderOption(ConfigEntry<float> configEntry, StepSliderConfig config)
        {
            _configEntry = configEntry;
            Config = config;
            
            SetCategoryName(configEntry.Definition.Section, config);
            SetName(configEntry.Definition.Key, config);
            SetDescription(configEntry.Description.Description, config);
        }

        public override string OptionTypeName { get; protected set; } = "step_slider";
        
        public override GameObject CreateOptionGameObject(GameObject prefab, Transform parent)
        {
            GameObject stepSlider = Object.Instantiate(prefab, parent);
            
            ModSettingsStepSlider settingsSlider = stepSlider.GetComponentInChildren<ModSettingsStepSlider>();
            
            settingsSlider.nameToken = GetNameToken();
            settingsSlider.settingToken = Identifier;
            
            settingsSlider.increment = Config.increment;
            settingsSlider.minValue = Config.min;
            settingsSlider.maxValue = Config.max;
            settingsSlider.formatString = Config.formatString;
            
            double stepsHighAccuracy = Math.Abs(Config.min - Config.max) / Config.increment;
            
            int steps = (int)Math.Round(stepsHighAccuracy);
            
            settingsSlider.slider.minValue = 0;
            settingsSlider.slider.maxValue = steps;
            settingsSlider.slider.wholeNumbers = true;
            
            stepSlider.name = $"Mod Option Step Slider, {Name}";

            return stepSlider;
        }
        
        public override BaseOptionConfig GetConfig()
        {
            return Config;
        }

        public void SetValue(float value)
        {
            _configEntry.Value = value;
        }

        public float GetValue()
        {
            return _configEntry.Value;
        }
    }
}