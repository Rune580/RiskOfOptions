using RiskOfOptions.OptionOverrides;
using UnityEngine.Events;

namespace RiskOfOptions.OptionConstructors
{
    public class CheckBox : OptionConstructorBase
    {
        public bool DefaultValue
        {
            set => this.value = $"{(value ? "1" : "0")}";
        }
        public bool RestartRequired;
        public CheckBoxOverride Override;
        public UnityAction<bool> OnValueChanged;
        public CheckBox()
        {
            OnValueChanged = null;
            DefaultValue = false;
            RestartRequired = false;
            Override = null;
        }
    }
}
