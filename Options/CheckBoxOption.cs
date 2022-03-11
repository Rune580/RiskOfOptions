using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.OptionConfigs;
using UnityEngine;

namespace RiskOfOptions.Options
{
    public class CheckBoxOption : BaseOption, ITypedValueHolder<bool>
    {
        private readonly bool _originalValue;
        private readonly ConfigEntry<bool> _configEntry;
        private CheckBoxConfig Config { get; }

        public CheckBoxOption(ConfigEntry<bool> configEntry) : this(configEntry, new CheckBoxConfig()) { }
        
        public CheckBoxOption(ConfigEntry<bool> configEntry, bool restartRequired) : this(configEntry, new CheckBoxConfig { restartRequired = restartRequired }) { }

        public CheckBoxOption(ConfigEntry<bool> configEntry, CheckBoxConfig config)
        {
            _originalValue = configEntry.Value;
            _configEntry = configEntry;
            Config = config;

            SetCategoryName(configEntry.Definition.Section, config);
            SetName(configEntry.Definition.Key, config);
            SetDescription(configEntry.Description.Description, config);
        }
        
        public override string OptionTypeName { get; protected set; } = "checkbox";

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
            return Config;
        }

        public override bool ValueChanged()
        {
            return GetValue() != GetOriginalValue();
        }

        public void SetValue(bool value)
        {
            _configEntry.Value = value;
        }

        public bool GetValue()
        {
            return _configEntry.Value;
        }

        public bool GetOriginalValue()
        {
            return _originalValue;
        }
    }
}