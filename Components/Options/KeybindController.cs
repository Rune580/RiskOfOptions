using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using On.RoR2;
using RoR2.UI;
using TMPro;
using UnityEngine;
using RoR2Application = RoR2.RoR2Application;

namespace RiskOfOptions.Components.Options
{
    // ReSharper disable once IdentifierTypo
    public class KeybindController : ModSettingsControl<KeyboardShortcut>
    {
        private bool _listening;
        private float _timeoutTimer;

        private TextMeshProUGUI _keyBindLabel;
        private MPEventSystem _mpEventSystem;
        private SimpleDialogBox _dialogBox;
        private HGButton _bindingButton;

        private readonly List<KeyCode> _heldKeys = new();
        private readonly List<KeyCode> _keySequence = new();

        protected new void Awake()
        {
            if (settingToken == "")
                return;

            base.Awake();

            _mpEventSystem = GetComponentInParent<MPEventSystemLocator>().eventSystem;

            _keyBindLabel = transform.Find("BindingContainer").Find("BindingButton").Find("ButtonText").GetComponent<TextMeshProUGUI>();

            _bindingButton = transform.Find("BindingContainer").Find("BindingButton").GetComponent<HGButton>();
        }

        protected new void Start()
        {
            base.Start();

            SetDisplay();
        }

        protected override void Disable()
        {
            
        }

        protected override void Enable()
        {
            
        }

        protected void Update()
        {
            _bindingButton.interactable = !ModSettingsManager.DoingKeybind;

            if (!_listening)
                return;

            _timeoutTimer -= Time.unscaledDeltaTime;
            
            ExtraKeyDown();
            ExtraKeyUp();

            UpdateDialogText();
        }

        protected override void OnUpdateControls()
        {
            base.OnUpdateControls();
            
            SetDisplay();
        }

        private void ExtraKeyDown()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                KeyDown(KeyCode.LeftShift);
            }
            else if (Input.GetKeyDown(KeyCode.RightShift))
            {
                KeyDown(KeyCode.RightShift);
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                KeyDown(KeyCode.Mouse0);
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                KeyDown(KeyCode.Mouse1);
            }
            else if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                KeyDown(KeyCode.Mouse2);
            }
            else if (Input.GetKeyDown(KeyCode.Mouse3))
            {
                KeyDown(KeyCode.Mouse3);
            }
            else if (Input.GetKeyDown(KeyCode.Mouse4))
            {
                KeyDown(KeyCode.Mouse4);
            }
        }

        private void ExtraKeyUp()
        {
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                KeyUp(KeyCode.LeftShift);
            }
            else if (Input.GetKeyUp(KeyCode.RightShift))
            {
                KeyUp(KeyCode.RightShift);
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                KeyUp(KeyCode.Mouse0);
            }
            else if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                KeyUp(KeyCode.Mouse1);
            }
            else if (Input.GetKeyUp(KeyCode.Mouse2))
            {
                KeyUp(KeyCode.Mouse2);
            }
            else if (Input.GetKeyUp(KeyCode.Mouse3))
            {
                KeyUp(KeyCode.Mouse3);
            }
            else if (Input.GetKeyUp(KeyCode.Mouse4))
            {
                KeyUp(KeyCode.Mouse4);
            }
        }

        public void StartListening()
        {
            if (_listening || ModSettingsManager.DoingKeybind)
                return;
            
            _heldKeys.Clear();
            _keySequence.Clear();

            _listening = true;

            ModSettingsManager.DoingKeybind = _listening;

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

            Language.GetString_string += ControlRebindingHook;
        }

        private void DestroyDialogBox()
        {
            if (_dialogBox && _dialogBox.rootObject)
            {
                GameObject.DestroyImmediate(_dialogBox.rootObject);

                _dialogBox = null;
            }

            Language.GetString_string -= ControlRebindingHook;
        }

        private string ControlRebindingHook(Language.orig_GetString_string orig, string token)
        {
            if (token != LanguageTokens.OptionRebindDialogDescription)
                return orig(token);


            string controlName = ModSettingsManager.OptionCollection.GetOption(settingToken).Name;

            int roundedTime = Mathf.RoundToInt(_timeoutTimer);

            return $"Press the button(s) you wish to assign for {controlName}.\n{roundedTime} second(s) remaining.";
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

            if (!Event.current.isKey || Event.current.type is not (EventType.KeyDown or EventType.KeyUp))
                return;

            if (Event.current.keyCode == KeyCode.None)
                return;

            if (Event.current.keyCode == KeyCode.Escape)
            {
                SetKeyBind(KeyCode.None);
                return;
            }

            switch (Event.current.type)
            {
                case EventType.KeyDown:
                    KeyDown(Event.current.keyCode);
                    break;
                case EventType.KeyUp:
                    KeyUp(Event.current.keyCode);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void KeyDown(KeyCode keyCode)
        {
            if (_heldKeys.Contains(keyCode))
                return;
            
            _heldKeys.Add(keyCode);
        }

        private void KeyUp(KeyCode keyCode)
        {
            if (!_heldKeys.Contains(keyCode))
                return;
            
            _heldKeys.Remove(keyCode);
            
            if (_keySequence.Contains(keyCode))
                return;
            
            _keySequence.Add(keyCode);

            if (_heldKeys.Count != 0)
                return;
            
            KeyboardShortcut keyBind;

            if (_keySequence.Count > 1)
            {
                KeyCode[] modifiers = new KeyCode[_keySequence.Count - 1];

                for (int i = 0; i < modifiers.Length; i++)
                    modifiers[i] = _keySequence[i + 1];

                keyBind = new KeyboardShortcut(_keySequence[0], modifiers);
            }
            else
            {
                keyBind = new KeyboardShortcut(_keySequence[0]);
            }
                
            SetKeyBind(keyBind);
        }

        private void SetKeyBind(KeyCode keyCode)
        {
            SetKeyBind(new KeyboardShortcut(keyCode));
        }

        private void SetKeyBind(KeyboardShortcut keyBind)
        {
            SubmitValue(keyBind);
            
            StopListening();
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

        private void FixPauseMenu()
        {
            ModSettingsManager.DoingKeybind = _listening;
        }
    }
}
