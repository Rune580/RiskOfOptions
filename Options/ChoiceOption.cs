using System;
using BepInEx.Configuration;
using R2API;
using RiskOfOptions.Components.Options;
using RiskOfOptions.OptionConfigs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RiskOfOptions.Options
{
    public class ChoiceOption : BaseOption, ITypedValueHolder<object>
    {
        private readonly object _originalValue;
        private readonly ConfigEntryBase _configEntry;
        internal readonly ChoiceConfig Config;
        private string[] nameTokens;
        
        public ChoiceOption(ConfigEntryBase configEntry) : this(configEntry, new ChoiceConfig()) { }

        public ChoiceOption(ConfigEntryBase configEntry, bool restartRequired) : this(configEntry, new ChoiceConfig { restartRequired = restartRequired }) { }

        public ChoiceOption(ConfigEntryBase configEntry, ChoiceConfig config)
        {
            if (!configEntry.SettingType.IsEnum)
                throw new InvalidCastException($"T in configEntry<T> must be of type Enum, Type found: {configEntry.SettingType.Name}");

            _originalValue = configEntry.BoxedValue;
            _configEntry = configEntry;
            Config = config;
        }

        public override string OptionTypeName { get; protected set; } = "choice";

        internal override ConfigEntryBase ConfigEntry => _configEntry;
        
        internal override void RegisterTokens()
        {
            base.RegisterTokens();

            string[] names = Enum.GetNames(Value.GetType());

            nameTokens = new string[names.Length];

            for (int i = 0; i < names.Length; i++)
            {
                string token = $"{ModSettingsManager.StartingText}.{ModGuid}.{Category}.{Name}.{OptionTypeName}.item.{names[i]}".Replace(" ", "_").ToUpper();

                nameTokens[i] = token;
                
                LanguageAPI.Add(token, names[i]);
            }
        }

        public override GameObject CreateOptionGameObject(GameObject prefab, Transform parent)
        {
            GameObject button = Object.Instantiate(prefab, parent);

            DropDownController controller = button.GetComponentInChildren<DropDownController>();

            controller.nameToken = GetNameToken();
            controller.settingToken = Identifier;
            
            button.name = $"Mod Option Choice, {Name}";

            return button;
        }

        public override BaseOptionConfig GetConfig()
        {
            return Config;
        }

        public bool ValueChanged()
        {
            return !Value.Equals(_originalValue);
        }

        public object GetOriginalValue()
        {
            return _originalValue;
        }

        public object Value
        {
            get => _configEntry.BoxedValue;
            set => _configEntry.BoxedValue = Enum.Parse(_configEntry.SettingType, value.ToString());
        }

        internal string[] GetNameTokens()
        {
            return nameTokens;
        }
    }
}