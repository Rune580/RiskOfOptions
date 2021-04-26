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
        public TMP_InputValidator StringValidator;
        public TMP_InputField.CharacterValidation CharacterValidation;

        public InputField()
        {
            StringValidator = null;
            CharacterValidation = TMP_InputField.CharacterValidation.None;
            RestartRequired = false;
            OnValueChanged = null;
            DefaultValue = "";
        }
    }
}