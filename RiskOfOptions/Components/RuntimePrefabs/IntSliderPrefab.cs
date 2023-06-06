using RiskOfOptions.Resources;
using UnityEngine;

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