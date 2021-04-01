using System;
using System.Collections.Generic;
using System.Text;
using On.RoR2;
using R2API.Utils;
using RoR2.UI;
using TMPro;
using UnityEngine;
using RoR2Application = RoR2.RoR2Application;

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

            onValueChangedKeyCode?.Invoke(((KeyCode)int.Parse(base.GetCurrentValue())));
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
                SetKeyBind(KeyCode.None);
                return;
            }

            base.SubmitSetting($"{(int)Event.current.keyCode}");

            StopListening();

            onValueChangedKeyCode?.Invoke((KeyCode)int.Parse(base.GetCurrentValue()));
        }

        private void SetKeyBind(KeyCode keyCode)
        {
            base.SubmitSetting($"{(int)keyCode}");
            onValueChangedKeyCode?.Invoke(keyCode);

            StopListening();
        }

        protected override void Update()
        {
            base.Update();

            if (!_listening)
                return;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                SetKeyBind(KeyCode.LeftShift);
            }
            else if (Input.GetKey(KeyCode.RightShift))
            {
                SetKeyBind(KeyCode.RightShift);
            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                SetKeyBind(KeyCode.Mouse0);
            }
            else if (Input.GetKey(KeyCode.Mouse1))
            {
                SetKeyBind(KeyCode.Mouse1);
            }
            else if (Input.GetKey(KeyCode.Mouse2))
            {
                SetKeyBind(KeyCode.Mouse2);
            }
            else if (Input.GetKey(KeyCode.Mouse3))
            {
                SetKeyBind(KeyCode.Mouse3);
            }
            else if (Input.GetKey(KeyCode.Mouse4))
            {
                SetKeyBind(KeyCode.Mouse4);
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
