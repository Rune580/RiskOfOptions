using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LeTai.Asset.TranslucentImage;
using R2API;
using R2API.Utils;
using RiskOfOptions.Containers;
using RiskOfOptions.Options;
using RiskOfOptions.Resources;
using RoR2;
using RoR2.UI;
using RoR2.UI.SkinControllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 618

namespace RiskOfOptions.Components.OptionComponents
{
    public class ModOptionPanelController : MonoBehaviour
    {
        public bool initialized = false;

        public GameObject modListPanel;
        public GameObject modListHighlight;

        public GameObject warningPanel;
        
        private List<GameObject> _toDestroy = new List<GameObject>();

        private GameObject _modDescriptionPanel;
        private GameObject _categoryHeader;
        private GameObject _optionsPanel;
        private GameObject _optionDescriptionPanel;
        private GameObject _categoryHeaderHighlight;

        private GameObject _checkBoxPrefab;
        private GameObject _sliderPrefab;
        private GameObject _keyBindPrefab;
        private GameObject _dropDownPrefab;
        private GameObject _inputFieldPrefab;

        private GameObject _leftButton;
        private GameObject _rightButton;

        //private GameObject _leftGlyph;
        //private GameObject _rightGlyph;

        private OverrideController[] _controllers;

        public Color warningColor = Color.red;

        public float degreesPerSecond = 2f;

        private bool _warningShown = false;

        private IEnumerator _animateRoutine;


        public void Start()
        {
            CreatePrefabs();
            CreatePanel();
            AddPanelsToSettings();
            CheckIfRestartNeeded();

            ModSettingsManager.InstanceModOptionPanelController = this;
        }
        private void CreatePrefabs()
        {
            Transform subPanelArea = transform.Find("SafeArea").Find("SubPanelArea");
            Transform headerArea = transform.Find("SafeArea").Find("HeaderContainer").Find("Header (JUICED)");

            //_leftGlyph = GameObject.Instantiate(headerArea.Find("GenericGlyph (Left)").gameObject);
            //_rightGlyph = GameObject.Instantiate(headerArea.Find("GenericGlyph (Right)").gameObject);

            //_leftGlyph.SetActive(false);
            //_rightGlyph.SetActive(false);

            //_leftGlyph.GetComponentInChildren<HGTextMeshProUGUI>().SetText("<sprite=\"tmpsprXboxOneGlyphs\" name=\"texXBoxOneGlyphs_5\">");
            //_rightGlyph.GetComponentInChildren<HGTextMeshProUGUI>().SetText("<sprite=\"tmpsprXboxOneGlyphs\" name=\"texXBoxOneGlyphs_9\">");

            Prefabs.Gdp = GameObject.Instantiate(subPanelArea.Find("GenericDescriptionPanel").gameObject);

            GameObject.DestroyImmediate(Prefabs.Gdp.GetComponentInChildren<DisableIfTextIsEmpty>());
            GameObject.DestroyImmediate(Prefabs.Gdp.GetComponentInChildren<LanguageTextMeshController>());
            GameObject.DestroyImmediate(Prefabs.Gdp.transform.Find("ContentSizeFitter").Find("BlurPanel").gameObject);
            GameObject.DestroyImmediate(Prefabs.Gdp.transform.Find("ContentSizeFitter").Find("CornerRect").gameObject);

            GameObject audioPanel = subPanelArea.Find("SettingsSubPanel, Audio").gameObject;

            Prefabs.MoPanelPrefab = GameObject.Instantiate(audioPanel);
            Prefabs.MoPanelPrefab.name = "SettingsSubPanel, Mod Options";

            Prefabs.MoHeaderButtonPrefab = GameObject.Instantiate(headerArea.Find("GenericHeaderButton (Audio)").gameObject);

            GameObject verticalLayout = Prefabs.MoPanelPrefab.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").gameObject;

            Prefabs.ModButtonPrefab = GameObject.Instantiate(verticalLayout.transform.Find("SettingsEntryButton, Bool (Audio Focus)").gameObject);

            _checkBoxPrefab = GameObject.Instantiate(Prefabs.ModButtonPrefab);

            _sliderPrefab = GameObject.Instantiate(verticalLayout.transform.Find("SettingsEntryButton, Slider (Master Volume)").gameObject);
            _keyBindPrefab = GameObject.Instantiate(subPanelArea.Find("SettingsSubPanel, Controls (M&KB)").Find("Scroll View").Find("Viewport").Find("VerticalLayout").Find("SettingsEntryButton, Binding (Jump)").gameObject);
            _dropDownPrefab = GameObject.Instantiate(subPanelArea.Find("SettingsSubPanel, Video").Find("Scroll View").Find("Viewport").Find("VerticalLayout").Find("Option, Resolution").gameObject);
            _inputFieldPrefab = GameObject.Instantiate(_checkBoxPrefab);

            _dropDownPrefab.SetActive(false);

            #region KeybindPrefabCleaning

            GameObject.DestroyImmediate(_keyBindPrefab.GetComponentInChildren<InputBindingControl>());
            GameObject.DestroyImmediate(_keyBindPrefab.GetComponentInChildren<InputBindingDisplayController>());

            _keyBindPrefab.AddComponent<KeybindController>();

            #endregion

            #region DropDown Prefab Setup

            GameObject.DestroyImmediate(_dropDownPrefab.transform.Find("CarouselRect").GetComponent<ResolutionControl>()); // Removing this entirely since it seems to mostly be made for resolution stuff.
            GameObject.DestroyImmediate(_dropDownPrefab.transform.Find("CarouselRect").Find("RefreshRateDropdown").gameObject); // I only really need one Drop down element.
            GameObject.DestroyImmediate(_dropDownPrefab.transform.Find("CarouselRect").Find("ApplyButton").gameObject); // I think most use cases don't need an apply button. If I think otherwise later I can make this optional
            GameObject.DestroyImmediate(_dropDownPrefab.GetComponent<SelectableDescriptionUpdater>());
            GameObject.DestroyImmediate(_dropDownPrefab.GetComponent<PanelSkinController>());
            GameObject.DestroyImmediate(_dropDownPrefab.GetComponent<Image>());


            GameObject.Instantiate(_checkBoxPrefab.transform.Find("BaseOutline").gameObject, _dropDownPrefab.transform);
            GameObject dropDownHoverOutline = GameObject.Instantiate(_checkBoxPrefab.transform.Find("HoverOutline").gameObject, _dropDownPrefab.transform);

            HGButton dropDownButton = _dropDownPrefab.AddComponent<HGButton>(_checkBoxPrefab.GetComponent<HGButton>());

            dropDownButton.imageOnHover = dropDownHoverOutline.GetComponent<Image>();

            var dropDownImage = _dropDownPrefab.AddComponent(_checkBoxPrefab.GetComponent<Image>());

            _dropDownPrefab.AddComponent<ButtonSkinController>(_checkBoxPrefab.GetComponent<ButtonSkinController>());

            var dropDownTargetGraphic = dropDownImage;

            dropDownButton.targetGraphic = dropDownTargetGraphic;
            dropDownButton.navigation = new Navigation();

            dropDownButton.onClick.RemoveAllListeners();

            _dropDownPrefab.AddComponent<DropDownController>().nameLabel = _dropDownPrefab.transform.Find("Text, Name").GetComponent<LanguageTextMeshController>();

            var dropDownGameObject = _dropDownPrefab.transform.Find("CarouselRect").Find("ResolutionDropdown").gameObject;

            dropDownGameObject.name = "Dropdown";

            var dropDownLayoutElement = dropDownGameObject.GetComponent<LayoutElement>();
            dropDownLayoutElement.minWidth = 300;
            dropDownLayoutElement.preferredWidth = 300;

            if (!RooDropdown.CheckMarkSprite)
            {
                RooDropdown.CheckMarkSprite = dropDownGameObject.transform.Find("Template").Find("Viewport").Find("Content").Find("Item").Find("Item Checkmark").GetComponent<Image>().sprite;
            }

            GameObject.DestroyImmediate(dropDownGameObject.GetComponent<MPDropdown>());
            GameObject.DestroyImmediate(dropDownGameObject.transform.Find("Template").gameObject);

            dropDownGameObject.AddComponent<RooDropdown>().colors = _checkBoxPrefab.GetComponent<HGButton>().colors;


            #endregion

            #region InputField Setup

            _inputFieldPrefab.SetActive(false);

            _inputFieldPrefab.name = "Input Field";

            GameObject.DestroyImmediate(_inputFieldPrefab.GetComponentInChildren<CarouselController>());
            //GameObject.DestroyImmediate(_inputFieldPrefab.GetComponentInChildren<ButtonSkinController>());
            //GameObject.DestroyImmediate(_inputFieldPrefab.GetComponentInChildren<HGButton>());
            GameObject.DestroyImmediate(_inputFieldPrefab.transform.Find("CarouselRect").gameObject);

            _inputFieldPrefab.AddComponent<InputFieldController>();

            ColorBlock inputColors = _inputFieldPrefab.GetComponent<HGButton>().colors;

            GameObject textPreview = new GameObject("Text Preview", typeof(RectTransform), typeof(HorizontalLayoutGroup));

            textPreview.transform.SetParent(_inputFieldPrefab.transform);

            var textPreviewHorizontalLayoutGroup = textPreview.GetComponent<HorizontalLayoutGroup>();

            textPreviewHorizontalLayoutGroup.childAlignment = TextAnchor.MiddleRight;
            textPreviewHorizontalLayoutGroup.padding = new RectOffset(8, 8, 0, 0);
            textPreviewHorizontalLayoutGroup.spacing = 8;
            textPreviewHorizontalLayoutGroup.childForceExpandWidth = false;
            textPreviewHorizontalLayoutGroup.childForceExpandHeight = false;

            var textPreviewRectTransform = textPreview.GetComponent<RectTransform>();

            textPreviewRectTransform.anchorMin = new Vector2(0, 0);
            textPreviewRectTransform.anchorMax = new Vector2(1, 1);
            textPreviewRectTransform.pivot = new Vector2(1, 0.5f);
            textPreviewRectTransform.anchoredPosition = new Vector2(-6, 0);


            GameObject placeHolderTextArea = new GameObject("Text Area", typeof(RectTransform), typeof(RectMask2D), typeof(CanvasRenderer), typeof(LayoutElement), typeof(GraphicRaycaster));

            placeHolderTextArea.transform.SetParent(textPreview.transform);

            placeHolderTextArea.AddComponent<Image>(_checkBoxPrefab.GetComponent<Image>());

            var placeHolderLayoutElement = placeHolderTextArea.GetComponent<LayoutElement>();

            placeHolderLayoutElement.minWidth = 256;
            placeHolderLayoutElement.minHeight = 48;
            placeHolderLayoutElement.preferredWidth = 256;

            placeHolderTextArea.name = "Text Area";


            GameObject textPlaceHolder = new GameObject("Text", typeof(RectTransform), typeof(RectMask2D), typeof(CanvasRenderer));

            textPlaceHolder.transform.SetParent(placeHolderTextArea.transform);

            textPlaceHolder.AddComponent<HGTextMeshProUGUI>(_inputFieldPrefab.transform.Find("ButtonText").GetComponent<HGTextMeshProUGUI>());

            textPlaceHolder.AddComponent<LanguageTextMeshController>();


            var textPlaceHolderRectTransform = textPlaceHolder.GetComponent<RectTransform>();

            textPlaceHolderRectTransform.anchorMin = new Vector2(0.03f, 0.5f);
            textPlaceHolderRectTransform.anchorMax = new Vector2(1, 0.5f);
            textPlaceHolderRectTransform.sizeDelta = new Vector2(0, 100);

            textPlaceHolder.name = "Text";

            GameObject textCanvas = new GameObject("Text Overlay", typeof(RectTransform), typeof(RectMask2D), typeof(Canvas), typeof(GraphicRaycaster), typeof(CanvasGroup));

            textCanvas.SetActive(false);
            textCanvas.transform.SetParent(_inputFieldPrefab.transform);

            //var textCanvasImage = textCanvas.AddComponent<Image>(_inputFieldPrefab.GetComponent<Image>());

            //textCanvas.AddComponent<TranslucentImage>(Prefabs.MoPanelPrefab.transform.Find("Scroll View").Find("BlurPanel").gameObject.GetComponent<TranslucentImage>());

            //textCanvasImage.color = inputColors.normalColor;

            textCanvas.AddComponent<RooInputFieldOverlay>();

            var textCanvasRectTransform = textCanvas.GetComponent<RectTransform>();

            textCanvasRectTransform.anchoredPosition = new Vector2(0, -50);
            textCanvasRectTransform.sizeDelta = new Vector2(525, 48);

            textCanvas.name = "Text Overlay";

            GameObject.Instantiate(Prefabs.MoPanelPrefab.transform.Find("Scroll View").Find("BlurPanel").gameObject, textCanvas.transform);
            GameObject.Instantiate(Prefabs.MoPanelPrefab.transform.Find("Scroll View").Find("ImagePanel").gameObject, textCanvas.transform);

            //GameObject.Instantiate(_inputFieldPrefab.transform.Find("BaseOutline"), textCanvas.transform);


            GameObject textArea = new GameObject("Text Area", typeof(RectTransform), typeof(RectMask2D), typeof(CanvasRenderer), typeof(GraphicRaycaster));

            textArea.transform.SetParent(textCanvas.transform);

            var textAreaRectTransform = textArea.GetComponent<RectTransform>();

            textAreaRectTransform.anchorMin = new Vector2(0, 0);
            textAreaRectTransform.anchorMax = new Vector2(1, 1);
            textAreaRectTransform.anchoredPosition = Vector2.zero;
            textAreaRectTransform.sizeDelta = Vector2.zero;

            var textAreaImage = textArea.AddComponent<Image>(_inputFieldPrefab.GetComponent<Image>());

            textAreaImage.color = inputColors.normalColor;

            textArea.name = "Text Area";


            GameObject inputText = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer)); 

            inputText.transform.SetParent(textAreaRectTransform.transform);

            inputText.AddComponent<HGTextMeshProUGUI>(_inputFieldPrefab.transform.Find("ButtonText").GetComponent<HGTextMeshProUGUI>());

            var inputTextRectTransform = inputText.GetComponent<RectTransform>();

            inputTextRectTransform.anchorMin = new Vector2(0.02f, 0);
            inputTextRectTransform.anchorMax = new Vector2(0.98f, 1);
            inputTextRectTransform.anchoredPosition = Vector2.zero; 
            inputTextRectTransform.sizeDelta = Vector2.zero;

            inputText.name = "Text";

            var tmpInputField = placeHolderTextArea.AddComponent<RooInputField>();

            tmpInputField.overlay = textCanvas;

            tmpInputField.textViewport = textAreaRectTransform;
            tmpInputField.textComponent = inputText.GetComponent<HGTextMeshProUGUI>();

            tmpInputField.colors = inputColors;


            //textCanvas.transform.SetAsFirstSibling();
            //textPreview.transform.SetAsFirstSibling();
            

            #endregion

            _checkBoxPrefab.SetActive(false);
            _sliderPrefab.SetActive(false);
            _keyBindPrefab.SetActive(false);

            if (!RooDropdown.CheckBoxPrefab)
            {
                RooDropdown.CheckBoxPrefab = _checkBoxPrefab;
            }

            if (!RooDropdown.PanelPrefab)
            {
                RooDropdown.PanelPrefab = Prefabs.MoPanelPrefab;
            }

            GameObject.DestroyImmediate(Prefabs.ModButtonPrefab.GetComponentInChildren<CarouselController>());
            GameObject.DestroyImmediate(Prefabs.ModButtonPrefab.GetComponentInChildren<ButtonSkinController>());
            GameObject.DestroyImmediate(Prefabs.ModButtonPrefab.transform.Find("CarouselRect").gameObject);
            
            

            _leftButton = GameObject.Instantiate(Prefabs.ModButtonPrefab);
            GameObject.DestroyImmediate(_leftButton.GetComponent<LayoutElement>());

            var leftButtonRectTransform = _leftButton.GetComponent<RectTransform>();
            leftButtonRectTransform.sizeDelta = new Vector2(64, 64);
            leftButtonRectTransform.anchoredPosition = new Vector2(60, -54);

            _leftButton.GetComponentInChildren<LanguageTextMeshController>().token = LanguageTokens.LeftPageButton;

            var leftButtonText = _leftButton.GetComponentInChildren<HGTextMeshProUGUI>();
            leftButtonText.alignment = TextAlignmentOptions.Midline;
            leftButtonText.enableAutoSizing = false;
            leftButtonText.fontSize = 72;

            _rightButton = GameObject.Instantiate(_leftButton);

            _rightButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(1064, -54);

            _rightButton.GetComponentInChildren<LanguageTextMeshController>().token = LanguageTokens.RightPageButton;

            _leftButton.name = "Previous Category Page Button";
            _rightButton.name = "Next Category Page Button";

            _leftButton.SetActive(false);
            _rightButton.SetActive(false);

            // Converting a HGButton to a ROOButton so we can modify it better.

            HGButton oldButton = Prefabs.ModButtonPrefab.GetComponent<HGButton>();

            bool allowAllEventSystems = oldButton.allowAllEventSystems;
            bool submitOnPointerUp = oldButton.submitOnPointerUp;
            UILayerKey requiredTopLayer = oldButton.requiredTopLayer;
            UnityEngine.Events.UnityEvent onFindSelectableLeft = oldButton.onFindSelectableLeft;
            UnityEngine.Events.UnityEvent onFindSelectableRight = oldButton.onFindSelectableRight;
            UnityEngine.Events.UnityEvent onSelect = oldButton.onSelect;
            UnityEngine.Events.UnityEvent onDeselect = oldButton.onDeselect;
            bool defaultFallbackButton = oldButton.defaultFallbackButton;
            UnityEngine.UI.Button.ButtonClickedEvent buttonClickedEvent = oldButton.onClick;
            UnityEngine.UI.ColorBlock colors = oldButton.colors;
            bool showImageOnHover = oldButton.showImageOnHover;
            UnityEngine.UI.Image imageOnHover = oldButton.imageOnHover;
            UnityEngine.UI.Image imageOnInteractable = oldButton.imageOnInteractable;
            bool updateTextOnHover = oldButton.updateTextOnHover;
            LanguageTextMeshController hoverLanguageTextMeshController = oldButton.hoverLanguageTextMeshController;
            string hoverToken = oldButton.hoverToken;
            string uiClickSoundOverride = oldButton.uiClickSoundOverride;

            GameObject.DestroyImmediate(oldButton);

            colors.disabledColor = Prefabs.MoHeaderButtonPrefab.GetComponent<HGButton>().colors.disabledColor;

            RooModListButton newButton = Prefabs.ModButtonPrefab.AddComponent<RooModListButton>();
            newButton.allowAllEventSystems = allowAllEventSystems;
            newButton.submitOnPointerUp = submitOnPointerUp;
            newButton.requiredTopLayer = requiredTopLayer;
            newButton.onFindSelectableLeft = onFindSelectableLeft;
            newButton.onFindSelectableRight = onFindSelectableRight;
            newButton.onSelect = onSelect;
            newButton.onDeselect = onDeselect;
            newButton.defaultFallbackButton = defaultFallbackButton;
            newButton.onClick = buttonClickedEvent;
            newButton.colors = colors;
            newButton.showImageOnHover = showImageOnHover;
            newButton.imageOnHover = imageOnHover;
            newButton.imageOnInteractable = imageOnInteractable;
            newButton.updateTextOnHover = updateTextOnHover;
            newButton.hoverLanguageTextMeshController = hoverLanguageTextMeshController;
            newButton.hoverToken = hoverToken;
            newButton.uiClickSoundOverride = uiClickSoundOverride;

            newButton.interactable = true;
            newButton.enabled = true;
            newButton.disablePointerClick = false;
            newButton.onClick.RemoveAllListeners();

            RectTransform buttonTextRectTransform = Prefabs.ModButtonPrefab.transform.Find("ButtonText").GetComponent<RectTransform>();

            buttonTextRectTransform.anchorMin = new Vector2(0.19f, 0);
            buttonTextRectTransform.anchorMax = new Vector2(1, 1);

            UnityEngine.Object.DestroyImmediate(Prefabs.ModButtonPrefab.GetComponent<LanguageTextMeshController>());

            GameObject modIconGameObject = new GameObject();

            modIconGameObject.name = "ModIcon";

            RectTransform modIconRectTransform = modIconGameObject.AddComponent<RectTransform>();
            modIconGameObject.AddComponent<CanvasRenderer>();
            modIconGameObject.AddComponent<UnityEngine.UI.Image>().preserveAspect = true;

            modIconRectTransform.anchorMin = new Vector2(0.04f, 0.13f);
            modIconRectTransform.anchorMax = new Vector2(0.19f, 0.86f);

            modIconRectTransform.pivot = new Vector2(0.5f, 0.5f);

            //modIconRectTransform.localPosition = new Vector3(-142, -0.32f, 0);

            modIconGameObject.transform.SetParent(Prefabs.ModButtonPrefab.transform);

            GameObject iconOutline = GameObject.Instantiate(Prefabs.ModButtonPrefab.transform.Find("BaseOutline").gameObject, modIconRectTransform);

            RectTransform iconOutlineRectTransform = iconOutline.GetComponent<RectTransform>();

            iconOutlineRectTransform.sizeDelta = Vector2.zero;
            iconOutlineRectTransform.anchoredPosition = Vector2.zero;

            iconOutlineRectTransform.localScale = new Vector3(0.94f, 1.16f, 1);

            Prefabs.ModButtonPrefab.SetActive(false);
        }

        private void CreatePanel()
        {
            Prefabs.MoPanelPrefab.name = "SettingsSubPanel, Mod Options";

            Transform headerArea = transform.Find("SafeArea").Find("HeaderContainer").Find("Header (JUICED)");

            GameObject verticalLayout = Prefabs.MoPanelPrefab.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").gameObject;

            UnityEngine.Object.DestroyImmediate(verticalLayout.transform.Find("SettingsEntryButton, Slider (Master Volume)").gameObject);
            UnityEngine.Object.DestroyImmediate(verticalLayout.transform.Find("SettingsEntryButton, Slider (SFX Volume)").gameObject);
            UnityEngine.Object.DestroyImmediate(verticalLayout.transform.Find("SettingsEntryButton, Slider (MSX Volume)").gameObject);
            UnityEngine.Object.DestroyImmediate(verticalLayout.transform.Find("SettingsEntryButton, Bool (Audio Focus)").gameObject);

            Prefabs.MoCanvas = GameObject.Instantiate(Prefabs.MoPanelPrefab, Prefabs.MoPanelPrefab.transform.parent);

            Prefabs.MoCanvas.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);

            GameObject.DestroyImmediate(Prefabs.MoCanvas.GetComponent<SettingsPanelController>());
            GameObject.DestroyImmediate(Prefabs.MoCanvas.GetComponent<UnityEngine.UI.Image>());
            GameObject.DestroyImmediate(Prefabs.MoCanvas.GetComponent<HGButtonHistory>());

            GameObject.DestroyImmediate(Prefabs.MoCanvas.transform.Find("Scroll View").gameObject);

            Prefabs.MoCanvas.AddComponent<GenericDescriptionController>();

            Prefabs.MoCanvas.name = "SettingsSubPanel, Mod Options";

            modListPanel = GameObject.Instantiate(Prefabs.MoPanelPrefab, Prefabs.MoCanvas.transform);

            modListPanel.GetComponent<RectTransform>().anchorMax = new Vector2(0.25f, 1f);

            modListPanel.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").GetComponent<VerticalLayoutGroup>().spacing = 6;

            //modListPanel.transform.Find("Scroll View").GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0.194f);

            modListPanel.SetActive(true);

            modListPanel.name = "Mod List Panel";

            modListPanel.AddComponent<ModListHeaderController>();


            modListHighlight = GameObject.Instantiate(GetComponent<HGHeaderNavigationController>().headerHighlightObject, GetComponent<HGHeaderNavigationController>().headerHighlightObject.transform.parent);

            foreach (var imageComp in modListHighlight.GetComponentsInChildren<UnityEngine.UI.Image>())
            {
                imageComp.maskable = false;
            }

            modListHighlight.SetActive(false);


            HGHeaderNavigationController modListController = modListPanel.AddComponent<HGHeaderNavigationController>();

            modListController.headerHighlightObject = modListHighlight;
            modListController.unselectedTextColor = Color.white;

            modListController.makeSelectedHeaderButtonNoninteractable = true;


            _modDescriptionPanel = GameObject.Instantiate(Prefabs.MoPanelPrefab, Prefabs.MoCanvas.transform);

            _modDescriptionPanel.GetComponent<RectTransform>().anchorMin = new Vector2(0.275f, 0f);
            _modDescriptionPanel.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);

            Transform mdpVerticalLayout = _modDescriptionPanel.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout");

            GameObject.Instantiate(Prefabs.Gdp, mdpVerticalLayout);

            _modDescriptionPanel.SetActive(true);

            _modDescriptionPanel.name = "Mod Description Panel";



            warningPanel = GameObject.Instantiate(Prefabs.Gdp, modListPanel.transform);

            warningPanel.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
            warningPanel.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0f);

            warningPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -9f);

            GameObject warningBlur = GameObject.Instantiate(modListPanel.transform.Find("Scroll View").Find("BlurPanel").gameObject, warningPanel.transform);
            GameObject warningImage = GameObject.Instantiate(modListPanel.transform.Find("Scroll View").Find("ImagePanel").gameObject, warningPanel.transform);

            //GameObject.DestroyImmediate(warningPanel.transform.Find("ContentSizeFitter").GetComponent<ContentSizeFitter>());
            //GameObject.DestroyImmediate(warningPanel.transform.Find("ContentSizeFitter").GetComponent<VerticalLayoutGroup>());

            warningPanel.transform.Find("ContentSizeFitter").GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);

            RectTransform warningDescriptionRectTransform = warningPanel.transform.Find("ContentSizeFitter").Find("DescriptionText").GetComponent<RectTransform>();

            warningDescriptionRectTransform.anchorMin = new Vector2(0.1f, 0);
            warningDescriptionRectTransform.anchorMax = new Vector2(1, 0.5f);

            LayoutElement warningDescriptionLayoutElement = warningDescriptionRectTransform.gameObject.AddComponent<LayoutElement>();

            warningDescriptionLayoutElement.ignoreLayout = true;

            warningPanel.transform.Find("ContentSizeFitter").SetAsLastSibling();

            warningPanel.AddComponent<RectMask2D>();

            warningBlur.GetComponent<TranslucentImage>().color = warningColor;
            warningImage.GetComponent<UnityEngine.UI.Image>().color = warningColor;

            warningPanel.name = "Warning Panel";

            warningPanel.SetActive(false);



            GameObject restartIconGameObject = new GameObject();

            restartIconGameObject.name = "RestartIcon";

            RectTransform restartIconRectTransform = restartIconGameObject.AddComponent<RectTransform>();
            restartIconGameObject.AddComponent<CanvasRenderer>();

            LayoutElement restartIconLayoutElement = restartIconGameObject.AddComponent<LayoutElement>();
            restartIconLayoutElement.ignoreLayout = true;

            Image restartIcon = restartIconGameObject.AddComponent<UnityEngine.UI.Image>();
            restartIcon.sprite = Assets.Load<Sprite>("assets/RiskOfOptions/ror2RestartSymbol.png");
            restartIcon.preserveAspect = true;

            restartIconRectTransform.pivot = new Vector2(0.5f, 0.5f);

            restartIconRectTransform.localScale = new Vector3(0.9f, 0.9f, 0.9f);

            restartIconRectTransform.anchorMin = new Vector2(0.0f, 0.0f);
            restartIconRectTransform.anchorMax = new Vector2(0.11f, 0.0f);

            restartIconRectTransform.anchoredPosition = new Vector2(0, -6);
            restartIconRectTransform.sizeDelta = new Vector2(0, 32);

            restartIconGameObject.transform.SetParent(warningPanel.transform.Find("ContentSizeFitter"));

            restartIconGameObject.transform.SetAsLastSibling();


            _categoryHeader = GameObject.Instantiate(Prefabs.MoPanelPrefab, Prefabs.MoCanvas.transform);

            _categoryHeader.GetComponent<RectTransform>().anchorMin = new Vector2(0.275f, 0.86f);
            _categoryHeader.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);

            _categoryHeader.SetActive(false);

            _categoryHeader.name = "Category Headers";

            //_leftButton = GameObject.Instantiate(_checkBoxPrefab, _categoryHeader.transform.Find("Scroll View"));

            GameObject.DestroyImmediate(_categoryHeader.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").gameObject);
            GameObject.DestroyImmediate(_categoryHeader.transform.Find("Scroll View").Find("Scrollbar Vertical").gameObject);

            GameObject headers = GameObject.Instantiate(headerArea.gameObject, _categoryHeader.transform.Find("Scroll View").Find("Viewport"));
            headers.name = "Categories (JUICED)";

            GameObject.DestroyImmediate(headers.GetComponent<OnEnableEvent>());
            GameObject.DestroyImmediate(headers.GetComponent<AwakeEvent>());

            RectTransform rt = headers.GetComponent<RectTransform>();

            rt.pivot = new Vector2(0.5f, 0.5f);

            rt.anchorMin = new Vector2(0f, 0.2f);
            rt.anchorMax = new Vector2(0f, 0.8f);

            rt.anchoredPosition = new Vector2(0, 0);

            var localPosition = headers.transform.localPosition;

            localPosition = new Vector3(localPosition.x, -47f, localPosition.z);
            headers.transform.localPosition = localPosition;

            headers.GetComponent<CanvasGroup>().alpha = 1;

            RectTransform[] oldButtons = headers.GetComponentsInChildren<RectTransform>();

            foreach (var oldButton in oldButtons)
            {
                if (oldButton != null)
                {
                    if (oldButton != headers.GetComponent<RectTransform>())
                    {
                        GameObject.DestroyImmediate(oldButton.gameObject);
                    }
                }
            }


            _categoryHeaderHighlight = GameObject.Instantiate(GetComponent<HGHeaderNavigationController>().headerHighlightObject, GetComponent<HGHeaderNavigationController>().headerHighlightObject.transform.parent);

            _categoryHeaderHighlight.SetActive(false);

            HGHeaderNavigationController categoryController = headers.AddComponent<HGHeaderNavigationController>();

            categoryController.headerHighlightObject = _categoryHeaderHighlight;
            categoryController.unselectedTextColor = Color.white;

            categoryController.makeSelectedHeaderButtonNoninteractable = true;

            var categoryViewPortHeaderRectTransform = _categoryHeader.transform.Find("Scroll View").Find("Viewport").gameObject.GetComponent<RectTransform>();

            categoryViewPortHeaderRectTransform.anchorMin = new Vector2(0.11f, 0);
            categoryViewPortHeaderRectTransform.anchorMax = new Vector2(0.89f, 1);

            UISkinData skinData = _categoryHeader.transform.Find("Scroll View").GetComponent<ScrollRectSkinController>().skinData;
            UILayerKey layerKey = _categoryHeader.transform.Find("Scroll View").GetComponent<HGScrollRectHelper>().requiredTopLayer;

            GameObject.DestroyImmediate(_categoryHeader.transform.Find("Scroll View").GetComponent<ScrollRectSkinController>());
            GameObject.DestroyImmediate(_categoryHeader.transform.Find("Scroll View").GetComponent<HGScrollRectHelper>());
            GameObject.DestroyImmediate(_categoryHeader.transform.Find("Scroll View").GetComponent<ScrollRect>());

            CategoryScrollRect categoryScrollRect = _categoryHeader.transform.Find("Scroll View").gameObject.AddComponent<CategoryScrollRect>();

            categoryScrollRect.inertia = false;

            categoryScrollRect.content = headers.GetComponent<RectTransform>();
            categoryScrollRect.content.pivot = new Vector2(0, 0.5f);

            categoryScrollRect.horizontal = true;
            categoryScrollRect.vertical = false;

            categoryScrollRect.movementType = ScrollRect.MovementType.Unrestricted;

            categoryScrollRect.viewport = categoryViewPortHeaderRectTransform;

            categoryScrollRect.horizontalScrollbar = null;

            _categoryHeader.transform.Find("Scroll View").gameObject.AddComponent<HGScrollRectHelper>().requiredTopLayer = layerKey;
            _categoryHeader.transform.Find("Scroll View").gameObject.AddComponent<ScrollRectSkinController>().skinData = skinData;

            ContentSizeFitter sizeFitter = headers.AddComponent<ContentSizeFitter>();

            sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;


            HorizontalLayoutGroup HLG = headers.GetComponent<HorizontalLayoutGroup>();

            HLG.enabled = true;

            HLG.padding = new RectOffset(8, 8, 4, 4);
            HLG.spacing = 16;
            HLG.childAlignment = TextAnchor.MiddleCenter;
            HLG.childControlWidth = true;
            HLG.childControlHeight = true;
            HLG.childForceExpandWidth = true;
            HLG.childForceExpandHeight = true;




            _optionsPanel = GameObject.Instantiate(Prefabs.MoPanelPrefab, Prefabs.MoCanvas.transform);

            _optionsPanel.GetComponent<RectTransform>().anchorMin = new Vector2(0.275f, 0);
            _optionsPanel.GetComponent<RectTransform>().anchorMax = new Vector2(0.625f, 0.82f);

            _optionsPanel.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").GetComponent<VerticalLayoutGroup>().childForceExpandHeight = false;

            _optionsPanel.SetActive(false);

            _optionsPanel.name = "Options Panel";

            _optionDescriptionPanel = GameObject.Instantiate(_modDescriptionPanel, Prefabs.MoCanvas.transform);

            _optionDescriptionPanel.GetComponent<RectTransform>().anchorMin = new Vector2(0.65f, 0);
            _optionDescriptionPanel.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0.82f);

            _optionDescriptionPanel.SetActive(false);

            _optionDescriptionPanel.name = "Option Description Panel";


            CreateModListButtons();
        }

        private void AddPanelsToSettings()
        {
            Transform subPanelArea = transform.Find("SafeArea").Find("SubPanelArea");
            Transform headerArea = transform.Find("SafeArea").Find("HeaderContainer").Find("Header (JUICED)");

            GameObject modOptionsPanel = GameObject.Instantiate(Prefabs.MoCanvas, subPanelArea);

            HGHeaderNavigationController navigationController = GetComponent<HGHeaderNavigationController>();

            LanguageAPI.Add(Prefabs.HeaderButtonTextToken, "Mod Options");

            GameObject modOptionsHeaderButton = GameObject.Instantiate(Prefabs.MoHeaderButtonPrefab, headerArea);

            modOptionsHeaderButton.name = "GenericHeaderButton (Mod Options)";
            modOptionsHeaderButton.GetComponentInChildren<LanguageTextMeshController>().token = Prefabs.HeaderButtonTextToken;
            modOptionsHeaderButton.GetComponentInChildren<HGButton>().onClick.RemoveAllListeners();
            modOptionsHeaderButton.GetComponentInChildren<HGButton>().onClick.AddListener(new UnityEngine.Events.UnityAction(
            delegate ()
            {
                navigationController.ChooseHeaderByButton(modOptionsHeaderButton.GetComponentInChildren<HGButton>());
            }));

            List<HGHeaderNavigationController.Header> headers = GetComponent<HGHeaderNavigationController>().headers.ToList();

            modOptionsHeaderButton.GetComponentInChildren<HGTextMeshProUGUI>().SetText("MOD OPTIONS");

            HGHeaderNavigationController.Header header = new HGHeaderNavigationController.Header
            {
                headerButton = modOptionsHeaderButton.GetComponent<HGButton>(),
                headerName = "Mod Options",
                tmpHeaderText = modOptionsHeaderButton.GetComponentInChildren<HGTextMeshProUGUI>(),
                headerRoot = modOptionsPanel
            };

            headers.Add(header);

            GetComponent<HGHeaderNavigationController>().headers = headers.ToArray();

            transform.Find("SafeArea").Find("HeaderContainer").Find("Header (JUICED)").Find("GenericGlyph (Right)").SetAsLastSibling();
        }


        private void CreateModListButtons()
        {
            Transform modListLayout = modListPanel.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout");

            HGHeaderNavigationController navigationController = modListPanel.GetComponent<HGHeaderNavigationController>();

            List<HGHeaderNavigationController.Header> headers = new List<HGHeaderNavigationController.Header>();

            //_leftGlyph.transform.SetParent(modListLayout);

            for (int i = 0; i < ModSettingsManager.OptionContainers.Count; i++)
            {
                var container = ModSettingsManager.OptionContainers[i];

                GameObject newModButton = GameObject.Instantiate(Prefabs.ModButtonPrefab, modListLayout);

                LanguageAPI.Add($"{ModSettingsManager.StartingText}.{container.ModGuid}.{container.ModName}.ModListOption".ToUpper().Replace(" ", "_"), container.Title);

                newModButton.GetComponentInChildren<HGTextMeshProUGUI>().text = container.Title;

                newModButton.GetComponent<RooModListButton>().description = container.GetDescriptionAsString();
                newModButton.GetComponent<RooModListButton>().tmp = _modDescriptionPanel.GetComponentInChildren<HGTextMeshProUGUI>();

                newModButton.GetComponent<RooModListButton>().containerIndex = i;

                newModButton.GetComponent<RooModListButton>().navigationController = navigationController;

                newModButton.GetComponent<RooModListButton>().hoverToken = $"{ModSettingsManager.StartingText}.{container.ModGuid}.{container.ModName}.ModListOption".ToUpper().Replace(" ", "_");

                RectTransform modIconRectTransform = newModButton.transform.Find("ModIcon").gameObject.GetComponent<RectTransform>();

                modIconRectTransform.localPosition = new Vector3(-147f, -0.32f, 0f);
                modIconRectTransform.sizeDelta = Vector2.zero;
                modIconRectTransform.anchoredPosition = Vector2.zero;

                modIconRectTransform.gameObject.AddComponent<FetchIconWhenReady>().modGuid = container.ModGuid;

                newModButton.name = $"ModListButton ({container.ModName})";

                newModButton.SetActive(true);

                HGHeaderNavigationController.Header header = new HGHeaderNavigationController.Header
                {
                    headerButton = newModButton.GetComponent<RooModListButton>(),
                    headerName = $"ModListButton ({container.ModName})",
                    tmpHeaderText = newModButton.GetComponentInChildren<HGTextMeshProUGUI>(),
                    headerRoot = null
                };


                headers.Add(header);
            }

            //_rightGlyph.transform.SetParent(modListLayout);

            //_leftGlyph.SetActive(true);
            //_rightGlyph.SetActive(true);

            navigationController.headers = headers.ToArray();

            navigationController.currentHeaderIndex = -1;
        }

        internal void LoadModOptionsFromContainer(int containerIndex, Transform canvas)
        {
            OptionContainer container = ModSettingsManager.OptionContainers[containerIndex];

            //Debug.Log($"Loading Container: {Container.ModName}");

            var mdp = canvas.Find("Mod Description Panel").gameObject;
            var ch = canvas.Find("Category Headers").gameObject;
            var op = canvas.Find("Options Panel").gameObject;
            var odp = canvas.Find("Option Description Panel").gameObject;

            GameObject categoriesObject = ch.transform.Find("Scroll View").Find("Viewport").Find("Categories (JUICED)").gameObject;

            HGHeaderNavigationController navigationController = categoriesObject.GetComponent<HGHeaderNavigationController>();

            if (ch.activeSelf || op.activeSelf || odp.activeSelf)
            {
                UnloadExistingCategoryButtons(categoriesObject.transform);
            }

            if (mdp.activeSelf || !ch.activeSelf || !op.activeSelf || !odp.activeSelf)
            {
                mdp.GetComponentInChildren<HGTextMeshProUGUI>().SetText("");

                mdp.SetActive(false);

                ch.SetActive(true);
                op.SetActive(true);
                odp.SetActive(true);
            }

            List<HGHeaderNavigationController.Header> headers = new List<HGHeaderNavigationController.Header>();

            navigationController.currentHeaderIndex = 0;

            var categoryScrollRect = ch.GetComponentInChildren<CategoryScrollRect>();

            categoryScrollRect.Categories = container.GetCategoriesCached().Count;

            var previousPageButton = GameObject.Instantiate(_leftButton, ch.transform.Find("Scroll View"));
            previousPageButton.SetActive(true);

            for (int i = 0; i < container.GetCategoriesCached().Count; i++)
            {
                GameObject newCategoryButton = GameObject.Instantiate(Prefabs.MoHeaderButtonPrefab, categoriesObject.transform);

                LayoutElement le = newCategoryButton.AddComponent<LayoutElement>();

                le.preferredWidth = 200;

                newCategoryButton.GetComponentInChildren<LanguageTextMeshController>().token = container.GetCategoriesCached()[i].NameToken;

                newCategoryButton.GetComponentInChildren<HGTextMeshProUGUI>().SetText(container.GetCategoriesCached()[i].Name);

                newCategoryButton.GetComponentInChildren<HGButton>().onClick.RemoveAllListeners();

                var categoryIndex = i;

                newCategoryButton.GetComponentInChildren<HGButton>().onClick.AddListener(new UnityEngine.Events.UnityAction(
                delegate ()
                {
                    navigationController.ChooseHeaderByButton(newCategoryButton.GetComponentInChildren<HGButton>());

                    LoadOptionListFromCategory(containerIndex, categoryIndex, container.GetCategoriesCached().Count, canvas);
                }));

                newCategoryButton.name = $"Category Button, {container.GetCategoriesCached()[i].Name}";

                newCategoryButton.SetActive(true);


                HGHeaderNavigationController.Header header = new HGHeaderNavigationController.Header
                {
                    headerButton = newCategoryButton.GetComponent<HGButton>(),
                    headerName = $"Category Button, {container.GetCategoriesCached()[i].Name}",
                    tmpHeaderText = newCategoryButton.GetComponentInChildren<HGTextMeshProUGUI>(),
                    headerRoot = null
                };

                headers.Add(header);
            }
            navigationController.headers = headers.ToArray();

            navigationController.InvokeMethod("RebuildHeaders");

            var nextPageButton = GameObject.Instantiate(_rightButton, ch.transform.Find("Scroll View"));

            //nextPageButton.transform.SetAsLastSibling();
            nextPageButton.SetActive(true);

            categoryScrollRect.Init();

            LoadOptionListFromCategory(containerIndex, navigationController.currentHeaderIndex, navigationController.headers.Length, canvas);
        }

        internal void LoadOptionListFromCategory(int containerIndex, int categoryIndex, int categories, Transform canvas)
        {
            UnloadExistingOptionButtons(canvas.Find("Options Panel"));

            canvas.Find("Category Headers").Find("Scroll View").Find("Scrollbar Horizontal").gameObject.GetComponent<CustomScrollbar>().value = (1f / ((float)categories) - 1) * ((float)categoryIndex);

            canvas.Find("Option Description Panel").GetComponentInChildren<HGTextMeshProUGUI>().SetText("");

            OptionCategory category = ModSettingsManager.OptionContainers[containerIndex].GetCategoriesCached()[categoryIndex];

            var verticalLayoutTransform = canvas.Find("Options Panel").Find("Scroll View").Find("Viewport").Find("VerticalLayout");

            Selectable lastSelectable = null;

            for (int i = 0; i < category.GetModOptionsCached().Count; i++)
            {
                var option = category.GetModOptionsCached()[i];

                GameObject button = option switch
                {
                    CheckBoxOption checkBoxOption => option.CreateOptionGameObject(option, _checkBoxPrefab, verticalLayoutTransform),
                    StepSliderOption stepSliderOption => option.CreateOptionGameObject(option, _sliderPrefab, verticalLayoutTransform),
                    SliderOption sliderOption => option.CreateOptionGameObject(option, _sliderPrefab, verticalLayoutTransform),
                    KeyBindOption keyBindOption => option.CreateOptionGameObject(option, _keyBindPrefab, verticalLayoutTransform),
                    DropDownOption dropDownOption => option.CreateOptionGameObject(option, _dropDownPrefab, verticalLayoutTransform),
                    InputFieldOption inputFieldOption => option.CreateOptionGameObject(option, _inputFieldPrefab, verticalLayoutTransform),
                    _ => throw new ArgumentOutOfRangeException(option.Name)
                };

                button.AddComponent<OptionIdentity>().optionToken = option.OptionToken;

                if (option.OptionOverride != null)
                {
                    OverrideController overrideController = button.AddComponent<OverrideController>();

                    overrideController.modGuid = option.ModGuid;

                    overrideController.overridingName = option.OptionOverride.Name;
                    overrideController.overridingCategoryName = option.OptionOverride.CategoryName;

                    overrideController.CheckForOverride(true);
                }

                CanvasGroup canvasGroup = button.AddComponent<CanvasGroup>();

                var buttonComponent = button.GetComponentInChildren<HGButton>();

                if (buttonComponent)
                {
                    button.GetComponentInChildren<HGButton>().hoverToken = option.OptionToken;

                    button.GetComponentInChildren<HGButton>().onSelect.AddListener(delegate
                    {
                        canvas.Find("Option Description Panel").GetComponentInChildren<HGTextMeshProUGUI>().SetText(option.GetDescriptionAsString());
                    });
                }

                var selectable = button.GetComponentInChildren<Selectable>();

                if (selectable)
                {
                    var selectableNavigation = selectable.navigation;
                    selectableNavigation.mode = Navigation.Mode.Explicit;
                    
                    if (i == 0 || i > category.GetModOptionsCached().Count)
                    {
                        // Todo if at top of list, select category.
                    }
                    else if (lastSelectable)
                    {
                        var lastSelectableNavigation = lastSelectable.navigation;
                        lastSelectableNavigation.selectOnDown = selectable;
                        lastSelectable.navigation = lastSelectableNavigation;
                        
                        selectableNavigation.selectOnUp = lastSelectable;
                    }
                    
                    selectable.navigation = selectableNavigation;
                    lastSelectable = selectable;
                }
                else
                {
                    lastSelectable = null;
                }

                if (!option.Visibility)
                {
                    canvasGroup.alpha = 1;
                    canvasGroup.blocksRaycasts = false;
                    button.GetComponent<LayoutElement>().ignoreLayout = true;
                    continue;
                }

                button.SetActive(true);
            }
        }

        internal void CheckIfRestartNeeded()
        {
            if (BaseSettingsControlOverride._restartOptions.Count > 0)
            {
                if (_warningShown)
                    return;

                ShowRestartWarning();
                _warningShown = true;
            }
            else
            {
                HideRestartWarning();
                _warningShown = false;
            }
        }

        public void ShowRestartWarning()
        {
            RectTransform modListScrollViewRT = transform.Find("SafeArea").Find("SubPanelArea").Find("SettingsSubPanel, Mod Options(Clone)").Find("Mod List Panel").Find("Scroll View").GetComponent<RectTransform>();
            RectTransform warningPanelRT = transform.Find("SafeArea").Find("SubPanelArea").Find("SettingsSubPanel, Mod Options(Clone)").Find("Mod List Panel").Find("Warning Panel").GetComponent<RectTransform>();

            warningPanelRT.gameObject.SetActive(true);

            warningPanelRT.GetComponentInChildren<HGTextMeshProUGUI>().SetText($"Restart Required!");

            if (_animateRoutine != null)
                StopCoroutine(_animateRoutine);

            HGTextMeshProUGUI warningText = warningPanelRT.GetComponentInChildren<HGTextMeshProUGUI>();

            Color showColor = new Color(warningText.color.r, warningText.color.b, warningText.color.g);

            _animateRoutine = AnimateWarningPanel(modListScrollViewRT, new Vector2(0f, 0.074f), warningPanelRT, new Vector2(1f, 0.08f), showColor, degreesPerSecond, 720f);

            StartCoroutine(_animateRoutine);

        }

        public void HideRestartWarning()
        {
            RectTransform modListScrollViewRT = transform.Find("SafeArea").Find("SubPanelArea").Find("SettingsSubPanel, Mod Options(Clone)").Find("Mod List Panel").Find("Scroll View").GetComponent<RectTransform>();
            RectTransform warningPanelRT = transform.Find("SafeArea").Find("SubPanelArea").Find("SettingsSubPanel, Mod Options(Clone)").Find("Mod List Panel").Find("Warning Panel").GetComponent<RectTransform>();

            if (_animateRoutine != null)
                StopCoroutine(_animateRoutine);

            HGTextMeshProUGUI warningText = warningPanelRT.GetComponentInChildren<HGTextMeshProUGUI>();

            Color hideColor = new Color(warningText.color.r, warningText.color.b, warningText.color.g, 0f);

            _animateRoutine = AnimateWarningPanel(modListScrollViewRT, new Vector2(0f, 0f), warningPanelRT, new Vector2(1f, 0f), hideColor, degreesPerSecond, -360f);

            StartCoroutine(_animateRoutine);
        }

        private IEnumerator AnimateWarningPanel(RectTransform modListTransform, Vector2 newModListPos, RectTransform warningTransform, Vector2 newWarningPos, Color textColor, float angleIncrement, float maxAngleRotation)
        {
            bool animating = true;

            float animSpeed = 2.25f;

            HGTextMeshProUGUI warningText = warningTransform.GetComponentInChildren<HGTextMeshProUGUI>();

            Image restartIcon = warningText.transform.parent.Find("RestartIcon").GetComponent<Image>();

            RectTransform restartRectTransform = restartIcon.GetComponent<RectTransform>();

            float max = Mathf.Abs(maxAngleRotation);

            while (animating)
            {
                //modListTransform.anchorMin = Vector2.Lerp(modListTransform.anchorMin, newModListPos, animSpeed * Time.deltaTime);

                //warningTransform.anchorMax = Vector2.Lerp(warningTransform.anchorMax, newWarningPos, animSpeed * Time.deltaTime);

                modListTransform.anchorMin = ExtensionMethods.SmoothStep(modListTransform.anchorMin, newModListPos, (animSpeed * 5.25f) * Time.deltaTime);

                warningTransform.anchorMax = ExtensionMethods.SmoothStep(warningTransform.anchorMax, newWarningPos, (animSpeed * 5.25f) * Time.deltaTime);

                float angle = Mathf.Clamp(Mathf.Lerp(angleIncrement * Time.deltaTime, max, 1f * Time.deltaTime), 90 * Time.deltaTime, Math.Abs(maxAngleRotation));

                if (angle > 90 * Time.deltaTime)
                {
                    max -= angle;
                }

                restartRectTransform.localRotation *= Quaternion.AngleAxis(maxAngleRotation > 0 ? angle : -angle, Vector3.forward);

                switch (textColor.a)
                {
                    case 1f:
                        warningText.color = Color.Lerp(warningText.color, textColor, (animSpeed * 2) * Time.deltaTime);
                        restartIcon.color = Color.Lerp(restartIcon.color, textColor, (animSpeed * 2) * Time.deltaTime);
                        break;
                    case 0f:
                        warningText.color = Color.Lerp(warningText.color, textColor, (animSpeed * 4) * Time.deltaTime);
                        restartIcon.color = Color.Lerp(restartIcon.color, textColor, (animSpeed * 4) * Time.deltaTime);
                        break;
                }

                if (ExtensionMethods.CloseEnough(modListTransform.anchorMin, newModListPos) && ExtensionMethods.CloseEnough(warningTransform.anchorMax, newWarningPos) && ExtensionMethods.CloseEnough(warningText.color, textColor) && ExtensionMethods.CloseEnough(restartIcon.color, textColor))
                {
                    modListTransform.anchorMin = newModListPos;
                    warningTransform.anchorMax = newWarningPos;

                    warningText.color = textColor;
                    restartIcon.color = textColor;
                }

                yield return new WaitForEndOfFrame();
            }
        }

        public void UpdateExistingOptionButtons()
        {
            if (_controllers == null || _controllers.Length == 0)
                _controllers = GetComponentsInChildren<OverrideController>();

            foreach (var controller in _controllers)
            {
                controller.CheckForOverride();
            }
        }

        public void UpdateVisibility(string optionToken, bool visible)
        {
            foreach (var optionIdentity in GetComponentsInChildren<OptionIdentity>())
            {
                if (optionIdentity.optionToken != optionToken)
                    continue;

                var canvasGroup = optionIdentity.GetComponent<CanvasGroup>();

                canvasGroup.alpha = visible ? 1 : 0;
                canvasGroup.blocksRaycasts = visible;

                optionIdentity.GetComponent<LayoutElement>().ignoreLayout = !visible;
                break;
            }
        }

        internal void UnLoad(Transform canvas)
        {
            if (!initialized)
                return;

            var mdp = canvas.Find("Mod Description Panel").gameObject;
            var ch = canvas.Find("Category Headers").gameObject;
            var op = canvas.Find("Options Panel").gameObject;
            var odp = canvas.Find("Option Description Panel").gameObject;

            if (!mdp.activeSelf)
            {
                mdp.SetActive(true);

                ch.SetActive(false);
                op.SetActive(false);
                odp.SetActive(false);
            }


            modListHighlight.transform.SetParent(transform);
            modListHighlight.SetActive(false);

            UnloadExistingOptionButtons(op.transform);
            UnloadExistingCategoryButtons(ch.transform);
        }


        private void UnloadExistingCategoryButtons(Transform ch)
        {
            _categoryHeaderHighlight.transform.SetParent(transform);
            _categoryHeaderHighlight.SetActive(false);

            HGButton[] activeCategoryButtons = ch.GetComponentsInChildren<HGButton>();

            if (activeCategoryButtons.Length <= 0)
                return;

            foreach (var activeCategoryButton in activeCategoryButtons)
            {
                if (activeCategoryButton.gameObject == null)
                    continue;

                //Debug.Log($"Unloading {activeCategoryButtons[i].gameObject}");

                GameObject buttonGameObject = activeCategoryButton.gameObject;

                buttonGameObject.SetActive(false);

                GameObject.DestroyImmediate(buttonGameObject);
            }

            //_leftGlyph.SetActive(false);
            //_rightGlyph.SetActive(false);
        }

        private void UnloadExistingOptionButtons(Transform op)
        {
            _controllers = Array.Empty<OverrideController>();

            OptionIdentity[] activeOptionButtons = op.GetComponentsInChildren<OptionIdentity>();

            if (activeOptionButtons.Length <= 0)
                return;

            foreach (var button in activeOptionButtons)
            {
                if (button.gameObject == null)
                    continue;

                GameObject buttonGameObject = button.gameObject;

                buttonGameObject.SetActive(false);

                GameObject.DestroyImmediate(buttonGameObject);
            }
        }

        public void OnDisable()
        {
            //Debug.Log("Unloading Assets");

            initialized = false;

            // foreach (var gameObject in _toDestroy)
            // {
            //     GameObject.DestroyImmediate(gameObject);
            // }

            GameObject.DestroyImmediate(_checkBoxPrefab);
            GameObject.DestroyImmediate(_sliderPrefab);
            GameObject.DestroyImmediate(_keyBindPrefab);
            GameObject.DestroyImmediate(modListPanel);
            GameObject.DestroyImmediate(_modDescriptionPanel);
            GameObject.DestroyImmediate(_categoryHeader);
            GameObject.DestroyImmediate(_optionsPanel);
            GameObject.DestroyImmediate(_optionDescriptionPanel);
            GameObject.DestroyImmediate(_categoryHeaderHighlight);
            GameObject.DestroyImmediate(modListHighlight);
            GameObject.DestroyImmediate(_leftButton);
            GameObject.DestroyImmediate(_rightButton);
            GameObject.DestroyImmediate(_dropDownPrefab);
            GameObject.DestroyImmediate(_inputFieldPrefab);
            GameObject.DestroyImmediate(Prefabs.Gdp);
            GameObject.DestroyImmediate(Prefabs.MoCanvas);
            GameObject.DestroyImmediate(Prefabs.MoHeaderButtonPrefab);
            GameObject.DestroyImmediate(Prefabs.MoPanelPrefab);
            GameObject.DestroyImmediate(Prefabs.ModButtonPrefab);

            OptionSerializer.Save(ModSettingsManager.OptionContainers.ToArray());
        }
        private void RegisterForDestroy(ref GameObject gameObject)
        {
            _toDestroy.Add(gameObject);
        }

        public void OnEnable()
        {
            initialized = true;
        }
    }
}
