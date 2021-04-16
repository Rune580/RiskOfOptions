using System;
using System.Collections.Generic;
using System.Text;
using RiskOfOptions.Interfaces;
using RiskOfOptions.OptionComponents;
using RiskOfOptions.OptionOverrides;
using RoR2.UI;
using UnityEngine;
using UnityEngine.Events;

namespace RiskOfOptions.Options
{
    public class KeyBindOption : RiskOfOption, IKeyCodeProvider
    {
        public UnityAction<KeyCode> OnValueChangedKeyCode { get; set; }

        private KeyCode _value;

        internal KeyBindOption(string modGuid, string modName, string name, object[] description, string defaultValue, string categoryName, OptionOverride optionOverride, bool visibility, bool restartRequired)
            : base(modGuid, modName, name, description, defaultValue, categoryName, optionOverride, visibility, restartRequired)
        {
            Value = (KeyCode)int.Parse(defaultValue);

            OptionToken = $"{ModSettingsManager.StartingText}.{modGuid}.{modName}.category_{categoryName.Replace(".", "")}.{name}.keybind".ToUpper().Replace(" ", "_");

            RegisterTokens();
        }

        public KeyCode Value
        {
            get => _value;
            set => _value = value;
        }

        public override GameObject CreateOptionGameObject(RiskOfOption option, GameObject prefab, Transform parent)
        {
            GameObject button = GameObject.Instantiate(prefab, parent);

            var keyBindController = button.GetComponentInChildren<KeybindController>();

            keyBindController.settingName = option.ConsoleToken;
            keyBindController.nameToken = option.NameToken;

            keyBindController.settingSource = RooSettingSource;

            if (OnValueChangedKeyCode != null)
                keyBindController.onValueChangedKeyCode = OnValueChangedKeyCode;

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
    }
}
