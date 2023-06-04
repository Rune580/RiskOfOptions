using RiskOfOptions.Components.Options;
using RoR2.UI;
using UnityEngine;
using UnityEngine.UI;

namespace RiskOfOptions.Components.RuntimePrefabs
{
    public class GenericButtonPrefab : IRuntimePrefab
    {
        public GameObject GenericButton { get; private set; }
        
        public void Instantiate(GameObject settingsPanel)
        {
            Transform verticalLayout = settingsPanel.transform.Find("SafeArea").Find("SubPanelArea")
                .Find("SettingsSubPanel, Audio").Find("Scroll View").Find("Viewport").Find("VerticalLayout");
            
            GenericButton = Object.Instantiate(verticalLayout.Find("SettingsEntryButton, Bool (Audio Focus)").gameObject);
            GenericButton.name = "Mod Option Prefab, GenericButton";
            GenericButton.SetActive(false);
            
            Object.DestroyImmediate(GenericButton.GetComponentInChildren<CarouselController>());
            Object.DestroyImmediate(GenericButton.transform.Find("CarouselRect").gameObject);
            
            var controller = GenericButton.AddComponent<GenericButtonController>();
            
            GameObject buttonContainer = new GameObject("ButtonContainer", typeof(RectTransform), typeof(HorizontalLayoutGroup), typeof(ContentSizeFitter));
            
            buttonContainer.transform.SetParent(GenericButton.transform);
            
            var containerLayoutGroup = buttonContainer.GetComponent<HorizontalLayoutGroup>();
            
            containerLayoutGroup.childAlignment = TextAnchor.MiddleRight;
            containerLayoutGroup.padding = new RectOffset(8, 8, 0, 0);
            containerLayoutGroup.spacing = 8;
            containerLayoutGroup.childForceExpandWidth = true;
            containerLayoutGroup.childForceExpandHeight = false;
            
            var containerRectTransform = buttonContainer.GetComponent<RectTransform>();
            containerRectTransform.anchorMin = new Vector2(0, 0);
            containerRectTransform.anchorMax = new Vector2(1, 1);
            containerRectTransform.pivot = new Vector2(1, 0.5f);
            containerRectTransform.anchoredPosition = new Vector2(-6, 0);

            var containerSizeFitter = buttonContainer.GetComponent<ContentSizeFitter>();
            containerSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            containerSizeFitter.verticalFit = ContentSizeFitter.FitMode.MinSize;
            
            var buttonObject = Object.Instantiate(verticalLayout.Find("SettingsEntryButton, Bool (Audio Focus)").gameObject, buttonContainer.transform);
            buttonObject.name = "Button";
            
            Object.DestroyImmediate(buttonObject.GetComponentInChildren<CarouselController>());
            Object.DestroyImmediate(buttonObject.transform.Find("CarouselRect").gameObject);

            var buttonLayoutElement = buttonObject.GetComponent<LayoutElement>();
            //buttonLayoutElement.minWidth = 256;
            buttonLayoutElement.minHeight = 48;
            //buttonLayoutElement.preferredWidth = 256;

            var button = buttonObject.GetComponent<HGButton>();
            button.disablePointerClick = false;
            button.onClick.RemoveAllListeners();

            var buttonText = buttonObject.GetComponentInChildren<HGTextMeshProUGUI>();
            buttonText.enableAutoSizing = false;
            buttonText.fontSizeMin = buttonText.fontSize;
            buttonText.fontSizeMax = buttonText.fontSize;

            //var buttonSizeFitter = buttonObject.AddComponent<ContentSizeFitter>();
            //buttonSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

            //var textSizeFitter = buttonObject.GetComponentInChildren<HGTextMeshProUGUI>().gameObject.AddComponent<ContentSizeFitter>();
            //textSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        }

        public void Destroy()
        {
            Object.DestroyImmediate(GenericButton);
        }
    }
}