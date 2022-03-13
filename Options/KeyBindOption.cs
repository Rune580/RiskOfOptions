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
        internal readonly KeyBindConfig Config;
        
        public KeyBindOption(ConfigEntry<KeyboardShortcut> configEntry) : this(configEntry, new KeyBindConfig()) { }
        
        public KeyBindOption(ConfigEntry<KeyboardShortcut> configEntry, bool restartRequired) : this(configEntry, new KeyBindConfig { restartRequired = restartRequired }) { }

        public KeyBindOption(ConfigEntry<KeyboardShortcut> configEntry, KeyBindConfig config)
        {
            _originalValue = configEntry.Value;
            _configEntry = configEntry;
            Config = config;
        }

        public override string OptionTypeName { get; protected set; } = "key_bind";

        internal override ConfigEntryBase ConfigEntry => _configEntry;

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

        public bool ValueChanged()
        {
            return !Value.Equals(GetOriginalValue());
        }

        public KeyboardShortcut GetOriginalValue()
        {
            return _originalValue;
        }

        public KeyboardShortcut Value
        {
            get => _configEntry.Value;
            set => _configEntry.Value = value;
        }
    }
}