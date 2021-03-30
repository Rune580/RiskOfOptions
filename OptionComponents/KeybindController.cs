using System;
using System.Collections.Generic;
using System.Text;
using R2API.Utils;
using RoR2;
using RoR2.UI;
using TMPro;
using UnityEngine;

namespace RiskOfOptions.OptionComponents
{
    // ReSharper disable once IdentifierTypo
    public class KeybindController : BaseSettingsControl
    {
        private bool _listening = false;
        private TextMeshProUGUI _keybindLabel;

        private int _count;

        public UnityEngine.Events.UnityAction<KeyCode> onValueChangedKeyCode;

        protected new void Awake()
        {
            base.settingSource = (BaseSettingsControl.SettingSource) 2;

            if (base.settingName == "")
                return;

            base.Awake();

            _keybindLabel = transform.Find("BindingContainer").Find("BindingButton").Find("ButtonText").GetComponent<TextMeshProUGUI>();
        }

        protected new void Start()
        {
            base.Start();

            SetDisplay();
        }


        public void StartListening()
        {
            if (_listening || ModSettingsManager.doingKeybind)
                return;

            _count = 2;
            _listening = true;

            ModSettingsManager.doingKeybind = _listening;

            _keybindLabel.SetText(".");

            RoR2Application.unscaledTimeTimers.CreateTimer(1f, UpdateKeyBindTextDuringListen);
        }

        public void OnGUI()
        {
            if (!_listening)
                return;

            if (!Event.current.isKey || Event.current.type != EventType.KeyDown)
                return;

            if (Event.current.keyCode == KeyCode.None)
                return;



            if (Event.current.keyCode == KeyCode.Escape)
            {
                base.SubmitSetting($"{(int)KeyCode.None}");

                StopListening();

                return;
            }

            base.SubmitSetting($"{(int)Event.current.keyCode}");

            StopListening();
        }

        protected override void Update()
        {
            base.Update();

            if (!_listening)
                return;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                base.SubmitSetting($"{(int)KeyCode.LeftShift}");

                StopListening();
            }
            else if (Input.GetKey(KeyCode.RightShift))
            {
                base.SubmitSetting($"{(int)KeyCode.RightShift}");

                StopListening();
            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                base.SubmitSetting($"{(int)KeyCode.Mouse0}");

                StopListening();
            }
            else if (Input.GetKey(KeyCode.Mouse1))
            {
                base.SubmitSetting($"{(int)KeyCode.Mouse1}");

                StopListening();
            }
            else if (Input.GetKey(KeyCode.Mouse2))
            {
                base.SubmitSetting($"{(int)KeyCode.Mouse2}");

                StopListening();
            }
            else if (Input.GetKey(KeyCode.Mouse3))
            {
                base.SubmitSetting($"{(int)KeyCode.Mouse3}");

                StopListening();
            }
            else if (Input.GetKey(KeyCode.Mouse4))
            {
                base.SubmitSetting($"{(int)KeyCode.Mouse4}");

                StopListening();
            }
        }

        private void UpdateKeyBindTextDuringListen()
        {
            if (!_listening)
                return;

            if (_count > 3)
                _count = 1;

            string text = "";

            for (int i = 0; i < _count; i++)
            {
                text += ".";
            }

            _count++;

            _keybindLabel.SetText(text);

            RoR2Application.unscaledTimeTimers.CreateTimer(1f, UpdateKeyBindTextDuringListen);
        }


        public void StopListening()
        {
            _listening = false;

            SetDisplay();

            RoR2Application.unscaledTimeTimers.CreateTimer(0.3f, FixPauseMenu);
        }

        private void SetDisplay()
        {
            string displayText = ((KeyCode) int.Parse(base.GetCurrentValue())).ToString();

            switch (displayText)
            {
                case "Mouse0":
                    displayText = "M1";
                    break;
                case "Mouse1":
                    displayText = "M2";
                    break;
                case "Mouse2":
                    displayText = "M3";
                    break;
                case "Mouse3":
                    displayText = "M4";
                    break;
                case "Mouse4":
                    displayText = "M5";
                    break;
            }

            char[] characters = displayText.ToCharArray();

            displayText = "";

            int i = 0;

            while (true)
            {
                if (characters.Length <= i)
                    break;

                if (char.IsUpper(characters[i]) && i != 0)
                    displayText += " ";

                displayText += characters[i];

                i++;
            }

            _keybindLabel.SetText(displayText);
        }

        private void FixPauseMenu()
        {
            ModSettingsManager.doingKeybind = _listening;
        }
    }
}
