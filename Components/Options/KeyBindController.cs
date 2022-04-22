using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using On.RoR2;
using RiskOfOptions.Utils;
using RoR2.UI;
using TMPro;
using UnityEngine;
using RoR2Application = RoR2.RoR2Application;

namespace RiskOfOptions.Components.Options
{
    // ReSharper disable once IdentifierTypo
    public class KeyBindController : ModSettingsControl<KeyboardShortcut>
    {
        private bool _interactable;

        private TextMeshProUGUI _keyBindLabel;
        private MPEventSystem _mpEventSystem;
        private HGButton _bindingButton;

        protected override void Awake()
        {
            if (settingToken == "")
                return;

            _interactable = true;

            base.Awake();

            _mpEventSystem = GetComponentInParent<MPEventSystemLocator>().eventSystem;

            _keyBindLabel = transform.Find("BindingContainer").Find("BindingButton").Find("ButtonText").GetComponent<TextMeshProUGUI>();

            _bindingButton = transform.Find("BindingContainer").Find("BindingButton").GetComponent<HGButton>();
        }

        protected override void Start()
        {
            base.Start();

            SetDisplay();
        }

        public void StartListening()
        {
            KeyBindUtil.StartBinding(SubmitValue, ModSettingsManager.OptionCollection.GetOption(settingToken).Name, 5f, _mpEventSystem);
        }

        protected override void Disable()
        {
            _interactable = false;
            
            foreach (var button in GetComponentsInChildren<HGButton>())
                button.interactable = false;
        }

        protected override void Enable()
        {
            _interactable = true;
            
            foreach (var button in GetComponentsInChildren<HGButton>())
                button.interactable = true;
        }

        protected void Update()
        {
            _bindingButton.interactable = _interactable && !ModSettingsManager.doingKeyBind;
        }

        protected override void OnUpdateControls()
        {
            base.OnUpdateControls();
            
            SetDisplay();
        }


        private void SetDisplay()
        {
            string displayText = GetCurrentValue().ToString();

            displayText = displayText.Replace("Mouse0", "M1").Replace("Mouse1", "M2")
                .Replace("Mouse2", "M3").Replace("Mouse3", "M4").Replace("Mouse4", "M5");

            char[] characters = displayText.ToCharArray();

            displayText = "";

            int i = 0;

            while (true)
            {
                if (characters.Length <= i)
                    break;

                if (char.IsUpper(characters[i]) && i != 0)
                {
                    if (i - 1 > 0 && characters[i - 1] != ' ')
                        displayText += " ";
                }

                displayText += characters[i];

                i++;
            }

            _keyBindLabel.SetText(displayText);
        }
    }
}
