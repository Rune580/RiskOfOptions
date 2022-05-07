using JetBrains.Annotations;
using LeTai.Asset.TranslucentImage;
using RiskOfOptions.Components.Panel;
using RiskOfOptions.Resources;
using RoR2;
using RoR2.UI;
using RoR2.UI.SkinControllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RiskOfOptions.Components.RuntimePrefabs
{
    public class ModOptionsPanelPrefab : IRuntimePrefab
    {
        [CanBeNull] private GameObject _optionsPanel;
        [CanBeNull] private GameObject _genericDescriptionPanel; 
        private GameObject _audioVerticalLayout;
        [CanBeNull] private GameObject _emptyButton;
        
        public GameObject ModListButton { get; private set; }
        public GameObject ModOptionsHeaderButton { get; private set; }
        public GameObject Canvas { get; private set; }
        public GameObject ModListPanel { get; private set;}
        public GameObject ModListHighlight { get; private set; }
        public GameObject ModDescriptionPanel { get; private set; }
        public GameObject CategoryHeader { get; private set; }
        public GameObject CategoryHeaderButton { get; private set; }
        public GameObject CategoryHeaderHighlight { get; private set; }
        public GameObject CategoryPageIndicators { get; private set; }
        public GameObject CategoryPageIndicator { get; private set; }
        public GameObject CategoryPageIndicatorOutline { get; private set; }
        public GameObject CategoryLeftButton { get; private set; }
        public GameObject CategoryRightButton { get; private set; }
        public GameObject ModOptionsPanel { get; private set; }
        public GameObject ModOptionsDescriptionPanel { get; private set; }
        public GameObject WarningPanel { get; private set; }

        public void Instantiate(GameObject settingsPanel)
        {
            Transform transform = settingsPanel.transform;
            Transform subPanelArea = transform.Find("SafeArea").Find("SubPanelArea");
            Transform headerArea = transform.Find("SafeArea").Find("HeaderContainer").Find("Header (JUICED)");
            
            CreateOptionsPanel(subPanelArea);
            CreateGenericDescriptionPanel(subPanelArea);
            CreateModOptionsHeaderButton(headerArea);
            CreateVerticalLayout(subPanelArea);
            CreateModListButton();
            CreateEmptyButton();

            CreateCanvas();
            CreateModListPanel(settingsPanel);
            CreateModDescriptionPanel();
            CreateCategoryHeader(settingsPanel, headerArea.gameObject);
            CreateAdditionalCategoryStuff();
            CreateModOptionsPanel();
            
            Object.DestroyImmediate(_emptyButton);
            Object.DestroyImmediate(_optionsPanel);
            
            CreateModOptionsDescriptionPanel();
            CreateWarningPanel();
            
            Object.DestroyImmediate(_genericDescriptionPanel);
        }

        public void Destroy()
        {
            Object.DestroyImmediate(ModListButton);
            Object.DestroyImmediate(ModOptionsHeaderButton);
            Object.DestroyImmediate(_emptyButton);
            Object.DestroyImmediate(Canvas);
            Object.DestroyImmediate(ModListPanel);
            Object.DestroyImmediate(ModListHighlight);
            Object.DestroyImmediate(ModDescriptionPanel);
            Object.DestroyImmediate(CategoryPageIndicators);
            Object.DestroyImmediate(CategoryPageIndicator);
            Object.DestroyImmediate(CategoryPageIndicatorOutline);
            Object.DestroyImmediate(CategoryLeftButton);
            Object.DestroyImmediate(CategoryRightButton);
            Object.DestroyImmediate(CategoryHeader);
            Object.DestroyImmediate(CategoryHeaderButton);
            Object.DestroyImmediate(CategoryHeaderHighlight);
            Object.DestroyImmediate(ModOptionsPanel);
            Object.DestroyImmediate(ModOptionsDescriptionPanel);
            Object.DestroyImmediate(WarningPanel);
        }

        private void CreateOptionsPanel(Transform subPanelArea)
        {
            GameObject audioPanel = subPanelArea.Find("SettingsSubPanel, Audio").gameObject;

            _optionsPanel = Object.Instantiate(audioPanel, subPanelArea);
            _optionsPanel!.name = "Empty Panel";
            
            Object.DestroyImmediate(_optionsPanel!.GetComponent<SettingsPanelController>());

            Transform verticalLayout = _optionsPanel!.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout");

            while (verticalLayout.childCount > 0)
                Object.DestroyImmediate(verticalLayout.GetChild(0).gameObject);
        }

        private void CreateGenericDescriptionPanel(Transform subPanelArea)
        {
            _genericDescriptionPanel = Object.Instantiate(subPanelArea.Find("GenericDescriptionPanel").gameObject);
            
            Object.DestroyImmediate(_genericDescriptionPanel!.GetComponentInChildren<DisableIfTextIsEmpty>());
            Object.DestroyImmediate(_genericDescriptionPanel!.GetComponentInChildren<LanguageTextMeshController>());
            Object.DestroyImmediate(_genericDescriptionPanel!.transform.Find("ContentSizeFitter").Find("BlurPanel").gameObject);
            Object.DestroyImmediate(_genericDescriptionPanel!.transform.Find("ContentSizeFitter").Find("CornerRect").gameObject);
        }

        private void CreateModOptionsHeaderButton(Transform headerArea)
        {
            ModOptionsHeaderButton = Object.Instantiate(headerArea.Find("GenericHeaderButton (Audio)").gameObject);
            ModOptionsHeaderButton.name = "GenericHeaderButton (Mod Options)";
            
            ModOptionsHeaderButton.GetComponentInChildren<LanguageTextMeshController>().token = LanguageTokens.HeaderToken;
            ModOptionsHeaderButton.GetComponentInChildren<HGButton>().onClick.RemoveAllListeners();
        }

        private void CreateVerticalLayout(Transform subPanelArea)
        {
            _audioVerticalLayout = subPanelArea.Find("SettingsSubPanel, Audio").Find("Scroll View").Find("Viewport").Find("VerticalLayout").gameObject;
        }

        private void CreateModListButton()
        {
            ModListButton = Object.Instantiate(_audioVerticalLayout.transform.Find("SettingsEntryButton, Bool (Audio Focus)").gameObject);
            ModListButton.name = "Mod Options Prefab, ModList Button";

            Object.DestroyImmediate(ModListButton.GetComponentInChildren<CarouselController>());
            Object.DestroyImmediate(ModListButton.GetComponentInChildren<ButtonSkinController>());
            Object.DestroyImmediate(ModListButton.transform.Find("CarouselRect").gameObject);


            HGButton oldButton = ModListButton.GetComponent<HGButton>();
            bool allowAllEventSystems = oldButton.allowAllEventSystems;
            bool submitOnPointerUp = oldButton.submitOnPointerUp;
            UILayerKey requiredTopLayer = oldButton.requiredTopLayer;
            UnityEngine.Events.UnityEvent onFindSelectableLeft = oldButton.onFindSelectableLeft;
            UnityEngine.Events.UnityEvent onFindSelectableRight = oldButton.onFindSelectableRight;
            UnityEngine.Events.UnityEvent onSelect = oldButton.onSelect;
            UnityEngine.Events.UnityEvent onDeselect = oldButton.onDeselect;
            bool defaultFallbackButton = oldButton.defaultFallbackButton;
            Button.ButtonClickedEvent buttonClickedEvent = oldButton.onClick;
            ColorBlock colors = oldButton.colors;
            bool showImageOnHover = oldButton.showImageOnHover;
            Image imageOnHover = oldButton.imageOnHover;
            Image imageOnInteractable = oldButton.imageOnInteractable;
            bool updateTextOnHover = oldButton.updateTextOnHover;
            LanguageTextMeshController hoverLanguageTextMeshController = oldButton.hoverLanguageTextMeshController;
            string hoverToken = oldButton.hoverToken;
            string uiClickSoundOverride = oldButton.uiClickSoundOverride;
            Object.DestroyImmediate(oldButton);
            
            colors.disabledColor = ModOptionsHeaderButton.GetComponent<HGButton>().colors.disabledColor;
            
            ModListButton newButton = ModListButton.AddComponent<ModListButton>();
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
            
            RectTransform buttonTextRectTransform = ModListButton.transform.Find("ButtonText").GetComponent<RectTransform>();

            buttonTextRectTransform.anchorMin = new Vector2(0.19f, 0);
            buttonTextRectTransform.anchorMax = new Vector2(1, 1);

            GameObject modIconGameObject = new GameObject("ModIcon");

            RectTransform modIconRectTransform = modIconGameObject.AddComponent<RectTransform>();
            modIconGameObject.AddComponent<CanvasRenderer>();
            modIconGameObject.AddComponent<Image>().preserveAspect = true;
            modIconRectTransform.anchorMin = new Vector2(0.04f, 0.13f);
            modIconRectTransform.anchorMax = new Vector2(0.19f, 0.86f);
            modIconRectTransform.pivot = new Vector2(0.5f, 0.5f);
            modIconGameObject.transform.SetParent(ModListButton.transform);
            
            
            GameObject iconOutline = Object.Instantiate(ModListButton.transform.Find("BaseOutline").gameObject, modIconRectTransform);
            RectTransform iconOutlineRectTransform = iconOutline.GetComponent<RectTransform>();
            iconOutlineRectTransform.sizeDelta = Vector2.zero;
            iconOutlineRectTransform.anchoredPosition = Vector2.zero;
            iconOutlineRectTransform.localScale = new Vector3(0.94f, 1.16f, 1);

            ModListButton.SetActive(false);
        }

        private void CreateEmptyButton()
        {
            _emptyButton = Object.Instantiate(_audioVerticalLayout.transform.Find("SettingsEntryButton, Bool (Audio Focus)").gameObject);
            _emptyButton!.name = "Mod Options Prefab, Empty Button";

            Object.DestroyImmediate(_emptyButton!.GetComponentInChildren<CarouselController>());
            Object.DestroyImmediate(_emptyButton!.GetComponentInChildren<ButtonSkinController>());
            Object.DestroyImmediate(_emptyButton!.transform.Find("CarouselRect").gameObject);
        }

        private void CreateCanvas()
        {
            Canvas = Object.Instantiate(_optionsPanel, _optionsPanel!.transform.parent);
            
            Canvas.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);

            Object.DestroyImmediate(Canvas.GetComponent<SettingsPanelController>());
            Object.DestroyImmediate(Canvas.GetComponent<Image>());
            Object.DestroyImmediate(Canvas.GetComponent<HGButtonHistory>());
            Object.DestroyImmediate(Canvas.transform.Find("Scroll View").gameObject);

            Canvas.AddComponent<GenericDescriptionController>();

            Canvas.name = "SettingsSubPanel, (Mod Options)";
        }

        private void CreateModListPanel(GameObject parent)
        {
            ModListPanel = Object.Instantiate(_optionsPanel, Canvas.transform);

            ModListPanel.GetComponent<RectTransform>().anchorMax = new Vector2(0.25f, 1f);
            ModListPanel.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").GetComponent<VerticalLayoutGroup>().spacing = 6;
            ModListPanel.SetActive(false);
            ModListPanel.name = "Mod List Panel";
            ModListPanel.AddComponent<ModListHeaderController>();
            
            ModListHighlight = Object.Instantiate(parent.GetComponent<HGHeaderNavigationController>().headerHighlightObject,
                parent.GetComponent<HGHeaderNavigationController>().headerHighlightObject.transform.parent);
            
            foreach (var imageComp in ModListHighlight.GetComponentsInChildren<Image>())
                imageComp.maskable = false;

            ModListHighlight.SetActive(true);
            
            HGHeaderNavigationController modListController = ModListPanel.AddComponent<HGHeaderNavigationController>();
            modListController.headerHighlightObject = ModListHighlight;
            modListController.unselectedTextColor = Color.white;
            modListController.makeSelectedHeaderButtonNoninteractable = true;
            
            ModListPanel.SetActive(true);
        }

        private void CreateModDescriptionPanel()
        {
            ModDescriptionPanel = Object.Instantiate(_optionsPanel, Canvas.transform);

            ModDescriptionPanel.GetComponent<RectTransform>().anchorMin = new Vector2(0.275f, 0f);
            ModDescriptionPanel.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);

            Transform mdpVerticalLayout = ModDescriptionPanel.transform.Find("Scroll View").Find("Viewport");
            GameObject descriptionPanel = Object.Instantiate(_genericDescriptionPanel, mdpVerticalLayout);

            ModDescriptionPanel.SetActive(true);
            ModDescriptionPanel.name = "Mod Description Panel";
            
            descriptionPanel.transform.Find("ContentSizeFitter").Find("DescriptionText").SetParent(descriptionPanel.transform);

            Object.DestroyImmediate(descriptionPanel.transform.Find("ContentSizeFitter").gameObject);
            
            descriptionPanel.GetComponent<RectTransform>().anchorMin = Vector2.zero;

            var descriptionText = descriptionPanel.transform.Find("DescriptionText").gameObject;

            var sizeFitter = descriptionText.AddComponent<ContentSizeFitter>();
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            sizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            
            var descriptionTransform = descriptionText.GetComponent<RectTransform>();
            descriptionTransform.pivot = new Vector2(0, 1);
            descriptionTransform.anchorMin = Vector2.zero;
            descriptionTransform.anchorMax = Vector2.one;
            descriptionTransform.anchoredPosition = Vector2.zero;

            descriptionText.GetComponent<HGTextMeshProUGUI>().margin = new Vector4(8, 8, 8, 8);
            
            var scrollRect = ModDescriptionPanel.GetComponentInChildren<ScrollRect>();
            scrollRect.content = descriptionText.GetComponent<RectTransform>();;
        }

        private void CreateCategoryHeader(GameObject parent, GameObject headerArea)
        {
            CategoryHeader = Object.Instantiate(_optionsPanel, Canvas.transform);

            CategoryHeader.GetComponent<RectTransform>().anchorMin = new Vector2(0.275f, 0.86f);
            CategoryHeader.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);

            CategoryHeader.SetActive(false);
            CategoryHeader.name = "Category Headers";
            
            Object.DestroyImmediate(CategoryHeader.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").gameObject);
            Object.DestroyImmediate(CategoryHeader.transform.Find("Scroll View").Find("Scrollbar Vertical").gameObject);

            GameObject headers = Object.Instantiate(headerArea, CategoryHeader.transform.Find("Scroll View").Find("Viewport"));
            headers.name = "Categories (JUICED)";

            Object.DestroyImmediate(headers.GetComponent<OnEnableEvent>());
            Object.DestroyImmediate(headers.GetComponent<AwakeEvent>());

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
                if (oldButton == null)
                    continue;
                
                if (oldButton != headers.GetComponent<RectTransform>())
                    Object.DestroyImmediate(oldButton.gameObject);
            }

            CategoryHeaderHighlight = Object.Instantiate(parent.GetComponent<HGHeaderNavigationController>().headerHighlightObject,
                parent.GetComponent<HGHeaderNavigationController>().headerHighlightObject.transform.parent);

            CategoryHeaderHighlight.SetActive(false);

            HGHeaderNavigationController categoryController = headers.AddComponent<HGHeaderNavigationController>();

            categoryController.headerHighlightObject = CategoryHeaderHighlight;
            categoryController.unselectedTextColor = Color.white;

            categoryController.makeSelectedHeaderButtonNoninteractable = true;

            var categoryViewPortHeaderRectTransform = CategoryHeader.transform.Find("Scroll View").Find("Viewport").gameObject.GetComponent<RectTransform>();

            categoryViewPortHeaderRectTransform.anchorMin = new Vector2(0.11f, 0);
            categoryViewPortHeaderRectTransform.anchorMax = new Vector2(0.89f, 1);

            UISkinData skinData = CategoryHeader.transform.Find("Scroll View").GetComponent<ScrollRectSkinController>().skinData;
            UILayerKey layerKey = CategoryHeader.transform.Find("Scroll View").GetComponent<HGScrollRectHelper>().requiredTopLayer;

            Object.DestroyImmediate(CategoryHeader.transform.Find("Scroll View").GetComponent<ScrollRectSkinController>());
            Object.DestroyImmediate(CategoryHeader.transform.Find("Scroll View").GetComponent<HGScrollRectHelper>());
            Object.DestroyImmediate(CategoryHeader.transform.Find("Scroll View").GetComponent<ScrollRect>());

            CategoryScrollRect categoryScrollRect = CategoryHeader.transform.Find("Scroll View").gameObject.AddComponent<CategoryScrollRect>();

            categoryScrollRect.inertia = false;
            categoryScrollRect.content = headers.GetComponent<RectTransform>();
            categoryScrollRect.content.pivot = new Vector2(0, 0.5f);
            categoryScrollRect.horizontal = true;
            categoryScrollRect.vertical = false;
            categoryScrollRect.movementType = ScrollRect.MovementType.Unrestricted;
            categoryScrollRect.viewport = categoryViewPortHeaderRectTransform;
            categoryScrollRect.horizontalScrollbar = null;

            CategoryHeader.transform.Find("Scroll View").gameObject.AddComponent<HGScrollRectHelper>().requiredTopLayer = layerKey;
            CategoryHeader.transform.Find("Scroll View").gameObject.AddComponent<ScrollRectSkinController>().skinData = skinData;

            ContentSizeFitter sizeFitter = headers.AddComponent<ContentSizeFitter>();

            sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
            
            HorizontalLayoutGroup hlg = headers.GetComponent<HorizontalLayoutGroup>();

            hlg.enabled = true;
            hlg.padding = new RectOffset(8, 8, 4, 4);
            hlg.spacing = 16;
            hlg.childAlignment = TextAnchor.MiddleCenter;
            hlg.childControlWidth = true;
            hlg.childControlHeight = true;
            hlg.childForceExpandWidth = true;
            hlg.childForceExpandHeight = true;
        }

        private void CreateAdditionalCategoryStuff()
        {
            CategoryHeaderButton = Object.Instantiate(ModOptionsHeaderButton, CategoryHeader.transform);
            CategoryHeaderButton.name = "Category Header Button Prefab";
            CategoryHeaderButton.SetActive(false);
            
            GameObject scrollView = CategoryHeader.transform.Find("Scroll View").gameObject;

            CategoryPageIndicators = new GameObject("Indicators", typeof(RectTransform));
            CategoryPageIndicators.transform.SetParent(scrollView.transform);
            
            GameObject layoutGroup = new GameObject("LayoutGroup", typeof(RectTransform), typeof(HorizontalLayoutGroup), typeof(ContentSizeFitter));
            layoutGroup.transform.SetParent(CategoryPageIndicators.transform);

            var horizontalLayoutGroup = layoutGroup.GetComponent<HorizontalLayoutGroup>();

            horizontalLayoutGroup.spacing = CategoryScrollRect.Spacing;
            horizontalLayoutGroup.childControlHeight = true;
            horizontalLayoutGroup.childControlWidth = true;
            horizontalLayoutGroup.childForceExpandHeight = true;
            horizontalLayoutGroup.childForceExpandWidth = true;
            horizontalLayoutGroup.childAlignment = TextAnchor.LowerCenter;

            var layoutRectTransform = layoutGroup.GetComponent<RectTransform>();

            layoutRectTransform.anchorMin = new Vector2(0.5f, 0);
            layoutRectTransform.anchorMax = new Vector2(0.5f, 0);
            layoutRectTransform.anchoredPosition = new Vector2(0, 8);
            layoutRectTransform.pivot = new Vector2(0.5f, 0.5f);

            var fitter = layoutGroup.GetComponent<ContentSizeFitter>();
            
            fitter.horizontalFit = ContentSizeFitter.FitMode.MinSize;
            fitter.verticalFit = ContentSizeFitter.FitMode.MinSize;

            var holderRectTransform = CategoryPageIndicators.GetComponent<RectTransform>();

            holderRectTransform.sizeDelta = Vector2.zero;
            holderRectTransform.anchorMin = new Vector2(0, 0f);
            holderRectTransform.anchorMax = new Vector2(1, 0.23f);
            holderRectTransform.pivot = new Vector2(0.5f, 1f);
            holderRectTransform.anchoredPosition = new Vector2(0, 0);

            CategoryPageIndicator = new GameObject("Indicator Dot", typeof(RectTransform), typeof(LayoutElement), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            CategoryPageIndicator.transform.SetParent(CategoryPageIndicators.transform);
            CategoryPageIndicator.SetActive(false);

            var image = CategoryPageIndicator.GetComponent<Image>();
            image.sprite = Assets.Load<Sprite>("assets/RiskOfOptions/IndicatorDot.png");
            image.preserveAspect = true;

            var dotRectTransform = CategoryPageIndicator.GetComponent<RectTransform>();
            dotRectTransform.pivot = Vector2.zero;

            var dotLayoutElement = CategoryPageIndicator.GetComponent<LayoutElement>();
            dotLayoutElement.minWidth = CategoryScrollRect.DotScale;
            dotLayoutElement.minHeight = CategoryScrollRect.DotScale;
            
            CategoryPageIndicatorOutline = new GameObject("Indicator Outline", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            CategoryPageIndicatorOutline.transform.SetParent(CategoryPageIndicators.transform);
            CategoryPageIndicatorOutline.SetActive(true);
            
            CategoryPageIndicatorOutline.GetComponent<Image>().sprite = Assets.Load<Sprite>("assets/RiskOfOptions/IndicatorOutline.png");
            
            var outlineRectTransform = CategoryPageIndicatorOutline.GetComponent<RectTransform>();
            outlineRectTransform.pivot = Vector2.zero;

            CategoryLeftButton = Object.Instantiate(_emptyButton, scrollView.transform);
            Object.DestroyImmediate(CategoryLeftButton.GetComponent<LayoutElement>());

            var leftButtonRectTransform = CategoryLeftButton.GetComponent<RectTransform>();
            leftButtonRectTransform.sizeDelta = new Vector2(64, 64);
            leftButtonRectTransform.anchoredPosition = new Vector2(60, -54);

            CategoryLeftButton.GetComponentInChildren<LanguageTextMeshController>().token = LanguageTokens.LeftPageButton;

            var leftButtonText = CategoryLeftButton.GetComponentInChildren<HGTextMeshProUGUI>();
            leftButtonText.alignment = TextAlignmentOptions.Midline;
            leftButtonText.enableAutoSizing = false;
            leftButtonText.fontSize = 72;

            CategoryLeftButton.GetComponent<HGButton>().onClick.RemoveAllListeners();
            
            CategoryRightButton = Object.Instantiate(CategoryLeftButton, scrollView.transform);

            CategoryRightButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(1064, -54);
             
            CategoryRightButton.GetComponentInChildren<LanguageTextMeshController>().token = LanguageTokens.RightPageButton;

            CategoryLeftButton.name = "Previous Category Page Button";
            CategoryRightButton.name = "Next Category Page Button";
            
            var scrollRect = scrollView.GetComponent<CategoryScrollRect>();
            
            scrollRect.leftButton = CategoryLeftButton.GetComponent<HGButton>();
            scrollRect.rightButton = CategoryRightButton.GetComponent<HGButton>();

            scrollRect.leftButton.disablePointerClick = false;
            scrollRect.rightButton.disablePointerClick = false;
            
            scrollRect.leftButton.onClick.AddListener(scrollRect.Previous);
            scrollRect.rightButton.onClick.AddListener(scrollRect.Next);

            scrollRect.indicatorPrefab = CategoryPageIndicator;
            scrollRect.emptyPrefab = ModOptionsHeaderButton;
            scrollRect.categoryTransform = CategoryHeader.transform.Find("Scroll View").Find("Viewport").Find("Categories (JUICED)");
            scrollRect.outline = CategoryPageIndicatorOutline;
            
            CategoryPageIndicatorOutline.SetActive(false);
            CategoryLeftButton.SetActive(false);
            CategoryRightButton.SetActive(false);
        }

        private void CreateModOptionsPanel()
        {
            // ModOptionsPanel = Object.Instantiate(_optionsPanel, Canvas.transform);
            //
            // ModOptionsPanel.GetComponent<RectTransform>().anchorMin = new Vector2(0.275f, 0);
            // ModOptionsPanel.GetComponent<RectTransform>().anchorMax = new Vector2(0.625f, 0.82f);
            // ModOptionsPanel.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").GetComponent<VerticalLayoutGroup>().childForceExpandHeight = false;
            // ModOptionsPanel.SetActive(false);
            // ModOptionsPanel.name = "Options Panel";

            ModOptionsPanel = Object.Instantiate(Prefabs.modOptionsPanel, Canvas.transform);
        }

        private void CreateModOptionsDescriptionPanel()
        {
            ModOptionsDescriptionPanel = Object.Instantiate(ModDescriptionPanel, Canvas.transform);
            ModOptionsDescriptionPanel.SetActive(false);
            ModOptionsDescriptionPanel.name = "Option Description Panel";

            var panelTransform = ModOptionsDescriptionPanel.GetComponent<RectTransform>();
            panelTransform.anchorMin = new Vector2(0.65f, 0);
            panelTransform.anchorMax = new Vector2(1f, 0.82f);
        }

        private void CreateWarningPanel()
        {
            WarningPanel = Object.Instantiate(_genericDescriptionPanel, ModListPanel.transform);
            
            WarningPanel.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
            WarningPanel.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0f);
            WarningPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -9f);

            GameObject warningBlur = Object.Instantiate(ModListPanel.transform.Find("Scroll View").Find("BlurPanel").gameObject, WarningPanel.transform);
            GameObject warningImage = Object.Instantiate(ModListPanel.transform.Find("Scroll View").Find("ImagePanel").gameObject, WarningPanel.transform);

            var sizeFitterObject = WarningPanel.transform.Find("ContentSizeFitter").gameObject;
            sizeFitterObject.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);

            Object.DestroyImmediate(sizeFitterObject.GetComponent<VerticalLayoutGroup>());

            var layoutGroup = sizeFitterObject.AddComponent<HorizontalLayoutGroup>();
            layoutGroup.childAlignment = TextAnchor.MiddleLeft;
            layoutGroup.spacing = 16;
            layoutGroup.childForceExpandHeight = false;

            var sizeFitter = sizeFitterObject.GetComponent<ContentSizeFitter>();
            sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;

            RectTransform warningDescriptionRectTransform = WarningPanel.transform.Find("ContentSizeFitter").Find("DescriptionText").GetComponent<RectTransform>();
            
            warningDescriptionRectTransform.gameObject.AddComponent<LayoutElement>();

            sizeFitterObject.transform.SetAsLastSibling();
            WarningPanel.AddComponent<RectMask2D>();
            
            warningBlur.GetComponent<TranslucentImage>().color = Color.red;
            warningImage.GetComponent<Image>().color = Color.red;

            WarningPanel.name = "Warning Panel";
            WarningPanel.SetActive(false);
            
            GameObject restartIconGameObject = new GameObject("RestartIcon");

            RectTransform restartIconRectTransform = restartIconGameObject.AddComponent<RectTransform>();
            restartIconGameObject.AddComponent<CanvasRenderer>();

            LayoutElement restartIconLayoutElement = restartIconGameObject.AddComponent<LayoutElement>();
            restartIconLayoutElement.preferredWidth = 30;

            Image restartIcon = restartIconGameObject.AddComponent<Image>();
            restartIcon.sprite = Assets.Load<Sprite>("assets/RiskOfOptions/ror2RestartSymbol.png");
            restartIcon.preserveAspect = true;

            restartIconGameObject.transform.SetParent(sizeFitterObject.transform);
            restartIconGameObject.transform.SetAsFirstSibling();
        }
    }
}