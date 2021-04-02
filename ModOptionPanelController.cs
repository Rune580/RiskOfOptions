using R2API;
using R2API.Utils;
using RoR2.UI;
using RoR2.UI.SkinControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeTai.Asset.TranslucentImage;
using RoR2;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

using RiskOfOptions.OptionComponents;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace RiskOfOptions
{
    public class ModOptionPanelController : MonoBehaviour
    {
        public bool initilized = false;

        public GameObject modListPanel;
        public GameObject modListHighlight;

        public GameObject warningPanel;

        private GameObject _modDescriptionPanel;
        private GameObject _categoryHeader;
        private GameObject _optionsPanel;
        private GameObject _optionDescriptionPanel;
        private GameObject _categoryHeaderHighlight;

        private GameObject _checkBoxPrefab;
        private GameObject _sliderPrefab;
        private GameObject _keybindPrefab;

        private OverrideController[] _controllers;

        public Color warningColor = Color.red;


        public void Start()
        {
            CreatePrefabs();
            CreatePanel();
            AddPanelsToSettings();
        }
        private void CreatePrefabs()
        {
            Transform subPanelArea = transform.Find("SafeArea").Find("SubPanelArea");
            Transform headerArea = transform.Find("SafeArea").Find("HeaderContainer").Find("Header (JUICED)");

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
            _keybindPrefab = GameObject.Instantiate(subPanelArea.Find("SettingsSubPanel, Controls (M&KB)").Find("Scroll View").Find("Viewport").Find("VerticalLayout").Find("SettingsEntryButton, Binding (Jump)").gameObject);

            #region KeybindPrefabCleaning

            GameObject.DestroyImmediate(_keybindPrefab.GetComponentInChildren<InputBindingControl>());
            GameObject.DestroyImmediate(_keybindPrefab.GetComponentInChildren<InputBindingDisplayController>());

            _keybindPrefab.AddComponent<KeybindController>();

            #endregion

            _checkBoxPrefab.SetActive(false);
            _sliderPrefab.SetActive(false);
            _keybindPrefab.SetActive(false);

            GameObject.DestroyImmediate(Prefabs.ModButtonPrefab.GetComponentInChildren<CarouselController>());
            GameObject.DestroyImmediate(Prefabs.ModButtonPrefab.GetComponentInChildren<ButtonSkinController>());
            GameObject.DestroyImmediate(Prefabs.ModButtonPrefab.transform.Find("CarouselRect").gameObject);

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
            modListPanel.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0.2f);

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
            warningPanel.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0.2f);

            GameObject warningBlur = GameObject.Instantiate(modListPanel.transform.Find("Scroll View").Find("BlurPanel").gameObject, warningPanel.transform);
            GameObject warningImage = GameObject.Instantiate(modListPanel.transform.Find("Scroll View").Find("ImagePanel").gameObject, warningPanel.transform);

            warningBlur.GetComponent<TranslucentImage>().color = warningColor;
            warningImage.GetComponent<UnityEngine.UI.Image>().color = warningColor;

            warningPanel.SetActive(true);

            warningPanel.name = "Warning Panel";




            _categoryHeader = GameObject.Instantiate(Prefabs.MoPanelPrefab, Prefabs.MoCanvas.transform);

            _categoryHeader.GetComponent<RectTransform>().anchorMin = new Vector2(0.275f, 0.86f);
            _categoryHeader.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);

            _categoryHeader.SetActive(false);

            _categoryHeader.name = "Category Headers";


            GameObject.DestroyImmediate(_categoryHeader.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").gameObject);
            GameObject.DestroyImmediate(_categoryHeader.transform.Find("Scroll View").Find("Scrollbar Vertical").gameObject);


            GameObject headers = GameObject.Instantiate(headerArea.gameObject, _categoryHeader.transform.Find("Scroll View").Find("Viewport"));
            headers.name = "Categories (JUICED)";

            RectTransform rt = headers.GetComponent<RectTransform>();

            rt.pivot = new Vector2(0.5f, 0.5f);

            rt.anchorMin = new Vector2(0f, 0.2f);
            rt.anchorMax = new Vector2(1f, 0.8f);

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


            ScrollRect scrollRectScript = _categoryHeader.transform.Find("Scroll View").GetComponent<ScrollRect>();

            scrollRectScript.content = headers.GetComponent<RectTransform>();


            scrollRectScript.horizontal = true;
            scrollRectScript.vertical = false;


            RectTransform scrollBar = _categoryHeader.transform.Find("Scroll View").Find("Scrollbar Horizontal").gameObject.GetComponent<RectTransform>();

            scrollRectScript.horizontalScrollbar = scrollBar.GetComponent<CustomScrollbar>();

            scrollBar.anchorMin = new Vector2(0, 0);
            scrollBar.anchorMax = new Vector2(1, 0);


            ContentSizeFitter sizeFitter = headers.AddComponent<ContentSizeFitter>();

            sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;


            HorizontalLayoutGroup HLG = headers.GetComponent<HorizontalLayoutGroup>();

            HLG.enabled = true;

            HLG.padding = new RectOffset(4, 4, 4, 4);
            HLG.spacing = 16;
            HLG.childAlignment = TextAnchor.MiddleCenter;
            HLG.childControlWidth = true;
            HLG.childControlHeight = true;
            HLG.childForceExpandWidth = true;
            HLG.childForceExpandHeight = true;



            _optionsPanel = GameObject.Instantiate(Prefabs.MoPanelPrefab, Prefabs.MoCanvas.transform);

            _optionsPanel.GetComponent<RectTransform>().anchorMin = new Vector2(0.275f, 0);
            _optionsPanel.GetComponent<RectTransform>().anchorMax = new Vector2(0.625f, 0.82f);

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
        }


        private void CreateModListButtons()
        {
            Transform modListLayout = modListPanel.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout");

            HGHeaderNavigationController navigationController = modListPanel.GetComponent<HGHeaderNavigationController>();

            List<HGHeaderNavigationController.Header> headers = new List<HGHeaderNavigationController.Header>();

            for (int i = 0; i < ModSettingsManager.OptionContainers.Count; i++)
            {
                var container = ModSettingsManager.OptionContainers[i];

                GameObject newModButton = GameObject.Instantiate(Prefabs.ModButtonPrefab, modListLayout);

                LanguageAPI.Add($"{ModSettingsManager.StartingText}.{container.ModGuid}.{container.ModName}.ModListOption".ToUpper().Replace(" ", "_"), container.Title);

                //newModButton.GetComponent<LanguageTextMeshController>().token = $"{ModSettingsManager.StartingText}.{Container.ModGUID}.{Container.ModName}.ModListOption".ToUpper().Replace(" ", "_");

                newModButton.GetComponentInChildren<HGTextMeshProUGUI>().text = container.Title;

                newModButton.GetComponent<RooModListButton>().description = container.Description;
                newModButton.GetComponent<RooModListButton>().tmp = _modDescriptionPanel.GetComponentInChildren<HGTextMeshProUGUI>();

                newModButton.GetComponent<RooModListButton>().containerIndex = i;

                newModButton.GetComponent<RooModListButton>().navigationController = navigationController;

                newModButton.GetComponent<RooModListButton>().hoverToken = $"{ModSettingsManager.StartingText}.{container.ModGuid}.{container.ModName}.ModListOption".ToUpper().Replace(" ", "_");

                RectTransform modIconRectTransform = newModButton.transform.Find("ModIcon").gameObject.GetComponent<RectTransform>();

                modIconRectTransform.localPosition = new Vector3(-147f, -0.32f, 0f);
                modIconRectTransform.sizeDelta = Vector2.zero;
                modIconRectTransform.anchoredPosition = Vector2.zero;

                //modIconRectTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

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

            ch.transform.Find("Scroll View").Find("Scrollbar Horizontal").gameObject.GetComponent<CustomScrollbar>().onValueChanged.AddListener(new UnityAction<float>(delegate(float value)
                {
                    Debug.Log(value);
                }));

            LoadOptionListFromCategory(containerIndex, navigationController.currentHeaderIndex, navigationController.headers.Length, canvas);
        }

        internal void LoadOptionListFromCategory(int containerIndex, int categoryIndex, int categories, Transform canvas)
        {
            UnloadExistingOptionButtons(canvas.Find("Options Panel"));

            canvas.Find("Category Headers").Find("Scroll View").Find("Scrollbar Horizontal").gameObject.GetComponent<CustomScrollbar>().value = (1f / ((float)categories) - 1) * ((float)categoryIndex);

            canvas.Find("Option Description Panel").GetComponentInChildren<HGTextMeshProUGUI>().SetText("");

            OptionCategory category = ModSettingsManager.OptionContainers[containerIndex].GetCategoriesCached()[categoryIndex];

            var verticalLayoutTransform = canvas.Find("Options Panel").Find("Scroll View").Find("Viewport").Find("VerticalLayout");

            for (int i = 0; i < category.GetModOptionsCached().Count; i++)
            {
                var option = category.GetModOptionsCached()[i];

                GameObject button;

                BaseSettingsControl.SettingSource roo = (BaseSettingsControl.SettingSource) 2;

                switch (option.optionType)
                {
                    case RiskOfOption.OptionType.Bool:
                        button = GameObject.Instantiate(_checkBoxPrefab, verticalLayoutTransform);

                        var controller = button.GetComponentInChildren<CarouselController>();

                        controller.settingName = option.ConsoleToken;
                        controller.nameToken = option.NameToken;

                        controller.settingSource = roo;

                        if (option.OnValueChangedBool != null)
                            button.AddComponent<BoolListener>().onValueChangedBool = option.OnValueChangedBool;

                        button.name = $"Mod Option CheckBox, {option.Name}";
                        break;
                    case RiskOfOption.OptionType.Slider:
                        button = GameObject.Instantiate(_sliderPrefab, verticalLayoutTransform);

                        var slider = button.GetComponentInChildren<SettingsSlider>();

                        slider.settingName = option.ConsoleToken;
                        slider.nameToken = option.NameToken;

                        slider.settingSource = roo;

                        if (option.OnValueChangedFloat != null)
                            slider.slider.onValueChanged.AddListener(option.OnValueChangedFloat);

                        button.name = $"Mod Option Slider, {option.Name}";
                        break;
                    case RiskOfOption.OptionType.Keybinding:
                        button = GameObject.Instantiate(_keybindPrefab, verticalLayoutTransform);

                        var keybindController = button.GetComponentInChildren<KeybindController>();

                        keybindController.settingName = option.ConsoleToken;
                        keybindController.nameToken = option.NameToken;

                        keybindController.settingSource = roo;

                        if (option.OnValueChangedKeyCode != null)
                            keybindController.onValueChangedKeyCode = option.OnValueChangedKeyCode;

                        button.transform.Find("ButtonText").GetComponent<HGTextMeshProUGUI>().SetText(option.Name);

                        button.name = $"Mod Option KeyBind, {option.Name}";

                        foreach (var hgButton in button.GetComponentsInChildren<HGButton>())
                        {
                            hgButton.onClick.RemoveAllListeners();

                            var kbController = keybindController;
                            hgButton.onClick.AddListener(new UnityAction(delegate()
                            {
                                kbController.StartListening();
                            }));
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (option.OptionOverride != null)
                {
                    OverrideController overrideController = button.AddComponent<OverrideController>();

                    overrideController.modGuid = option.ModGuid;

                    overrideController.overridingName = option.OptionOverride.Name;
                    overrideController.overridingCategoryName = option.OptionOverride.CategoryName;

                    overrideController.CheckForOverride();
                }

                button.GetComponentInChildren<HGButton>().hoverToken = option.DescriptionToken;

                button.GetComponentInChildren<HGButton>().onSelect.AddListener(new UnityAction(delegate()
                {
                    canvas.Find("Option Description Panel").GetComponentInChildren<HGTextMeshProUGUI>().SetText(option.Description);
                }));

                button.SetActive(true);
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

        internal void UnLoad(Transform canvas)
        {
            if (!initilized)
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


        internal void UnloadExistingCategoryButtons(Transform ch)
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
        }

        internal void UnloadExistingOptionButtons(Transform op)
        {
            _controllers = Array.Empty<OverrideController>();

            BaseSettingsControl[] activeOptionButtons = op.GetComponentsInChildren<BaseSettingsControl>();

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

            initilized = false;

            GameObject.DestroyImmediate(modListPanel);
            GameObject.DestroyImmediate(_modDescriptionPanel);
            GameObject.DestroyImmediate(_categoryHeader);
            GameObject.DestroyImmediate(_optionsPanel);
            GameObject.DestroyImmediate(_optionDescriptionPanel);
            GameObject.DestroyImmediate(_categoryHeaderHighlight);
            GameObject.DestroyImmediate(modListHighlight);
        }

        public void OnEnable()
        {
            initilized = true;
        }
    }
}
