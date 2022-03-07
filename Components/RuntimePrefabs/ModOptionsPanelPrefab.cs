using JetBrains.Annotations;
using LeTai.Asset.TranslucentImage;
using RiskOfOptions.Components.OptionComponents;
using RiskOfOptions.Components.Options;
using RiskOfOptions.Resources;
using RoR2;
using RoR2.UI;
using RoR2.UI.SkinControllers;
using UnityEngine;
using UnityEngine.UI;

namespace RiskOfOptions.Components.RuntimePrefabs
{
    public class ModOptionsPanelPrefab : IRuntimePrefab
    {
        [CanBeNull] private GameObject _optionsPanel;
        [CanBeNull] private GameObject _genericDescriptionPanel;
        [CanBeNull] private GameObject _verticalLayout;
        
        public GameObject ModListButton { get; private set; }
        public GameObject ModOptionsHeaderButton { get; private set; }
        public GameObject EmptyButton { get; private set; }
        public GameObject Canvas { get; private set; }
        public GameObject ModListPanel { get; private set;}
        public GameObject ModListHighlight { get; private set; }
        public GameObject ModDescriptionPanel { get; private set; }
        public GameObject CategoryHeader { get; private set; }
        public GameObject CategoryHeaderHighlight { get; private set; }
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
            CreateVerticalLayout();
            CreateModListButton();
            CreateEmptyButton();
            
            Object.DestroyImmediate(_verticalLayout!.transform.Find("SettingsEntryButton, Bool (Audio Focus)").gameObject);

            CreateCanvas();
            CreateModListPanel(settingsPanel);
            CreateModDescriptionPanel();
            CreateCategoryHeader(settingsPanel, headerArea.gameObject);
            CreateModOptionsPanel();
            
            Object.DestroyImmediate(_optionsPanel);
            Object.DestroyImmediate(_verticalLayout);
            
            CreateModOptionsDescriptionPanel();
            CreateWarningPanel();
            
            Object.DestroyImmediate(_genericDescriptionPanel);
        }

        public void Destroy()
        {
            Object.DestroyImmediate(ModListButton);
            Object.DestroyImmediate(ModOptionsHeaderButton);
            Object.DestroyImmediate(EmptyButton);
            Object.DestroyImmediate(Canvas);
            Object.DestroyImmediate(ModListPanel);
            Object.DestroyImmediate(ModListHighlight);
            Object.DestroyImmediate(ModDescriptionPanel);
            Object.DestroyImmediate(CategoryHeader);
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
        }

        private void CreateVerticalLayout()
        {
            _verticalLayout = _optionsPanel!.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").gameObject;
            
            Object.DestroyImmediate(_verticalLayout.transform.Find("SettingsEntryButton, Slider (Master Volume)").gameObject);
            Object.DestroyImmediate(_verticalLayout.transform.Find("SettingsEntryButton, Slider (SFX Volume)").gameObject);
            Object.DestroyImmediate(_verticalLayout.transform.Find("SettingsEntryButton, Slider (MSX Volume)").gameObject);
        }

        private void CreateModListButton()
        {
            ModListButton = Object.Instantiate(_verticalLayout!.transform.Find("SettingsEntryButton, Bool (Audio Focus)").gameObject);

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
            
            Object.DestroyImmediate(ModListButton.GetComponent<LanguageTextMeshController>());
            
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
            EmptyButton = Object.Instantiate(_verticalLayout!.transform.Find("SettingsEntryButton, Bool (Audio Focus)").gameObject);
            
            Object.DestroyImmediate(EmptyButton.GetComponentInChildren<CarouselController>());
            Object.DestroyImmediate(EmptyButton.GetComponentInChildren<ButtonSkinController>());
            Object.DestroyImmediate(EmptyButton.transform.Find("CarouselRect").gameObject);
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
            ModListPanel.SetActive(true);
            ModListPanel.name = "Mod List Panel";
            ModListPanel.AddComponent<ModListHeaderController>();
            
            ModListHighlight = Object.Instantiate(parent.GetComponent<HGHeaderNavigationController>().headerHighlightObject,
                parent.GetComponent<HGHeaderNavigationController>().headerHighlightObject.transform.parent);
            
            foreach (var imageComp in ModListHighlight.GetComponentsInChildren<Image>())
                imageComp.maskable = false;

            ModListHighlight.SetActive(false);
            
            HGHeaderNavigationController modListController = ModListPanel.AddComponent<HGHeaderNavigationController>();
            modListController.headerHighlightObject = ModListHighlight;
            modListController.unselectedTextColor = Color.white;
            modListController.makeSelectedHeaderButtonNoninteractable = true;
        }

        private void CreateModDescriptionPanel()
        {
            ModDescriptionPanel = Object.Instantiate(_optionsPanel, Canvas.transform);

            ModDescriptionPanel.GetComponent<RectTransform>().anchorMin = new Vector2(0.275f, 0f);
            ModDescriptionPanel.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);

            Transform mdpVerticalLayout = ModDescriptionPanel.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout");
            Object.Instantiate(_genericDescriptionPanel, mdpVerticalLayout);

            ModDescriptionPanel.SetActive(true);
            ModDescriptionPanel.name = "Mod Description Panel";
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
                if (oldButton != null)
                {
                    if (oldButton != headers.GetComponent<RectTransform>())
                    {
                        Object.DestroyImmediate(oldButton.gameObject);
                    }
                }
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

        private void CreateModOptionsPanel()
        {
            ModOptionsPanel = Object.Instantiate(_optionsPanel, Canvas.transform);

            ModOptionsPanel.GetComponent<RectTransform>().anchorMin = new Vector2(0.275f, 0);
            ModOptionsPanel.GetComponent<RectTransform>().anchorMax = new Vector2(0.625f, 0.82f);
            ModOptionsPanel.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").GetComponent<VerticalLayoutGroup>().childForceExpandHeight = false;
            ModOptionsPanel.SetActive(false);
            ModOptionsPanel.name = "Options Panel";
        }

        private void CreateModOptionsDescriptionPanel()
        {
            ModOptionsDescriptionPanel = Object.Instantiate(ModDescriptionPanel, Canvas.transform);
            
            ModOptionsDescriptionPanel.GetComponent<RectTransform>().anchorMin = new Vector2(0.65f, 0);
            ModOptionsDescriptionPanel.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0.82f);
            ModOptionsDescriptionPanel.SetActive(false);
            ModOptionsDescriptionPanel.name = "Option Description Panel";
        }

        private void CreateWarningPanel()
        {
            WarningPanel = Object.Instantiate(_genericDescriptionPanel, ModListPanel.transform);

            WarningPanel.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
            WarningPanel.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0f);
            WarningPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -9f);

            GameObject warningBlur = Object.Instantiate(ModListPanel.transform.Find("Scroll View").Find("BlurPanel").gameObject, WarningPanel.transform);
            GameObject warningImage = Object.Instantiate(ModListPanel.transform.Find("Scroll View").Find("ImagePanel").gameObject, WarningPanel.transform);

            WarningPanel.transform.Find("ContentSizeFitter").GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);

            RectTransform warningDescriptionRectTransform = WarningPanel.transform.Find("ContentSizeFitter").Find("DescriptionText").GetComponent<RectTransform>();
            warningDescriptionRectTransform.anchorMin = new Vector2(0.1f, 0);
            warningDescriptionRectTransform.anchorMax = new Vector2(1, 0.5f);

            LayoutElement warningDescriptionLayoutElement = warningDescriptionRectTransform.gameObject.AddComponent<LayoutElement>();
            warningDescriptionLayoutElement.ignoreLayout = true;

            WarningPanel.transform.Find("ContentSizeFitter").SetAsLastSibling();
            WarningPanel.AddComponent<RectMask2D>();

            warningBlur.GetComponent<TranslucentImage>().color = Color.red;
            warningImage.GetComponent<Image>().color = Color.red;

            WarningPanel.name = "Warning Panel";
            WarningPanel.SetActive(false);
            
            GameObject restartIconGameObject = new GameObject("RestartIcon");

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

            restartIconGameObject.transform.SetParent(WarningPanel.transform.Find("ContentSizeFitter"));
            restartIconGameObject.transform.SetAsLastSibling();
        }
    }
}