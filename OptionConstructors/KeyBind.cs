using UnityEngine;
using UnityEngine.Events;

namespace RiskOfOptions.OptionConstructors
{
    public class KeyBind : OptionConstructorBase
    {
        public KeyCode DefaultValue
        {
            set => this.value = $"{(int) value}";
        }

        public UnityAction<KeyCode> OnValueChanged;

        public KeyBind()
        {
            OnValueChanged = null;
            DefaultValue = KeyCode.None;
        }
    }
}
