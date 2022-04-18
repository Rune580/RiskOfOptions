using RiskOfOptions.Components.Options;
using RiskOfOptions.Resources;
using RoR2.UI;
using UnityEngine;
using UnityEngine.Events;

namespace RiskOfOptions.Components.RuntimePrefabs
{
    public class CheckBoxPrefab : IRuntimePrefab
    {
        public GameObject CheckBoxButton { get; private set; }
        
        public void Instantiate(GameObject settingsPanel)
        {
            CheckBoxButton = Object.Instantiate(Prefabs.BoolButton);
            
            Transform verticalLayout = settingsPanel.transform.Find("SafeArea").Find("SubPanelArea")
                .Find("SettingsSubPanel, Audio").Find("Scroll View").Find("Viewport").Find("VerticalLayout");

            var temp = Object.Instantiate(verticalLayout.Find("SettingsEntryButton, Bool (Audio Focus)").gameObject);
            var carouselController = temp.GetComponentInChildren<CarouselController>();
            var boolController = CheckBoxButton.GetComponent<ModSettingsBool>();

            boolController.checkBoxFalse = carouselController.choices[0].customSprite;
            boolController.checkBoxTrue = carouselController.choices[1].customSprite;
            
            CheckBoxButton.GetComponent<HGButton>().hoverLanguageTextMeshController = temp.GetComponent<HGButton>().hoverLanguageTextMeshController;
            Object.DestroyImmediate(temp);
        }

        public void Destroy()
        {
            Object.DestroyImmediate(CheckBoxButton);
        }
    }
}