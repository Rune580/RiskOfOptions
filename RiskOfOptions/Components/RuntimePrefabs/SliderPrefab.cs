using RiskOfOptions.Resources;
using UnityEngine;

namespace RiskOfOptions.Components.RuntimePrefabs
{
    public class SliderPrefab : IRuntimePrefab
    {
        public GameObject Slider { get; private set; }
        
        public void Instantiate(GameObject settingsPanel)
        {
            // Transform verticalLayout = settingsPanel.transform.Find("SafeArea").Find("SubPanelArea")
            //     .Find("SettingsSubPanel, Audio").Find("Scroll View").Find("Viewport").Find("VerticalLayout");
            //
            // Slider = Object.Instantiate(verticalLayout.Find("SettingsEntryButton, Slider (Master Volume)").gameObject);
            // Slider.name = "Mod Option Prefab, Slider";
            //
            // var settingsSlider = Slider.GetComponentInChildren<SettingsSlider>();
            //
            // var sliderController = settingsSlider.gameObject.AddComponent<ModSettingsSlider>();
            //
            // sliderController.slider = settingsSlider.slider;
            // sliderController.valueText = settingsSlider.valueText;
            // sliderController.nameLabel = settingsSlider.nameLabel;
            //
            // Object.DestroyImmediate(settingsSlider);

            Slider = Object.Instantiate(Prefabs.sliderButton);
        }

        public void Destroy()
        {
            Object.DestroyImmediate(Slider);
        }
    }
}