using System;
using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.Lib;
using RiskOfOptions.OptionConfigs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RiskOfOptions.Options
{
    public class ChoiceOption : BaseOption, ITypedValueHolder<object>
    {
        protected readonly object originalValue;
        private readonly ConfigEntryBase _configEntry;
        protected readonly ChoiceConfig config;
        private string[] _nameTokens;
        
        public ChoiceOption(ConfigEntryBase configEntry) : this(configEntry, new ChoiceConfig()) { }

        public ChoiceOption(ConfigEntryBase configEntry, bool restartRequired) : this(configEntry, new ChoiceConfig { restartRequired = restartRequired }) { }

        public ChoiceOption(ConfigEntryBase configEntry, ChoiceConfig config) : this(config, configEntry.BoxedValue)
        {
            if (!configEntry.SettingType.IsEnum)
                throw new InvalidCastException($"T in configEntry<T> must be of type Enum, Type found: {configEntry.SettingType.Name}");
            _configEntry = configEntry;
        }
        protected ChoiceOption(ChoiceConfig config, object originalValue)
        {
            this.originalValue = originalValue;
            this.config = config;
        }

        public override string OptionTypeName { get; protected set; } = "choice";

        internal override ConfigEntryBase ConfigEntry => _configEntry;
        
        internal override void RegisterTokens()
        {
            base.RegisterTokens();
            RegisterChoiceTokens();
        }

        public void RegisterChoiceTokens()
        {
            string[] names = Enum.GetNames(Value.GetType());

            _nameTokens = new string[names.Length];

            for (int i = 0; i < names.Length; i++)
            {
                string token = $"{ModSettingsManager.StartingText}.{ModGuid}.{Category}.{Name}.{OptionTypeName}.item.{names[i]}".Replace(" ", "_").ToUpper();

                _nameTokens[i] = token;
                
                LanguageApi.Add(token, names[i]);
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
            return config;
        }

        public bool ValueChanged()
        {
            return !Value.Equals(originalValue);
        }

        public object GetOriginalValue()
        {
            return originalValue;
        }

        public virtual object Value
        {
            get => _configEntry.BoxedValue;
            set => _configEntry.BoxedValue = Enum.Parse(_configEntry.SettingType, value.ToString());
        }

        internal string[] GetNameTokens()
        {
            return _nameTokens;
        }
    }
}