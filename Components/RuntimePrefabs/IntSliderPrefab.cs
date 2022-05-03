using RiskOfOptions.Components.Options;
using RiskOfOptions.Resources;
using RoR2.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace RiskOfOptions.Components.RuntimePrefabs
{
    public class IntSliderPrefab : IRuntimePrefab
    {
        public GameObject IntSlider { get; private set; }
        
        public void Instantiate(GameObject settingsPanel)
        {
            IntSlider = Object.Instantiate(Prefabs.intSliderButton);
        }

        public void Destroy()
        {
            Object.DestroyImmediate(IntSlider);
        }
    }
}