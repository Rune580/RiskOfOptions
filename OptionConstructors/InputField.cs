using System;
using BepInEx.Configuration;
using TMPro;
using UnityEngine.Events;

namespace RiskOfOptions.OptionConstructors
{
    public class InputField : OptionConstructorBase
    {
        public string DefaultValue
        {
            set => this.value = value;
        }
        
        public bool ValidateOnEnter;
        public bool RestartRequired;
        public UnityAction<string> OnValueChanged;
        public ConfigEntry<string> ConfigEntry;
        public TMP_InputValidator StringValidator;
        public TMP_InputField.CharacterValidation CharacterValidation;

        public InputField(ConfigEntry<string> configEntry)
        {
            ConfigEntry = configEntry ?? throw new NullReferenceException("configEntry must not be null");
            StringValidator = null;
            CharacterValidation = TMP_InputField.CharacterValidation.None;
            RestartRequired = false;
            OnValueChanged = null;
            DefaultValue = "";
        }
    }
}