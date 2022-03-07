using UnityEngine;

namespace RiskOfOptions.Components.RuntimePrefabs
{
    public class SliderPrefab : IRuntimePrefab
    {
        public GameObject Slider { get; private set; }
        
        public void Instantiate(GameObject settingsPanel)
        {
            Transform verticalLayout = settingsPanel.transform.Find("SafeArea").Find("SubPanelArea")
                .Find("SettingsSubPanel, Audio").Find("Scroll View").Find("Viewport").Find("VerticalLayout");

            Slider = Object.Instantiate(verticalLayout.Find("SettingsEntryButton, Slider (Master Volume)").gameObject);
        }

        public void Destroy()
        {
            Object.DestroyImmediate(Slider);
        }
    }
}