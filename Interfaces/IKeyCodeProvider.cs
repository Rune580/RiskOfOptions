using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RiskOfOptions.Interfaces
{
    internal interface IKeyCodeProvider
    {
        public UnityEngine.Events.UnityAction<KeyCode> OnValueChangedKeyCode { get; set; }

        public KeyCode Value { get; set; }
    }
}
