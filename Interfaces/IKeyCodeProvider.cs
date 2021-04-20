using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace RiskOfOptions.Interfaces
{
    internal interface IKeyCodeProvider
    {
        public List<UnityAction<KeyCode>> Events { get; set; }

        public KeyCode Value { get; set; }
    }
}
