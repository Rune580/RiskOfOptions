using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.OptionConfigs;
using UnityEngine;

namespace RiskOfOptions.Options
{
    public class ColorOption : BaseOption, ITypedValueHolder<Color>
    {
        protected readonly Color originalValue;
        private readonly ConfigEntry<Color> _configEntry;
        protected readonly ColorOptionConfig config;
        
        public ColorOption(ConfigEntry<Color> configEntry) : this(configEntry, new ColorOptionConfig()) { }
        
        public ColorOption(ConfigEntry<Color> configEntry, bool restartRequired) : this(configEntry, new ColorOptionConfig { restartRequired = true }) { }

        public ColorOption(ConfigEntry<Color> configEntry, ColorOptionConfig config) : this(config, configEntry.Value)
        {
            _configEntry = configEntry;
        }

        protected ColorOption(ColorOptionConfig config, Color originalValue)
        {
            this.originalValue = originalValue;
            this.config = config;
        }

        public override string OptionTypeName { get; protected set; } = "color";

        internal override ConfigEntryBase ConfigEntry => _configEntry;

        public override GameObject CreateOptionGameObject(GameObject prefab, Transform parent)
        {
            GameObject button = Object.Instantiate(prefab, parent);

            var controller = button.GetComponentInChildren<ModSettingsColor>();

            controller.nameToken = GetNameToken();
            controller.settingToken = Identifier;

            button.name = $"Mod Option Color, {Name}";

            return button;
        }

        public override BaseOptionConfig GetConfig()
        {
            return config;
        }

        public bool ValueChanged()
        {
            return Value != GetOriginalValue();
        }

        public Color GetOriginalValue()
        {
            return originalValue;
        }

        public virtual Color Value
        {
            get => _configEntry.Value;
            set => _configEntry.Value = value;
        }
    }
}