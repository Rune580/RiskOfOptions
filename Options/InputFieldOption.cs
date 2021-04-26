using System;
using RiskOfOptions.Events;
using RiskOfOptions.Interfaces;
using RiskOfOptions.OptionComponents;
using RiskOfOptions.OptionConstructors;
using RiskOfOptions.OptionOverrides;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace RiskOfOptions.Options
{
    public class InputFieldOption : RiskOfOption, IStringProvider
    {
        public StringEvent OnValueChanged { get; set; } = new StringEvent();

        private string _value;

        private TMP_InputValidator _validator;
        private TMP_InputField.CharacterValidation _characterValidation;
        private bool _validateOnEnter;

        internal InputFieldOption(string modGuid, string modName, string name, object[] description, string defaultValue, string categoryName, bool visibility, bool restartRequired, UnityAction<string> unityAction, bool invokeValueChangedEvent, bool validateOnEnter, TMP_InputValidator stringValidator, TMP_InputField.CharacterValidation characterValidation)
            : base(modGuid, modName, name, description, defaultValue, categoryName, null, visibility, restartRequired, invokeValueChangedEvent)
        {
            Value = defaultValue;

            if (unityAction != null)
            {
                OnValueChanged.AddListener(unityAction);
            }

            _validateOnEnter = validateOnEnter;

            _characterValidation = characterValidation;

            if (stringValidator != null)
            {
                _validator = stringValidator;
                _characterValidation = TMP_InputField.CharacterValidation.CustomValidator;
            }

            OptionToken = $"{ModSettingsManager.StartingText}.{modGuid}.{modName}.category_{categoryName.Replace(".", "")}.{name}.inputfield".ToUpper().Replace(" ", "_");

            RegisterTokens();
        }

        public string Value
        {
            get => _value;
            set
            {
                if (_value == value)
                    return;

                _value = value;

                OnValueChanged.Invoke(Value);
            }
        }

        public override GameObject CreateOptionGameObject(RiskOfOption option, GameObject prefab, Transform parent)
        {
            GameObject button = GameObject.Instantiate(prefab, parent);

            var controller = button.GetComponentInChildren<InputFieldController>();

            controller.settingName = option.ConsoleToken;
            controller.nameToken = option.NameToken;
            
            controller.settingSource = RooSettingSource;

            if (_validator != null)
            {
                controller.validator = _validator;
            }

            controller.characterValidation = _characterValidation;

            controller.validateOnSubmit = _validateOnEnter;

            button.name = $"Mod Option Input Field, {option.Name}";

            return button;
        }

        public override string GetValueAsString()
        {
            return Value;
        }

        public override string GetInternalValueAsString()
        {
            return _value;
        }
        
        public override void SetValue(string newValue)
        {
            Value = newValue;
        }

        public override void SetInternalValue(string newValue)
        {
            _value = newValue;
        }

        public override T GetValue<T>()
        {
            if (typeof(T) != typeof(bool))
            {
                throw new Exception($"{Name} can only return a string! {typeof(T)} is not a valid return type!");
            }

            return (T) Convert.ChangeType(Value, typeof(T));
        }

        internal override void Invoke<T>(T value)
        {
            OnValueChanged.Invoke((string) Convert.ChangeType(value, typeof(string)));
        }

        internal override void Invoke()
        {
            OnValueChanged.Invoke(Value);
        }
    }
}