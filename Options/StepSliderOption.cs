using System;
using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.OptionConfigs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RiskOfOptions.Options
{
    public class StepSliderOption : BaseOption, ITypedValueHolder<float>
    {
        private readonly float _originalValue;
        private readonly ConfigEntry<float> _configEntry;
        internal readonly StepSliderConfig Config;
        
        public StepSliderOption(ConfigEntry<float> configEntry) : this(configEntry, new StepSliderConfig()) { }

        public StepSliderOption(ConfigEntry<float> configEntry, bool restartRequired) : this(configEntry, new StepSliderConfig { restartRequired = restartRequired }) { }

        public StepSliderOption(ConfigEntry<float> configEntry, StepSliderConfig config)
        {
            _originalValue = configEntry.Value;
            _configEntry = configEntry;
            Config = config;
        }

        public override string OptionTypeName { get; protected set; } = "step_slider";
        
        internal override ConfigEntryBase ConfigEntry => _configEntry;
        
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

        public bool ValueChanged()
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return Value != GetOriginalValue();
        }

        public float GetOriginalValue()
        {
            return _originalValue;
        }

        public float Value
        {
            get => _configEntry.Value;
            set => _configEntry.Value = value;
        }
    }
}