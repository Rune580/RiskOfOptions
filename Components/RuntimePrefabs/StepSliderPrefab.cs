using RiskOfOptions.Components.Options;
using RoR2.UI;
using UnityEngine;

namespace RiskOfOptions.Components.RuntimePrefabs
{
    public class StepSliderPrefab : IRuntimePrefab
    {
        public GameObject StepSlider { get; private set; }
        
        public void Instantiate(GameObject settingsPanel)
        {
            Transform verticalLayout = settingsPanel.transform.Find("SafeArea").Find("SubPanelArea")
                .Find("SettingsSubPanel, Audio").Find("Scroll View").Find("Viewport").Find("VerticalLayout");

            StepSlider = Object.Instantiate(verticalLayout.Find("SettingsEntryButton, Slider (Master Volume)").gameObject);
            StepSlider.name = "Mod Option Prefab, Step Slider";

            var settingsSlider = StepSlider.GetComponentInChildren<SettingsSlider>();

            var sliderController = settingsSlider.gameObject.AddComponent<ModSettingsStepSlider>();

            sliderController.slider = settingsSlider.slider;
            sliderController.valueText = settingsSlider.valueText;
            sliderController.nameLabel = settingsSlider.nameLabel;
            
            Object.DestroyImmediate(settingsSlider);
        }

        public void Destroy()
        {
            Object.DestroyImmediate(StepSlider);
        }
    }
}