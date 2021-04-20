using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Events;

namespace RiskOfOptions.Interfaces
{
    internal interface IBoolProvider
    {
        public List<UnityAction<bool>> Events { get; set; }

        public bool Value { get; set; }
    }
}
