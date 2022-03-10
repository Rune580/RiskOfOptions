using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.OptionConfigs;
using RoR2.UI;
using UnityEngine;

namespace RiskOfOptions.Options
{
    public class KeyBindOption : BaseOption, ITypedValueHolder<KeyboardShortcut>
    {
        private readonly KeyboardShortcut _originalValue;
        private readonly ConfigEntry<KeyboardShortcut> _configEntry;
        internal KeyBindConfig Config { get; }

        public KeyBindOption(ConfigEntry<KeyboardShortcut> configEntry) : this(configEntry, new KeyBindConfig()) { }
        
        public KeyBindOption(ConfigEntry<KeyboardShortcut> configEntry, KeyBindConfig config)
        {
            _originalValue = configEntry.Value;
            _configEntry = configEntry;
            Config = config;

            SetCategoryName(configEntry.Definition.Section, config);
            SetName(configEntry.Definition.Key, config);
            SetDescription(configEntry.Description.Description, config);
        }

        public override string OptionTypeName { get; protected set; } = "key_bind";
        
        public override GameObject CreateOptionGameObject(GameObject prefab, Transform parent)
        {
            GameObject keyBind = Object.Instantiate(prefab, parent);

            KeyBindController controller = keyBind.GetComponentInChildren<KeyBindController>();

            controller.nameToken = GetNameToken();
            controller.settingToken = Identifier;
            
            keyBind.transform.Find("ButtonText").GetComponent<HGTextMeshProUGUI>().SetText(Name);
            keyBind.name = $"Mod Option KeyBind, {Name}";

            foreach (var button in keyBind.GetComponentsInChildren<HGButton>())
                button.onClick.AddListener(delegate { controller.StartListening(); });

            return keyBind;
        }

        public override BaseOptionConfig GetConfig()
        {
            return Config;
        }

        public override bool ValueChanged()
        {
            return !GetValue().Equals(GetOriginalValue());
        }

        public void SetValue(KeyboardShortcut value)
        {
            _configEntry.Value = value;
        }

        public KeyboardShortcut GetValue()
        {
            return _configEntry.Value;
        }

        public KeyboardShortcut GetOriginalValue()
        {
            return _originalValue;
        }
    }
}