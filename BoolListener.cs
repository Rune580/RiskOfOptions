using RoR2.UI;
using UnityEngine;

namespace RiskOfOptions
{
    public class BoolListener : MonoBehaviour
    {
        public UnityEngine.Events.UnityAction<bool> onValueChangedBool;

        private bool _previousValue = false;

        private CarouselController _cc;

        private void Start()
        {
            _cc = gameObject.GetComponentInChildren<CarouselController>();
        }

        private void Update()
        {
            if (!_cc)
                return;

            bool currentValue = false;

            string value = _cc.GetCurrentValue();

            if (value == "1")
            {
                currentValue = true;
            }

            if (_previousValue != currentValue)
            {
                onValueChangedBool.Invoke(currentValue);
            }

            _previousValue = currentValue;
        }
    }
}
