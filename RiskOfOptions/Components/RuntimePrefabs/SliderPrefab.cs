using RiskOfOptions.Resources;
using UnityEngine;

namespace RiskOfOptions.Components.RuntimePrefabs
{
    public class SliderPrefab : IRuntimePrefab
    {
        public GameObject Slider { get; private set; }
        
        public void Instantiate(GameObject settingsPanel)
        {
            Slider = Object.Instantiate(Prefabs.sliderButton);
        }

        public void Destroy()
        {
            Object.DestroyImmediate(Slider);
        }
    }
}