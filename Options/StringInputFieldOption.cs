using System;
using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.OptionConfigs;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RiskOfOptions.Options
{
    public class StringInputFieldOption : BaseOption, ITypedValueHolder<string>
    {
        private readonly string _originalValue;
        private readonly ConfigEntry<string> _configEntry;
        internal readonly InputFieldConfig config;
        
        public StringInputFieldOption(ConfigEntry<string> configEntry) : this(configEntry, new InputFieldConfig()) { }

        public StringInputFieldOption(ConfigEntry<string> configEntry, bool restartRequired) : this(configEntry, new InputFieldConfig { restartRequired =  restartRequired }) { }

        public StringInputFieldOption(ConfigEntry<string> configEntry, InputFieldConfig config)
        {
            _originalValue = configEntry.Value;
            _configEntry = configEntry;
            this.config = config;
        }

        public override string OptionTypeName { get; protected set; } = "string_input_field";
        
        internal override ConfigEntryBase ConfigEntry => _configEntry;
        
        public override GameObject CreateOptionGameObject(GameObject prefab, Transform parent)
        {
            GameObject button = Object.Instantiate(prefab, parent);

            var controller = button.GetComponentInChildren<InputFieldController>();

            controller.nameToken = GetNameToken();
            controller.settingToken = Identifier;

            controller.characterValidation = TMP_InputField.CharacterValidation.None;
            
            button.name = $"Mod Option Input Field, {Name}";

            return button;
        }

        public override BaseOptionConfig GetConfig()
        {
            return config;
        }

        public bool ValueChanged()
        {
            return !string.Equals(_configEntry.Value, _originalValue, StringComparison.InvariantCulture);
        }

        public string GetOriginalValue()
        {
            return _originalValue;
        }

        public string Value
        {
            get => _configEntry.Value;
            set => _configEntry.Value = value;
        }
    }
}