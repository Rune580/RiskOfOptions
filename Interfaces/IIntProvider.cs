using System.Collections.Generic;
using UnityEngine.Events;

namespace RiskOfOptions.Interfaces
{
    internal interface IIntProvider
    {
        public List<UnityAction<int>> Events { get; set; }

        public int Value { get; set; }
    }
}