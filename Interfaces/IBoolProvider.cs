using System;
using System.Collections.Generic;
using System.Text;

namespace RiskOfOptions.Interfaces
{
    internal interface IBoolProvider
    {
        public UnityEngine.Events.UnityAction<bool> OnValueChangedBool { get; set; }

        public bool Value { get; set; }
    }
}
