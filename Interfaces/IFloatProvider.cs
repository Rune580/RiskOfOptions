using System;
using System.Collections.Generic;
using System.Text;
using RiskOfOptions.Events;
using UnityEngine.Events;

namespace RiskOfOptions.Interfaces
{
    internal interface IFloatProvider
    {
        public FloatEvent OnValueChanged { get; set; }

        public float Value { get; set; }
    }
}
