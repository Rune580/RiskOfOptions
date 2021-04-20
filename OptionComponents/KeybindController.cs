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
        private int _count;
        private float _timeoutTimer;

        private TextMeshProUGUI _keybindLabel;
        private MPEventSystem _mpEventSystem;
        private SimpleDialogBox _dialogBox;
        private HGButton _bindingButton;

        protected new void Awake()
        {
            settingSource = (BaseSettingsControl.SettingSource) 2;

            if (base.settingName == "")
                return;

            base.Awake();

            _mpEventSystem = GetComponentInParent<MPEventSystemLocator>().eventSystem;

            _keybindLabel = transform.Find("BindingContainer").Find("BindingButton").Find("ButtonText").GetComponent<TextMeshProUGUI>();

            _bindingButton = transform.Find("BindingContainer").Find("BindingButton").GetComponent<HGButton>();
        }

        protected new void Start()
        {
            base.Start();

            SetDisplay();

            //onValueChangedKeyCode?.Invoke(((KeyCode)int.Parse(base.GetCurrentValue())));
        }

        protected override void Update()
        {
            base.Update();

            _bindingButton.interactable = !ModSettingsManager.doingKeybind;

            if (!_listening)
                return;

            _timeoutTimer -= Time.unscaledDeltaTime;

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

            UpdateDialogText();
        }

        public void StartListening()
        {
            if (_listening || ModSettingsManager.doingKeybind)
                return;

            _count = 2;
            _listening = true;

            ModSettingsManager.doingKeybind = _listening;

            _timeoutTimer = 5f;

            DisplayDialogBox();
        }

        public void StopListening()
        {
            _listening = false;

            SetDisplay();

            RoR2Application.unscaledTimeTimers.CreateTimer(0.3f, FixPauseMenu);

            DestroyDialogBox();
        }

        private void DisplayDialogBox()
        {
            _dialogBox = SimpleDialogBox.Create(_mpEventSystem);

            On.RoR2.Language.GetString_string += ControlRebindingHook;
        }

        private void DestroyDialogBox()
        {
            if (_dialogBox && _dialogBox.rootObject)
            {
                GameObject.DestroyImmediate(_dialogBox.rootObject);

                _dialogBox = null;
            }

            On.RoR2.Language.GetString_string -= ControlRebindingHook;
        }

        private string ControlRebindingHook(Language.orig_GetString_string orig, string token)
        {
            if (token != LanguageTokens.OptionRebindDialogDescription)
                return orig(token);


            string controlName = ModSettingsManager.GetOption(settingName).Name;

            int roundedTime = Mathf.RoundToInt(_timeoutTimer);

            return $"Press the button you wish to assign for {controlName}.\n{roundedTime} second(s) remaining.";
            //return $"Press the button you wish to assign for {controlName}.\n{(roundedTime == 1 ? roundedTime + " second" : roundedTime + " seconds")} remaining.";
        }

        private void UpdateDialogText()
        {
            if (_timeoutTimer < 0f || !_dialogBox)
            {
                StopListening();
                return;
            }

            _dialogBox.headerToken = new SimpleDialogBox.TokenParamsPair
            {
                token = LanguageTokens.OptionRebindDialogTitle,
                formatParams = Array.Empty<object>()
            };

            

            _dialogBox.descriptionToken = new SimpleDialogBox.TokenParamsPair
            {
                token = LanguageTokens.OptionRebindDialogDescription,
                formatParams = new object[]
                {
                    LanguageTokens.OptionRebindDialogDescription,
                    _timeoutTimer
                }
            };
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

            SubmitSetting($"{(int)Event.current.keyCode}");

            StopListening();

            //onValueChangedKeyCode?.Invoke((KeyCode)int.Parse(base.GetCurrentValue()));
        }

        private void SetKeyBind(KeyCode keyCode)
        {
            SubmitSetting($"{(int)keyCode}");
            //onValueChangedKeyCode?.Invoke(keyCode);

            StopListening();
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
