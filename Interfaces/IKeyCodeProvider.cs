using System;
using System.Collections.Generic;
using System.Text;
using RiskOfOptions.Events;
using UnityEngine;
using UnityEngine.Events;

namespace RiskOfOptions.Interfaces
{
    internal interface IKeyCodeProvider
    {
        public KeyCodeEvent OnValueChanged { get; set; }

        public KeyCode Value { get; set; }
    }
}
