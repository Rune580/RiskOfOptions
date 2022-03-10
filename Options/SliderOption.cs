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
        internal SliderConfig Config { get; }
        
        public SliderOption(ConfigEntry<float> configEntry) : this(configEntry, new SliderConfig()) { }

        public SliderOption(ConfigEntry<float> configEntry, SliderConfig config)
        {
            _originalValue = configEntry.Value;
            _configEntry = configEntry;
            Config = config;
            
            SetCategoryName(configEntry.Definition.Section, config);
            SetName(configEntry.Definition.Key, config);
            SetDescription(configEntry.Description.Description, config);
        }

        public override string OptionTypeName { get; protected set; } = "slider";
        
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

        public override bool ValueChanged()
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return GetValue() != GetOriginalValue();
        }

        public void SetValue(float value)
        {
            _configEntry.Value = value;
        }

        public float GetValue()
        {
            return _configEntry.Value;
        }

        public float GetOriginalValue()
        {
            return _originalValue;
        }
    }
}