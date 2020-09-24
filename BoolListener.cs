using RoR2.UI;
using UnityEngine;

namespace RiskOfOptions
{
    class BoolListener : MonoBehaviour
    {
        public UnityEngine.Events.UnityAction<bool> onValueChangedBool;

        private bool previousValue = false;

        private CarouselController cc;

        private void Start()
        {
            cc = gameObject.GetComponentInChildren<CarouselController>();
        }

        private void Update()
        {
            if (cc != null)
            {
                bool currentValue = false;

                string value = cc.GetCurrentValue();

                if (value == "1")
                {
                    currentValue = true;
                }

                if (previousValue != currentValue)
                {
                    onValueChangedBool.Invoke(previousValue);
                }

                previousValue = currentValue;
            }
        }
    }
}
