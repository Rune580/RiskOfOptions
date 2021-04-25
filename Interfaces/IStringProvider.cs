using System.Collections.Generic;
using RiskOfOptions.Events;
using UnityEngine.Events;

namespace RiskOfOptions.Interfaces
{
    internal interface IStringProvider
    {
        public StringEvent OnValueChanged { get; set; }

        public string Value { get; set; }
    }
}