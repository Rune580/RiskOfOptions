using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.OptionConfigs;
using UnityEngine;

namespace RiskOfOptions.Options
{
    public class SliderOption : BaseOption, ITypedValueHolder<float>
    {
        protected readonly float originalValue;
        private readonly ConfigEntry<float> _configEntry;
        protected readonly SliderConfig config;
        
        public SliderOption(ConfigEntry<float> configEntry) : this(configEntry, new SliderConfig()) { }
        
        public SliderOption(ConfigEntry<float> configEntry, bool restartRequired) : this(configEntry, new SliderConfig { restartRequired = restartRequired }) { }

        public SliderOption(ConfigEntry<float> configEntry, SliderConfig config) : this(config, configEntry.Value)
        {
            _configEntry = configEntry;
        }
        protected SliderOption(SliderConfig config, float originalValue)
        {
            this.originalValue = originalValue;
            this.config = config;
        }

        public override string OptionTypeName { get; protected set; } = "slider";
        
        internal override ConfigEntryBase ConfigEntry => _configEntry;
        
        public override GameObject CreateOptionGameObject(GameObject prefab, Transform parent)
        {
            GameObject slider = Object.Instantiate(prefab, parent);

            ModSettingsSlider settingsSlider = slider.GetComponentInChildren<ModSettingsSlider>();

            settingsSlider.nameToken = GetNameToken();
            settingsSlider.settingToken = Identifier;
            
            settingsSlider.minValue = config.min;
            settingsSlider.maxValue = config.max;
            settingsSlider.formatString = config.formatString;
            
            slider.name = $"Mod Option Slider, {Name}";

            return slider;
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