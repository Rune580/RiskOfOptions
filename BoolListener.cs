using System;
using RoR2.UI;
using UnityEngine;

namespace RiskOfOptions
{
    public class BoolListener : MonoBehaviour
    {
        public UnityEngine.Events.UnityAction<bool> onValueChangedBool;

        private bool _previousValue;

        public bool isOverriden = false;

        private CarouselController _cc;

        private void Awake()
        {
            _cc = gameObject.GetComponentInChildren<CarouselController>();

            _previousValue = _cc.GetCurrentValue() == "1";
        }

        private void LateUpdate()
        {
            if (!_cc || isOverriden)
                return;

            bool currentValue = false;

            string value = _cc.GetCurrentValue();

            if (value == "1")
            {
                currentValue = true;
            }

            if (_previousValue != currentValue)
            {
                Debug.Log($"Bool listener fired off from {gameObject.name}, Outgoing Value {currentValue}");
                onValueChangedBool.Invoke(currentValue);
            }

            _previousValue = currentValue;
        }
    }
}
