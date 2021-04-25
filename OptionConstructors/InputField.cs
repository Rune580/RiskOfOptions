using UnityEngine.Events;

namespace RiskOfOptions.OptionConstructors
{
    public class InputField : OptionConstructorBase
    {
        /// <summary>
        /// A delegate that provides developers a way to validate a input field.
        /// return true if the string is valid. message will not be shown to the user.
        /// return false if the string is invalid. the output message will be shown to the user.
        /// </summary>
        public delegate bool ValidateString(string input, out string message);

        public string DefaultValue
        {
            set => this.value = value;
        }
        
        public bool ValidateOnEnter;
        public bool RestartRequired;
        public UnityAction<string> OnValueChanged;
        public ValidateString StringValidator;

        public InputField()
        {
            StringValidator = null;
            RestartRequired = false;
            OnValueChanged = null;
            DefaultValue = "";
        }
    }
}