using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.OptionConfigs;
using UnityEngine;

namespace RiskOfOptions.Options
{
    public class ColorOption : BaseOption, ITypedValueHolder<Color>
    {
        private readonly Color _originalValue;
        private readonly ConfigEntry<Color> _configEntry;
        internal readonly ColorOptionConfig config;
        
        public ColorOption(ConfigEntry<Color> configEntry) : this(configEntry, new ColorOptionConfig()) { }
        
        public ColorOption(ConfigEntry<Color> configEntry, bool restartRequired) : this(configEntry, new ColorOptionConfig { restartRequired = true }) { }

        public ColorOption(ConfigEntry<Color> configEntry, ColorOptionConfig config)
        {
            _originalValue = configEntry.Value;
            _configEntry = configEntry;
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
            return _originalValue;
        }

        public Color Value
        {
            get => _configEntry.Value;
            set => _configEntry.Value = value;
        }
    }
}