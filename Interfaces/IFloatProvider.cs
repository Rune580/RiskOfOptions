using System;
using System.Collections.Generic;
using System.Text;

namespace RiskOfOptions.Interfaces
{
    internal interface IFloatProvider
    {
        public UnityEngine.Events.UnityAction<float> OnValueChangedFloat { get; set; }

        public float Value { get; set; }
    }
}
