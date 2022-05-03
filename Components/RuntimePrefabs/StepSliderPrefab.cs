using RiskOfOptions.Components.Options;
using RiskOfOptions.Resources;
using RoR2.UI;
using UnityEngine;

namespace RiskOfOptions.Components.RuntimePrefabs
{
    public class StepSliderPrefab : IRuntimePrefab
    {
        public GameObject StepSlider { get; private set; }
        
        public void Instantiate(GameObject settingsPanel)
        {
            StepSlider = Object.Instantiate(Prefabs.stepSliderButton);
        }

        public void Destroy()
        {
            Object.DestroyImmediate(StepSlider);
        }
    }
}