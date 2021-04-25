using System;
using System.Collections.Generic;
using System.Text;
using RiskOfOptions.Events;
using UnityEngine.Events;

namespace RiskOfOptions.Interfaces
{
    internal interface IBoolProvider
    {
        public BoolEvent OnValueChanged { get; set; }

        public bool Value { get; set; }
    }
}
