using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.OptionConfigs;
using UnityEngine;

namespace RiskOfOptions.Options
{
    public class SliderOption : BaseOption, ITypedValueHolder<float>
    {
        private readonly float _originalValue;
        private readonly ConfigEntry<float> _configEntry;
        internal readonly SliderConfig Config;
        
        public SliderOption(ConfigEntry<float> configEntry) : this(configEntry, new SliderConfig()) { }
        
        public SliderOption(ConfigEntry<float> configEntry, bool restartRequired) : this(configEntry, new SliderConfig { restartRequired = restartRequired }) { }
        
        public SliderOption(ConfigEntry<float> configEntry, SliderConfig config)
        {
            _originalValue = configEntry.Value;
            _configEntry = configEntry;
            Config = config;
        }

        public override string OptionTypeName { get; protected set; } = "slider";
        
        internal override ConfigEntryBase ConfigEntry => _configEntry;
        
        public override GameObject CreateOptionGameObject(GameObject prefab, Transform parent)
        {
            GameObject slider = Object.Instantiate(prefab, parent);

            ModSettingsSlider settingsSlider = slider.GetComponentInChildren<ModSettingsSlider>();

            settingsSlider.nameToken = GetNameToken();
            settingsSlider.settingToken = Identifier;
            
            settingsSlider.minValue = Config.min;
            settingsSlider.maxValue = Config.max;
            settingsSlider.formatString = Config.formatString;
            
            slider.name = $"Mod Option Slider, {Name}";

            return slider;
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