using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using RiskOfOptions.Lib;
using RoR2.UI;
using UnityEngine;
using RoR2Application = RoR2.RoR2Application;

namespace RiskOfOptions.Utils
{
    public class KeyBindUtil : MonoBehaviour
    {
        private MPEventSystem _mpEventSystem;
        private SimpleDialogBox _dialogBox;
        private Action<KeyboardShortcut> _onBind;
        private float _timeoutTimer;
        private string _keyBindName;
        private bool _finished;
        
        private readonly List<KeyCode> _heldKeys = new();
        private readonly List<KeyCode> _keySequence = new();

        public void StartListening()
        {
            if (ModSettingsManager.disablePause || _finished)
            {
                DestroyImmediate(gameObject);
                return;
            }

            ModSettingsManager.disablePause = true;
            
            _dialogBox = SimpleDialogBox.Create(_mpEventSystem);
            LanguageApi.AddDelegate(LanguageTokens.OptionRebindDialogDescription, ControlRebindingHook);
        }

        public void StopListening()
        {
            _finished = true;
            if (_dialogBox && _dialogBox.rootObject)
            {
                DestroyImmediate(_dialogBox.rootObject);
                _dialogBox = null;
            }
            LanguageApi.RemoveDelegate(LanguageTokens.OptionRebindDialogDescription);

            RoR2Application.unscaledTimeTimers.CreateTimer(0.3f, Finish);
            Destroy(gameObject, 0.5f);
        }
        
        private void Update()
        {
            if (_finished)
                return;
            
            _timeoutTimer -= Time.unscaledDeltaTime;
            
            ExtraKeyDown();
            ExtraKeyUp();
            
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
            if (_finished)
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
            _onBind.Invoke(keyBind);
            
            StopListening();
        }

        private string ControlRebindingHook()
        {
            int roundedTime = Mathf.RoundToInt(_timeoutTimer);

            return $"Press the button(s) you wish to assign for {_keyBindName}.\n{roundedTime} second(s) remaining.";
        }

        private void Finish()
        {
            ModSettingsManager.disablePause = false;
        }

        public static void StartBinding(Action<KeyboardShortcut> onBind, string keyBindName, float timeoutTimer = 5f, MPEventSystem mpEventSystem = null)
        {
            GameObject gameObject = new GameObject("KeyBindUtil instance", typeof(KeyBindUtil));

            KeyBindUtil keyBindUtil = gameObject.GetComponent<KeyBindUtil>();
            keyBindUtil._onBind = onBind;
            keyBindUtil._timeoutTimer = timeoutTimer;
            keyBindUtil._mpEventSystem = mpEventSystem;
            keyBindUtil._keyBindName = keyBindName;
            
            keyBindUtil.StartListening();
        }
    }
}