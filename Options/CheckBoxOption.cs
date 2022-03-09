using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.OptionConfigs;
using UnityEngine;

namespace RiskOfOptions.Options
{
    public class CheckBoxOption : BaseOption, ITypedValue<bool>
    {
        private readonly ConfigEntry<bool> _configEntry;
        internal CheckBoxConfig Config { get; }

        public CheckBoxOption(ConfigEntry<bool> configEntry) : this(configEntry, new CheckBoxConfig()) { }

        public CheckBoxOption(ConfigEntry<bool> configEntry, CheckBoxConfig config)
        {
            _configEntry = configEntry;
            Config = config;
            
            SetCategoryName(configEntry.Definition.Section, config);
            SetName(configEntry.Definition.Key, config);
            SetDescription(configEntry.Definition.Section, config);
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

        public void SetValue(bool value)
        {
            _configEntry.Value = value;
        }

        public bool GetValue()
        {
            return _configEntry.Value;
        }
    }
}