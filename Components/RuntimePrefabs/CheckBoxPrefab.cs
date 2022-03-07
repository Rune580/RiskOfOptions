using UnityEngine;

namespace RiskOfOptions.Components.RuntimePrefabs
{
    public class CheckBoxPrefab : IRuntimePrefab
    {
        public GameObject CheckBoxButton { get; private set; }
        
        public void Instantiate(GameObject settingsPanel)
        {
            Transform verticalLayout = settingsPanel.transform.Find("SafeArea").Find("SubPanelArea")
                .Find("SettingsSubPanel, Audio").Find("Scroll View").Find("Viewport").Find("VerticalLayout");

            CheckBoxButton = Object.Instantiate(verticalLayout.Find("SettingsEntryButton, Bool (Audio Focus)").gameObject);
        }

        public void Destroy()
        {
            Object.DestroyImmediate(CheckBoxButton);
        }
    }
}