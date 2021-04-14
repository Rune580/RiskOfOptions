using System;
using System.Collections.Generic;
using RoR2.UI;
using UnityEngine;
using UnityEngine.Events;

namespace RiskOfOptions
{
    public class BoolListener : MonoBehaviour
    {
        private readonly List<UnityEngine.Events.UnityAction<bool>> _listeners = new List<UnityAction<bool>>();

        private bool _previousValue;

        public bool isOverriden = false;

        private CarouselController _cc;

        private void Awake()
        {
            _cc = gameObject.GetComponentInChildren<CarouselController>();

            _previousValue = GetCurrentValue();
        }

        private void LateUpdate()
        {
            if (!_cc || isOverriden)
                return;

            bool currentValue = GetCurrentValue();

            if (_previousValue != currentValue)
            {
                Debug.Log($"Bool listener fired off from {gameObject.name}, Outgoing Value {currentValue}");
                Invoke(currentValue);
            }

            _previousValue = currentValue;
        }

        private bool GetCurrentValue()
        {
            return _cc.GetCurrentValue() == "1";
        }

        public void AddListener(UnityEngine.Events.UnityAction<bool> unityAction)
        {
            _listeners.Add(unityAction);
        }

        public void RemoveAllListeners()
        {
            _listeners.Clear();
        }

        public void Invoke(bool newValue)
        {
            foreach (var listener in _listeners)
            {
                listener.Invoke(newValue);
            }
        }

    }
}
