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
        protected readonly float originalValue;
        private readonly ConfigEntry<float> _configEntry;
        protected readonly StepSliderConfig config;
        
        public StepSliderOption(ConfigEntry<float> configEntry) : this(configEntry, new StepSliderConfig()) { }

        public StepSliderOption(ConfigEntry<float> configEntry, bool restartRequired) : this(configEntry, new StepSliderConfig { restartRequired = restartRequired }) { }

        public StepSliderOption(ConfigEntry<float> configEntry, StepSliderConfig config) : this(config, configEntry.Value)
        {
            _configEntry = configEntry;
        }

        protected StepSliderOption(StepSliderConfig config, float originalValue)
        {
            this.originalValue = originalValue;
            this.config = config;
        }

        public override string OptionTypeName { get; protected set; } = "step_slider";
        
        internal override ConfigEntryBase ConfigEntry => _configEntry;
        
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
        
        public override BaseOptionConfig GetConfig()
        {
            return config;
        }

        public bool ValueChanged()
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return Value != GetOriginalValue();
        }

        public float GetOriginalValue()
        {
            return originalValue;
        }

        public virtual float Value
        {
            get => _configEntry.Value;
            set => _configEntry.Value = value;
        }
    }
}