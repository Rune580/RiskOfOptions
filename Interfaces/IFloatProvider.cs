using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Events;

namespace RiskOfOptions.Interfaces
{
    internal interface IFloatProvider
    {
        public List<UnityAction<float>> Events { get; set; }

        public float Value { get; set; }
    }
}
