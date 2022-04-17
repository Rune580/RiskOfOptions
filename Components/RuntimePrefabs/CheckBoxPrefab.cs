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
            Transform verticalLayout = settingsPanel.transform.Find("SafeArea").Find("SubPanelArea")
                .Find("SettingsSubPanel, Audio").Find("Scroll View").Find("Viewport").Find("VerticalLayout");

            var temp = Object.Instantiate(verticalLayout.Find("SettingsEntryButton, Bool (Audio Focus)").gameObject);

            CheckBoxButton = Object.Instantiate(Prefabs.BoolButton);
            Object.DestroyImmediate(CheckBoxButton.GetComponentInChildren<CarouselController>());

            var carouselController = temp.GetComponentInChildren<CarouselController>();
            var boolController = CheckBoxButton.AddComponent<ModSettingsBool>();

            boolController.checkBox = carouselController.optionalImage;
            boolController.checkBoxFalse = carouselController.choices[0].customSprite;
            boolController.checkBoxTrue = carouselController.choices[1].customSprite;
            boolController.nameLabel = carouselController.nameLabel;
            
            Object.DestroyImmediate(temp);
            
            var boolButton = CheckBoxButton.transform.Find("CarouselRect").Find("BoolButton").GetComponent<HGButton>();
            boolButton.onClick.RemoveAllListeners();
        }

        public void Destroy()
        {
            Object.DestroyImmediate(CheckBoxButton);
        }
    }
}