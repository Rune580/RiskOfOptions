using System;
using System.Globalization;
using UnityEngine.Events;

namespace RiskOfOptions.OptionConstructors
{
    public class DropDown : OptionConstructorBase
    {
        public int DefaultValue
        {
            set => this.value = value.ToString(CultureInfo.InvariantCulture);
        }

        public UnityAction<int> OnValueChanged;
        public string[] Choices;
        public bool RestartRequired;

        public DropDown()
        {
            OnValueChanged = null;
            DefaultValue = 0;
            Choices = Array.Empty<string>();
            RestartRequired = false;
        }
    }
}