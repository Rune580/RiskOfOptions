using RiskOfOptions.Components.Options;
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

            CheckBoxButton = Object.Instantiate(verticalLayout.Find("SettingsEntryButton, Bool (Audio Focus)").gameObject);
            CheckBoxButton.name = "Mod Option Prefab, Bool";

            var carouselController = CheckBoxButton.GetComponentInChildren<CarouselController>();

            var boolController = carouselController.gameObject.AddComponent<ModSettingsBool>();

            boolController.checkBox = carouselController.optionalImage;
            boolController.checkBoxFalse = carouselController.choices[0].customSprite;
            boolController.checkBoxTrue = carouselController.choices[1].customSprite;
            boolController.nameLabel = carouselController.nameLabel;
                
            Object.DestroyImmediate(carouselController);

            var boolButton = CheckBoxButton.transform.Find("CarouselRect").Find("BoolButton").GetComponent<HGButton>();
            boolButton.onClick.RemoveAllListeners();
        }

        public void Destroy()
        {
            Object.DestroyImmediate(CheckBoxButton);
        }
    }
}