using RiskOfOptions.Components.Options;
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
            Transform verticalLayout = settingsPanel.transform.Find("SafeArea").Find("SubPanelArea")
                .Find("SettingsSubPanel, Audio").Find("Scroll View").Find("Viewport").Find("VerticalLayout");

            IntSlider = Object.Instantiate(verticalLayout.Find("SettingsEntryButton, Slider (Master Volume)").gameObject);
            IntSlider.name = "Mod Option Prefab, Int Slider";

            var settingsSlider = IntSlider.GetComponentInChildren<SettingsSlider>();

            var sliderController = settingsSlider.gameObject.AddComponent<ModSettingsIntSlider>();

            sliderController.slider = settingsSlider.slider;
            sliderController.valueText = settingsSlider.valueText;
            sliderController.nameLabel = settingsSlider.nameLabel;
            
            Object.DestroyImmediate(settingsSlider);
        }

        public void Destroy()
        {
            Object.DestroyImmediate(IntSlider);
        }
    }
}