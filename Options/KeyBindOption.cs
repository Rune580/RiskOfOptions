using System;
using System.Collections.Generic;
using System.Text;
using BepInEx.Configuration;
using R2API.Utils;
using RiskOfOptions.Components.OptionComponents;
using RiskOfOptions.Components.Options;
using RiskOfOptions.Events;
using RiskOfOptions.Interfaces;
using RiskOfOptions.OptionOverrides;
using RoR2.UI;
using UnityEngine;
using UnityEngine.Events;

namespace RiskOfOptions.Options
{
    public class KeyBindOption : RiskOfOption, IKeyCodeProvider
    {
        public KeyCodeEvent OnValueChanged { get; set; } = new KeyCodeEvent();

        private KeyCode _value;

        internal ConfigEntry<KeyboardShortcut> ConfigEntry;

        internal KeyBindOption(string modGuid, string modName, string name, object[] description, string defaultValue, string categoryName, bool visibility, UnityAction<KeyCode> unityAction, bool invokeEventOnStart)
            : base(modGuid, modName, name, description, defaultValue, categoryName, null, visibility, false, invokeEventOnStart)
        {
            Value = (KeyCode)int.Parse(defaultValue);

            if (unityAction != null)
            {
                OnValueChanged.AddListener(unityAction);
            }
            
            OptionToken = $"{ModSettingsManager.StartingText}.{modGuid}.{modName}.category_{categoryName.Replace(".", "")}.{name}.keybind".ToUpper().Replace(" ", "_");

            RegisterTokens();
        }

        public KeyCode Value
        {
            get => _value;
            set
            {
                if (_value == value)
                    return;

                _value = value;

                if (ConfigEntry != null)
                {
                    ConfigEntry.Value = new KeyboardShortcut(Value);
                }

                OnValueChanged.Invoke(Value);
            }
        }

        public override GameObject CreateOptionGameObject(RiskOfOption option, GameObject prefab, Transform parent)
        {
            GameObject button = GameObject.Instantiate(prefab, parent);

            var keyBindController = button.GetComponentInChildren<KeybindController>();

            keyBindController.settingName = option.ConsoleToken;
            keyBindController.nameToken = option.NameToken;

            keyBindController.settingSource = RooSettingSource;

            button.transform.Find("ButtonText").GetComponent<HGTextMeshProUGUI>().SetText(option.Name);

            button.name = $"Mod Option KeyBind, {option.Name}";

            foreach (var hgButton in button.GetComponentsInChildren<HGButton>())
            {
                hgButton.onClick.RemoveAllListeners();

                var kbController = keyBindController;
                hgButton.onClick.AddListener(new UnityAction(delegate ()
                {
                    kbController.StartListening();
                }));
            }

            return button;
        }

        public override string GetValueAsString()
        {
            return $"{(int)Value}";
        }

        public override string GetInternalValueAsString()
        {
            return $"{(int)_value}";
        }

        public override void SetValue(string newValue)
        {
            Value = (KeyCode)int.Parse(newValue);
        }

        public override void SetInternalValue(string newValue)
        {
            _value = (KeyCode)int.Parse(newValue);
        }

        public override T GetValue<T>()
        {
            if (typeof(T) != typeof(KeyCode))
            {
                throw new Exception($"{Name} can only return a KeyCode! {typeof(T)} is not a valid return type!");
            }

            return (T)Convert.ChangeType(Value, typeof(T));
        }
        
        internal override void Invoke<T>(T value)
        {
            OnValueChanged.Invoke((KeyCode) Convert.ChangeType(value, typeof(KeyCode)));
        }
        
        internal override void Invoke()
        {
            OnValueChanged.Invoke(Value);
        }
    }
}
