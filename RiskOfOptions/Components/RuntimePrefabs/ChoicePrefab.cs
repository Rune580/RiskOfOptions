using RiskOfOptions.Components.Options;
using RoR2;
using RoR2.UI;
using RoR2.UI.SkinControllers;
using UnityEngine;
using UnityEngine.UI;

namespace RiskOfOptions.Components.RuntimePrefabs
{
    // Todo come back to this and clean up.
    public class ChoicePrefab : IRuntimePrefab
    {
        public GameObject ChoiceButton { get; private set; }
        
        public void Instantiate(GameObject settingsPanel)
        {
            Transform verticalLayout = settingsPanel.transform.Find("SafeArea").Find("SubPanelArea")
                .Find("SettingsSubPanel, Audio").Find("Scroll View").Find("Viewport").Find("VerticalLayout");
            GameObject checkBox = verticalLayout.Find("SettingsEntryButton, Bool (Audio Focus)").gameObject;
            
            ChoiceButton = Object.Instantiate(settingsPanel.transform.Find("SafeArea").Find("SubPanelArea")
                .Find("SettingsSubPanel, Video").Find("Scroll View").Find("Viewport").Find("VerticalLayout")
                .Find("Option, Resolution").gameObject);

            var carouselRect = ChoiceButton.transform.Find("CarouselRect");
            
             Object.DestroyImmediate(carouselRect.GetComponent<ResolutionControl>()); // Removing this entirely since it seems to mostly be made for resolution stuff.
             Object.DestroyImmediate(carouselRect.Find("RefreshRateDropdown").gameObject); // I only really need one Drop down element.
             Object.DestroyImmediate(carouselRect.Find("ApplyButton").gameObject); // I think most use cases don't need an apply button. If I think otherwise later I can make this optional
             Object.DestroyImmediate(ChoiceButton.GetComponent<SelectableDescriptionUpdater>());
             Object.DestroyImmediate(ChoiceButton.GetComponent<PanelSkinController>());
             Object.DestroyImmediate(ChoiceButton.GetComponent<Image>());
             
             Object.Instantiate(checkBox.transform.Find("BaseOutline").gameObject, ChoiceButton.transform);

             GameObject dropDownHoverOutline = Object.Instantiate(checkBox.transform.Find("HoverOutline").gameObject, ChoiceButton.transform);
             ChoiceButton.SetActive(false);
            
             HGButton button = ChoiceButton.AddComponent(checkBox.GetComponent<HGButton>());
             button.imageOnHover = dropDownHoverOutline.GetComponent<Image>();

             ChoiceButton.AddComponent(checkBox.GetComponent<ButtonSkinController>());
             
             var dropDownImage = ChoiceButton.AddComponent(checkBox.GetComponent<Image>());
             var dropDownTargetGraphic = dropDownImage;
            
             button.targetGraphic = dropDownTargetGraphic;
             button.navigation = new Navigation();
             button.onClick.RemoveAllListeners();

             Transform ChoiceButtonLabel = ChoiceButton.transform.Find("Text, Name");
             ChoiceButton.AddComponent<DropDownController>().nameLabel = ChoiceButtonLabel.GetComponent<LanguageTextMeshController>();

            // todo: fix this in the Unity prefab and update the asset bundle?
            // values obtained from cross-referencing other option prefabs using Unity Explorer
            RectTransform fixPosition = (RectTransform)ChoiceButtonLabel;
            fixPosition.anchoredPosition = Vector2.zero;
            fixPosition.pivot = new Vector2(0.5f, 0.5f);
            fixPosition.sizeDelta = new Vector2(-24, -8);
            fixPosition.anchorMax = new Vector2(0.5f, 1f);
             
             var dropDownGameObject = ChoiceButton.transform.Find("CarouselRect").Find("ResolutionDropdown").gameObject;
             dropDownGameObject.name = "Dropdown";
            
             var dropDownLayoutElement = dropDownGameObject.GetComponent<LayoutElement>();
             dropDownLayoutElement.minWidth = 300;
             dropDownLayoutElement.preferredWidth = 300;

             Object.DestroyImmediate(dropDownGameObject.GetComponent<MPDropdown>());

             dropDownGameObject.transform.Find("Label").gameObject.AddComponent<LanguageTextMeshController>();

             var rooDropDown = dropDownGameObject.AddComponent<RooDropdown>();
             //rooDropDown.checkMarkSprite = dropDownGameObject.transform.Find("Template").Find("Viewport").Find("Content").Find("Item").Find("Item Checkmark").GetComponent<Image>().sprite;
             rooDropDown.defaultColors = checkBox.GetComponent<HGButton>().colors;
             
             Object.DestroyImmediate(dropDownGameObject.transform.Find("Template").gameObject);

             var choiceItemPrefab = Object.Instantiate(checkBox, ChoiceButton.transform);
             choiceItemPrefab.name = "Choice Item Prefab";
             choiceItemPrefab.SetActive(false);

             var choiceItemButton = choiceItemPrefab.GetComponent<HGButton>();
             choiceItemButton.onClick.RemoveAllListeners();
             choiceItemButton.disablePointerClick = false;

             Object.DestroyImmediate(choiceItemPrefab.GetComponent<CarouselController>());
             Object.DestroyImmediate(choiceItemPrefab.transform.Find("CarouselRect").gameObject);

             var choiceItemLayoutElement = choiceItemPrefab.GetComponent<LayoutElement>();
             choiceItemLayoutElement.minHeight = 40;
             choiceItemLayoutElement.preferredHeight = 40;
            
             rooDropDown.choiceItemPrefab = choiceItemPrefab;
             
             GameObject dropDownTemplate = Object.Instantiate(
                     settingsPanel.transform.Find("SafeArea").Find("SubPanelArea").Find("SettingsSubPanel, Audio")
                         .gameObject, dropDownGameObject.transform);
             
             dropDownTemplate.name = "Drop Down Parent";
             dropDownTemplate.SetActive(false);

             Transform templateLayout = dropDownTemplate.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout");
             Object.DestroyImmediate(templateLayout.transform.Find("SettingsEntryButton, Bool (Audio Focus)").gameObject);
             Object.DestroyImmediate(templateLayout.transform.Find("SettingsEntryButton, Slider (Master Volume)").gameObject);
             Object.DestroyImmediate(templateLayout.transform.Find("SettingsEntryButton, Slider (SFX Volume)").gameObject);
             Object.DestroyImmediate(templateLayout.transform.Find("SettingsEntryButton, Slider (MSX Volume)").gameObject);

             Object.DestroyImmediate(dropDownTemplate.GetComponent<SettingsPanelController>());
             Object.DestroyImmediate(dropDownTemplate.GetComponent<HGButtonHistory>());
             Object.DestroyImmediate(dropDownTemplate.GetComponent<OnEnableEvent>());
             Object.DestroyImmediate(dropDownTemplate.GetComponent<UIJuice>());

             var templateCanvas = dropDownTemplate.GetOrAddComponent<Canvas>();

             templateCanvas.overrideSorting = true;
             templateCanvas.sortingOrder = 30000;

             dropDownTemplate.GetOrAddComponent<GraphicRaycaster>();
             var dropDownCanvasGroup = dropDownTemplate.GetOrAddComponent<CanvasGroup>();
             dropDownCanvasGroup.alpha = 1f;

             var templateRectTransform = dropDownTemplate.GetComponent<RectTransform>();

             templateRectTransform.anchorMin = new Vector2(0, 0);
             templateRectTransform.anchorMax = new Vector2(1, 0);
             templateRectTransform.pivot = new Vector2(0.5f, 1);
             templateRectTransform.sizeDelta = new Vector2(0, 300f);
             templateRectTransform.anchoredPosition = Vector2.zero;

             var scrollbarRectTransform = dropDownTemplate.transform.Find("Scroll View").Find("Scrollbar Vertical").GetComponent<RectTransform>();

             scrollbarRectTransform.anchorMin = new Vector2(1, 0.02f);
             scrollbarRectTransform.anchorMax = new Vector2(1, 0.98f);
             scrollbarRectTransform.sizeDelta = new Vector2(24, 0);
             scrollbarRectTransform.anchoredPosition = new Vector2(-26, 0);

             GameObject dropDownContent = dropDownTemplate.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").gameObject;
             
             dropDownContent.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(4, 18, 4, 4);

             rooDropDown.content = dropDownContent;
             rooDropDown.template = dropDownTemplate;
             
             ChoiceButton.name = "Mod Option Prefab, Choice";
        }

        public void Destroy()
        {
            Object.DestroyImmediate(ChoiceButton);
        }
    }
}