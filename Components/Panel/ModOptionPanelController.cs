using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using R2API;
using R2API.Utils;
using RiskOfOptions.Components.OptionComponents;
using RiskOfOptions.Components.Options;
using RiskOfOptions.Components.RuntimePrefabs;
using RiskOfOptions.Containers;
using RiskOfOptions.Options;
using RoR2;
using RoR2.UI;
using RoR2.UI.SkinControllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 618

namespace RiskOfOptions.Components.Panel
{
    public class ModOptionPanelController : MonoBehaviour
    {
        public bool initialized = false;

        public GameObject modListHighlight;

        private GameObject _checkBoxPrefab;
        private GameObject _sliderPrefab;
        private GameObject _stepSliderPrefab;
        private GameObject _keyBindPrefab;
        private GameObject _dropDownPrefab;
        private GameObject _inputFieldPrefab;

        private GameObject _leftButton;
        private GameObject _rightButton;

        private MPButton _revertButton;

        //private GameObject _leftGlyph;
        //private GameObject _rightGlyph;
        
        public Color warningColor = Color.red;

        public float degreesPerSecond = 2f;

        private bool _warningShown = false;

        private IEnumerator _animateRoutine;

        private ModOptionsPanelPrefab _panel;
        private ModSetting[] _modSettings = Array.Empty<ModSetting>();


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
            RuntimePrefabManager.InitializePrefabs(gameObject);

            _panel = RuntimePrefabManager.Get<ModOptionsPanelPrefab>();
            
            _revertButton = transform.Find("SafeArea").Find("FooterContainer").Find("FooterPanel, M&KB").Find("RevertAndBack (JUICED)").Find("NakedButton (Revert)").GetComponent<HGButton>();
            _revertButton.onClick.AddListener(RevertChanges);
            
            Transform subPanelArea = transform.Find("SafeArea").Find("SubPanelArea");
            //Transform headerArea = transform.Find("SafeArea").Find("HeaderContainer").Find("Header (JUICED)");

            //_leftGlyph = GameObject.Instantiate(headerArea.Find("GenericGlyph (Left)").gameObject);
            //_rightGlyph = GameObject.Instantiate(headerArea.Find("GenericGlyph (Right)").gameObject);

            //_leftGlyph.SetActive(false);
            //_rightGlyph.SetActive(false);

            //_leftGlyph.GetComponentInChildren<HGTextMeshProUGUI>().SetText("<sprite=\"tmpsprXboxOneGlyphs\" name=\"texXBoxOneGlyphs_5\">");
            //_rightGlyph.GetComponentInChildren<HGTextMeshProUGUI>().SetText("<sprite=\"tmpsprXboxOneGlyphs\" name=\"texXBoxOneGlyphs_9\">");

            _checkBoxPrefab = RuntimePrefabManager.Get<CheckBoxPrefab>().CheckBoxButton;
            _sliderPrefab = RuntimePrefabManager.Get<SliderPrefab>().Slider;
            _stepSliderPrefab = RuntimePrefabManager.Get<StepSliderPrefab>().StepSlider;
            _keyBindPrefab = RuntimePrefabManager.Get<KeyBindPrefab>().KeyBind;
            
            _dropDownPrefab = GameObject.Instantiate(subPanelArea.Find("SettingsSubPanel, Video").Find("Scroll View").Find("Viewport").Find("VerticalLayout").Find("Option, Resolution").gameObject);
            _inputFieldPrefab = GameObject.Instantiate(_checkBoxPrefab);

            _dropDownPrefab.SetActive(false);

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

            //GameObject.Instantiate(_modOptionsPanelPrefab.OptionsPanel.transform.Find("Scroll View").Find("BlurPanel").gameObject, textCanvas.transform);
            //GameObject.Instantiate(_modOptionsPanelPrefab.OptionsPanel.transform.Find("Scroll View").Find("ImagePanel").gameObject, textCanvas.transform);

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

            // if (!RooDropdown.CheckBoxPrefab)
            // {
            //     RooDropdown.CheckBoxPrefab = _checkBoxPrefab;
            // }
            //
            // if (!RooDropdown.PanelPrefab)
            // {
            //     RooDropdown.PanelPrefab = _modOptionsPanelPrefab.OptionsPanel;
            // }

            _leftButton = GameObject.Instantiate(_panel.EmptyButton);
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
            _checkBoxPrefab.SetActive(false);
            _sliderPrefab.SetActive(false);
            _stepSliderPrefab.SetActive(false);
            _keyBindPrefab.SetActive(false);
        }

        private void CreatePanel()
        {
            CreateModListButtons(); // TODO Move this
        }

        private void AddPanelsToSettings()
        {
            Transform headerArea = transform.Find("SafeArea").Find("HeaderContainer").Find("Header (JUICED)");
            
            HGHeaderNavigationController navigationController = GetComponent<HGHeaderNavigationController>();

            GameObject modOptionsHeaderButton = Instantiate(_panel.ModOptionsHeaderButton, headerArea);

            modOptionsHeaderButton.name = "GenericHeaderButton (Mod Options)";
            modOptionsHeaderButton.GetComponentInChildren<LanguageTextMeshController>().token = SettingsModifier.HeaderToken;
            modOptionsHeaderButton.GetComponentInChildren<HGButton>().onClick.RemoveAllListeners();
            modOptionsHeaderButton.GetComponentInChildren<HGButton>().onClick.AddListener(delegate ()
            {
                navigationController.ChooseHeaderByButton(modOptionsHeaderButton.GetComponentInChildren<HGButton>());
            });

            List<HGHeaderNavigationController.Header> headers = GetComponent<HGHeaderNavigationController>().headers.ToList();

            modOptionsHeaderButton.GetComponentInChildren<HGTextMeshProUGUI>().SetText("MOD OPTIONS");

            HGHeaderNavigationController.Header header = new HGHeaderNavigationController.Header
            {
                headerButton = modOptionsHeaderButton.GetComponent<HGButton>(),
                headerName = "Mod Options",
                tmpHeaderText = modOptionsHeaderButton.GetComponentInChildren<HGTextMeshProUGUI>(),
                headerRoot = _panel.Canvas
            };

            headers.Add(header);

            GetComponent<HGHeaderNavigationController>().headers = headers.ToArray();

            transform.Find("SafeArea").Find("HeaderContainer").Find("Header (JUICED)").Find("GenericGlyph (Right)").SetAsLastSibling();
        }


        private void CreateModListButtons()
        {
            HGHeaderNavigationController navigationController = _panel.ModListPanel.GetComponent<HGHeaderNavigationController>();

            List<HGHeaderNavigationController.Header> headers = new List<HGHeaderNavigationController.Header>();

            //_leftGlyph.transform.SetParent(modListLayout);

            Transform modListVerticalLayout = _panel.ModListPanel.transform.Find("Scroll View").Find("Viewport")
                .Find("VerticalLayout");

            foreach (var collection in ModSettingsManager.OptionCollection)
            {
                GameObject newModButton = Instantiate(_panel.ModListButton, modListVerticalLayout);

                //LanguageAPI.Add($"{ModSettingsManager.StartingText}.{collection.ModName}.ModListOption".ToUpper().Replace(" ", "_"), collection.title);
                
                ModListButton modListButton = newModButton.GetComponent<ModListButton>();
                
                modListButton.description = collection.description;
                modListButton.nameLabel = modListButton.GetComponent<LanguageTextMeshController>();
                modListButton.token = collection.NameToken;
                modListButton.modGuid = collection.ModGuid;
                modListButton.navigationController = navigationController;
                modListButton.descriptionText = _panel.ModDescriptionPanel.GetComponentInChildren<HGTextMeshProUGUI>();

                RectTransform modIconRectTransform = newModButton.transform.Find("ModIcon").gameObject.GetComponent<RectTransform>();

                modIconRectTransform.localPosition = new Vector3(-147f, -0.32f, 0f);
                modIconRectTransform.sizeDelta = Vector2.zero;
                modIconRectTransform.anchoredPosition = Vector2.zero;
                modIconRectTransform.gameObject.AddComponent<FetchIconWhenReady>().modGuid = collection.ModGuid;

                newModButton.name = $"ModListButton ({collection.ModName})";
                newModButton.SetActive(true);

                HGHeaderNavigationController.Header header = new HGHeaderNavigationController.Header
                {
                    headerButton = newModButton.GetComponent<ModListButton>(),
                    headerName = $"ModListButton ({collection.ModName})",
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

        internal void LoadModOptionsFromOptionCollection(string modGuid)
        {
            OptionCollection collection = ModSettingsManager.OptionCollection[modGuid];
            
            GameObject categoriesObject = _panel.CategoryHeader.transform.Find("Scroll View").Find("Viewport").Find("Categories (JUICED)").gameObject;

            HGHeaderNavigationController navigationController = categoriesObject.GetComponent<HGHeaderNavigationController>();
            
            bool existingCategory = _panel.CategoryHeader.GetComponentsInChildren<HGButton>().Length > 0;
            if (_panel.CategoryHeader.activeSelf || _panel.ModOptionsPanel.activeSelf || _panel.ModOptionsDescriptionPanel.activeSelf || existingCategory)
                UnloadExistingCategoryButtons();

            if (_panel.ModDescriptionPanel.activeSelf || !_panel.CategoryHeader.activeSelf || !_panel.ModOptionsPanel.activeSelf || !_panel.ModOptionsDescriptionPanel.activeSelf)
            {
                _panel.ModDescriptionPanel.GetComponentInChildren<HGTextMeshProUGUI>().SetText("");

                _panel.ModDescriptionPanel.SetActive(false);
                _panel.CategoryHeader.SetActive(true);
                _panel.ModOptionsPanel.SetActive(true);
                _panel.ModOptionsDescriptionPanel.SetActive(true);
            }

            List<HGHeaderNavigationController.Header> headers = new List<HGHeaderNavigationController.Header>();
            navigationController.currentHeaderIndex = 0;

            var categoryScrollRect = _panel.CategoryHeader.GetComponentInChildren<CategoryScrollRect>();
            categoryScrollRect.Categories = collection.CategoryCount;

            var previousPageButton = Instantiate(_leftButton, _panel.CategoryHeader.transform.Find("Scroll View"));
            previousPageButton.SetActive(true);

            for (int i = 0; i < collection.CategoryCount; i++)
            {
                GameObject newCategoryButton = Instantiate(_panel.ModOptionsHeaderButton, categoriesObject.transform);

                LayoutElement le = newCategoryButton.AddComponent<LayoutElement>();

                le.preferredWidth = 200;

                newCategoryButton.GetComponentInChildren<LanguageTextMeshController>().token = collection[i].NameToken;
                newCategoryButton.GetComponentInChildren<HGTextMeshProUGUI>().SetText(collection[i].name);
                newCategoryButton.GetComponentInChildren<HGButton>().onClick.RemoveAllListeners();

                var categoryIndex = i;

                newCategoryButton.GetComponentInChildren<HGButton>().onClick.AddListener(delegate
                {
                    navigationController.ChooseHeaderByButton(newCategoryButton.GetComponentInChildren<HGButton>());

                    LoadOptionListFromCategory(modGuid, categoryIndex, headers.Count);
                });

                newCategoryButton.name = $"Category Button, {collection[i].name}";
                newCategoryButton.SetActive(true);

                HGHeaderNavigationController.Header header = new HGHeaderNavigationController.Header
                {
                    headerButton = newCategoryButton.GetComponent<HGButton>(),
                    headerName = $"Category Button, {collection[i].name}",
                    tmpHeaderText = newCategoryButton.GetComponentInChildren<HGTextMeshProUGUI>(),
                    headerRoot = null
                };

                headers.Add(header);
            }
            navigationController.headers = headers.ToArray();

            navigationController.InvokeMethod("RebuildHeaders");

            var nextPageButton = Instantiate(_rightButton, _panel.CategoryHeader.transform.Find("Scroll View"));

            //nextPageButton.transform.SetAsLastSibling();
            nextPageButton.SetActive(true);

            categoryScrollRect.Init();

            LoadOptionListFromCategory(collection.ModGuid, navigationController.currentHeaderIndex, navigationController.headers.Length);
        }

        internal void LoadOptionListFromCategory(string modGuid, int categoryIndex, int headerCount)
        {
            UnloadExistingOptionButtons();

            _panel.CategoryHeader.transform.Find("Scroll View").Find("Scrollbar Horizontal").gameObject.GetComponent<CustomScrollbar>().value = (1f / ((float)headerCount) - 1) * ((float)categoryIndex);

            _panel.ModOptionsDescriptionPanel.GetComponentInChildren<HGTextMeshProUGUI>().SetText("");

            Category category = ModSettingsManager.OptionCollection[modGuid][categoryIndex];

            var verticalLayoutTransform = _panel.ModOptionsPanel.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout");

            Selectable lastSelectable = null;

            _modSettings = new ModSetting[category.OptionCount];

            for (int i = 0; i < category.OptionCount; i++)
            {
                var option = category[i];

                GameObject button = option switch
                {
                    CheckBoxOption checkBoxOption => option.CreateOptionGameObject(_checkBoxPrefab, verticalLayoutTransform),
                    SliderOption sliderOption => option.CreateOptionGameObject(_sliderPrefab, verticalLayoutTransform),
                    StepSliderOption stepSliderOption => option.CreateOptionGameObject(_stepSliderPrefab, verticalLayoutTransform),
                    KeyBindOption keyBindOption => option.CreateOptionGameObject(_keyBindPrefab, verticalLayoutTransform),
                    // DropDownOption dropDownOption => option.CreateOptionGameObject(option, _dropDownPrefab, verticalLayoutTransform),
                    // InputFieldOption inputFieldOption => option.CreateOptionGameObject(option, _inputFieldPrefab, verticalLayoutTransform),
                    _ => throw new ArgumentOutOfRangeException(option.Name)
                };

                _modSettings[i] = button.GetComponentInChildren<ModSetting>();
                _modSettings[i].optionController = this;

                // if (option.OptionOverride != null)
                // {
                //     OverrideController overrideController = button.AddComponent<OverrideController>();
                //
                //     overrideController.modGuid = option.ModGuid;
                //
                //     overrideController.overridingName = option.OptionOverride.Name;
                //     overrideController.overridingCategoryName = option.OptionOverride.CategoryName;
                //
                //     overrideController.CheckForOverride(true);
                // }

                CanvasGroup canvasGroup = button.AddComponent<CanvasGroup>();

                var buttonComponent = button.GetComponentInChildren<HGButton>();
                if (buttonComponent)
                {
                    button.GetComponentInChildren<HGButton>().hoverToken = option.GetNameToken();

                    button.GetComponentInChildren<HGButton>().onSelect.AddListener(delegate
                    {
                        _panel.ModOptionsDescriptionPanel.GetComponentInChildren<HGTextMeshProUGUI>().SetText(option.Description);
                    });
                }

                var selectable = button.GetComponentInChildren<Selectable>();

                if (selectable)
                {
                    var selectableNavigation = selectable.navigation;
                    selectableNavigation.mode = Navigation.Mode.Explicit;
                    
                    if (i == 0 || i > category.OptionCount)
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

                // if (!option.Visibility)
                // {
                //     canvasGroup.alpha = 1;
                //     canvasGroup.blocksRaycasts = false;
                //     button.GetComponent<LayoutElement>().ignoreLayout = true;
                //     continue;
                // }

                button.SetActive(true);
            }
        }

        internal void CheckIfRestartNeeded()
        {
            // if (BaseSettingsControlOverride._restartOptions.Count > 0)
            // {
            //     if (_warningShown)
            //         return;
            //
            //     ShowRestartWarning();
            //     _warningShown = true;
            // }
            // else
            // {
            //     HideRestartWarning();
            //     _warningShown = false;
            // }
        }

        public void ShowRestartWarning()
        {
            RectTransform modListScrollViewRT = _panel.WarningPanel.GetComponent<RectTransform>();
            RectTransform warningPanelRT = _panel.WarningPanel.GetComponent<RectTransform>();

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
            RectTransform modListScrollViewRT = _panel.WarningPanel.GetComponent<RectTransform>();
            RectTransform warningPanelRT = _panel.WarningPanel.GetComponent<RectTransform>();

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

        public void UpdateVisibility(string optionToken, bool visible)
        {
            foreach (var modSetting in GetComponentsInChildren<ModSetting>())
            {
                if (modSetting.settingToken != optionToken)
                    continue;

                var canvasGroup = modSetting.GetComponent<CanvasGroup>();

                canvasGroup.alpha = visible ? 1 : 0;
                canvasGroup.blocksRaycasts = visible;

                modSetting.GetComponent<LayoutElement>().ignoreLayout = !visible;
                break;
            }
        }

        internal void UnLoad()
        {
            if (!initialized)
                return;

            if (!_panel.ModDescriptionPanel.activeSelf)
            {
                _panel.ModDescriptionPanel.SetActive(true);

                _panel.CategoryHeader.SetActive(false);
                _panel.ModOptionsPanel.SetActive(false);
                _panel.ModOptionsDescriptionPanel.SetActive(false);
            }

            modListHighlight.transform.SetParent(transform);
            modListHighlight.SetActive(false);

            UnloadExistingOptionButtons();
            UnloadExistingCategoryButtons();
        }


        private void UnloadExistingCategoryButtons()
        {
            _panel.CategoryHeaderHighlight.transform.SetParent(transform);
            _panel.CategoryHeaderHighlight.SetActive(false);

            HGButton[] activeCategoryButtons = _panel.CategoryHeader.GetComponentsInChildren<HGButton>();

            if (activeCategoryButtons.Length <= 0)
                return;

            foreach (var activeCategoryButton in activeCategoryButtons)
            {
                if (activeCategoryButton.gameObject == null)
                    continue;
                
                GameObject buttonGameObject = activeCategoryButton.gameObject;
                buttonGameObject.SetActive(false);

                DestroyImmediate(buttonGameObject);
            }

            //_leftGlyph.SetActive(false);
            //_rightGlyph.SetActive(false);
        }

        private void UnloadExistingOptionButtons()
        {
            if (_modSettings.Length <= 0)
                return;

            foreach (var button in _modSettings)
            {
                if (button.gameObject == null)
                    continue;

                GameObject buttonGameObject = button.gameObject;
                buttonGameObject.SetActive(false);

                DestroyImmediate(buttonGameObject);
            }
        }

        public void OnDisable()
        {
            //Debug.Log("Unloading Assets");

            initialized = false;
            
            GameObject.DestroyImmediate(_keyBindPrefab);
            GameObject.DestroyImmediate(modListHighlight);
            GameObject.DestroyImmediate(_leftButton);
            GameObject.DestroyImmediate(_rightButton);
            GameObject.DestroyImmediate(_dropDownPrefab);
            GameObject.DestroyImmediate(_inputFieldPrefab);
            
            RuntimePrefabManager.DestroyPrefabs();

            //OptionSerializer.Save(ModSettingsManager.OptionContainers.ToArray());
        }

        public void OnEnable()
        {
            initialized = true;
        }

        public void Update()
        {
            bool interactable = false;
            foreach (var modSetting in _modSettings)
            {
                if (modSetting.HasChanged())
                    interactable = true;
            }

            _revertButton.interactable = interactable;
        }

        public void RevertChanges()
        {
            foreach (var modSetting in _modSettings)
                modSetting.Revert();
        }

        public void OptionChanged()
        {
            foreach (var modSetting in _modSettings)
                modSetting.CheckIfDisabled();
        }
    }
}
