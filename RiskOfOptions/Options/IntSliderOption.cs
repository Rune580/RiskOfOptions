﻿using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.OptionConfigs;
using UnityEngine;

namespace RiskOfOptions.Options
{
    public class IntSliderOption : BaseOption, ITypedValueHolder<int>
    {
        protected readonly int originalValue;
        private readonly ConfigEntry<int> _configEntry;
        protected readonly IntSliderConfig config;
        
        public IntSliderOption(ConfigEntry<int> configEntry) : this(configEntry, new IntSliderConfig()) { }
        
        public IntSliderOption(ConfigEntry<int> configEntry, bool restartRequired) : this(configEntry, new IntSliderConfig { restartRequired = restartRequired }) { }

        public IntSliderOption(ConfigEntry<int> configEntry, IntSliderConfig config) : this(config, configEntry.Value)
        {
            _configEntry = configEntry;
        }
        protected IntSliderOption(IntSliderConfig config, int originalValue)
        {
            this.originalValue = originalValue;
            this.config = config;
        }

        public override string OptionTypeName { get; protected set; } = "int_slider";

        internal override ConfigEntryBase ConfigEntry => _configEntry;

        public override GameObject CreateOptionGameObject(GameObject prefab, Transform parent)
        {
            GameObject intSlider = Object.Instantiate(prefab, parent);

            ModSettingsIntSlider settingsSlider = intSlider.GetComponentInChildren<ModSettingsIntSlider>();

            settingsSlider.nameToken = GetNameToken();
            settingsSlider.settingToken = Identifier;

            settingsSlider.minValue = config.min;
            settingsSlider.maxValue = config.max;
            settingsSlider.formatString = config.formatString;

            intSlider.name = $"Mod Options Int Slider, {Name}";

            return intSlider;
        }

        public override BaseOptionConfig GetConfig()
        {
            return config;
        }

        public bool ValueChanged()
        {
            return Value != GetOriginalValue();
        }

        public int GetOriginalValue()
        {
            return originalValue;
        }

        public virtual int Value
        {
            get => _configEntry.Value;
            set => _configEntry.Value = value;
        }
    }
}