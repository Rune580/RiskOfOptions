using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.OptionConfigs;
using UnityEngine;

namespace RiskOfOptions.Options
{
    public class CheckBoxOption : BaseOption, ITypedValueHolder<bool>
    {
        protected readonly bool originalValue;
        private readonly ConfigEntry<bool> _configEntry;
        protected readonly CheckBoxConfig config;

        public CheckBoxOption(ConfigEntry<bool> configEntry) : this(configEntry, new CheckBoxConfig()) { }
        
        public CheckBoxOption(ConfigEntry<bool> configEntry, bool restartRequired) : this(configEntry, new CheckBoxConfig { restartRequired = restartRequired }) { }

        public CheckBoxOption(ConfigEntry<bool> configEntry, CheckBoxConfig config) : this(config, configEntry.Value)
        {
            _configEntry = configEntry;
        }

        protected CheckBoxOption(CheckBoxConfig config, bool originalValue)
        {
            this.originalValue = originalValue;
            this.config = config;
        }
        
        public override string OptionTypeName { get; protected set; } = "checkbox";
        
        internal override ConfigEntryBase ConfigEntry => _configEntry;

        public override GameObject CreateOptionGameObject(GameObject prefab, Transform parent)
        {
            GameObject button = Object.Instantiate(prefab, parent);

            var controller = button.GetComponentInChildren<ModSettingsBool>();

            controller.nameToken = GetNameToken();
            controller.settingToken = Identifier;
            
            button.name = $"Mod Option CheckBox, {Name}";

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

        public bool GetOriginalValue()
        {
            return originalValue;
        }

        public virtual bool Value
        {
            get => _configEntry.Value; 
            set => _configEntry.Value = value;
        }
    }
}