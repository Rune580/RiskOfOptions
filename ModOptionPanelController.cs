using R2API;
using RoR2.UI;
using RoR2.UI.SkinControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace RiskOfOptions
{
    public class ModOptionPanelController : MonoBehaviour
    {
        private GameObject ModListPanel;
        private GameObject ModDescriptionPanel;
        private GameObject CategoryHeader;
        private GameObject OptionsPanel;
        private GameObject OptionDescriptionPanel;

        public void Start()
        {
            CreatePrefabs();
            CreatePanel();
            AddPanelsToSettings();
        }
        private void CreatePrefabs()
        {
            Transform SubPanelArea = transform.Find("SafeArea").Find("SubPanelArea");
            Transform HeaderArea = transform.Find("SafeArea").Find("HeaderContainer").Find("Header (JUICED)");

            Prefabs.GDP = GameObject.Instantiate(SubPanelArea.Find("GenericDescriptionPanel").gameObject);

            UnityEngine.Object.DestroyImmediate(Prefabs.GDP.GetComponentInChildren<DisableIfTextIsEmpty>());
            UnityEngine.Object.DestroyImmediate(Prefabs.GDP.GetComponentInChildren<LanguageTextMeshController>());
            UnityEngine.Object.DestroyImmediate(Prefabs.GDP.transform.Find("ContentSizeFitter").Find("BlurPanel").gameObject);
            UnityEngine.Object.DestroyImmediate(Prefabs.GDP.transform.Find("ContentSizeFitter").Find("CornerRect").gameObject);

            GameObject audioPanel = SubPanelArea.Find("SettingsSubPanel, Audio").gameObject;

            Prefabs.MOPanelPrefab = GameObject.Instantiate(audioPanel);
            Prefabs.MOPanelPrefab.name = "SettingsSubPanel, Mod Options";

            Prefabs.MOHeaderButtonPrefab = GameObject.Instantiate(HeaderArea.Find("GenericHeaderButton (Audio)").gameObject);

            GameObject verticalLayout = Prefabs.MOPanelPrefab.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").gameObject;

            Prefabs.ModButtonPrefab = GameObject.Instantiate(verticalLayout.transform.Find("SettingsEntryButton, Bool (Audio Focus)").gameObject);

            UnityEngine.Object.DestroyImmediate(Prefabs.ModButtonPrefab.GetComponentInChildren<CarouselController>());
            UnityEngine.Object.DestroyImmediate(Prefabs.ModButtonPrefab.GetComponentInChildren<ButtonSkinController>());
            UnityEngine.Object.DestroyImmediate(Prefabs.ModButtonPrefab.transform.Find("CarouselRect").gameObject);

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

            colors.disabledColor = Prefabs.MOHeaderButtonPrefab.GetComponent<HGButton>().colors.disabledColor;


            ROOModListButton newButton = Prefabs.ModButtonPrefab.AddComponent<ROOModListButton>();
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


            UnityEngine.Object.DestroyImmediate(Prefabs.ModButtonPrefab.GetComponent<LanguageTextMeshController>());


            Prefabs.ModButtonPrefab.SetActive(false);
        }

        private void CreatePanel()
        {
            Prefabs.MOPanelPrefab.name = "SettingsSubPanel, Mod Options";

            Transform HeaderArea = transform.Find("SafeArea").Find("HeaderContainer").Find("Header (JUICED)");

            GameObject verticalLayout = Prefabs.MOPanelPrefab.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").gameObject;

            UnityEngine.Object.DestroyImmediate(verticalLayout.transform.Find("SettingsEntryButton, Slider (Master Volume)").gameObject);
            UnityEngine.Object.DestroyImmediate(verticalLayout.transform.Find("SettingsEntryButton, Slider (SFX Volume)").gameObject);
            UnityEngine.Object.DestroyImmediate(verticalLayout.transform.Find("SettingsEntryButton, Slider (MSX Volume)").gameObject);
            UnityEngine.Object.DestroyImmediate(verticalLayout.transform.Find("SettingsEntryButton, Bool (Audio Focus)").gameObject);

            Prefabs.MOCanvas = GameObject.Instantiate(Prefabs.MOPanelPrefab, Prefabs.MOPanelPrefab.transform.parent);

            Prefabs.MOCanvas.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);

            GameObject.DestroyImmediate(Prefabs.MOCanvas.GetComponent<SettingsPanelController>());
            GameObject.DestroyImmediate(Prefabs.MOCanvas.GetComponent<UnityEngine.UI.Image>());
            GameObject.DestroyImmediate(Prefabs.MOCanvas.GetComponent<HGButtonHistory>());

            GameObject.DestroyImmediate(Prefabs.MOCanvas.transform.Find("Scroll View").gameObject);

            Prefabs.MOCanvas.AddComponent<GenericDescriptionController>();

            Prefabs.MOCanvas.name = "SettingsSubPanel, Mod Options";



            ModListPanel = GameObject.Instantiate(Prefabs.MOPanelPrefab, Prefabs.MOCanvas.transform);

            ModListPanel.GetComponent<RectTransform>().anchorMax = new Vector2(0.25f, 1f);

            ModListPanel.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").GetComponent<VerticalLayoutGroup>().spacing = 6;

            ModListPanel.SetActive(true);

            ModListPanel.name = "Mod List Panel";



            ModDescriptionPanel = GameObject.Instantiate(Prefabs.MOPanelPrefab, Prefabs.MOCanvas.transform);

            ModDescriptionPanel.GetComponent<RectTransform>().anchorMin = new Vector2(0.275f, 0f);
            ModDescriptionPanel.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);

            Transform MDPVerticalLayout = ModDescriptionPanel.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout");

            GameObject.Instantiate(Prefabs.GDP, MDPVerticalLayout);

            ModDescriptionPanel.SetActive(true);

            ModDescriptionPanel.name = "Mod Description Panel";

            CategoryHeader = GameObject.Instantiate(Prefabs.MOPanelPrefab, Prefabs.MOCanvas.transform);

            CategoryHeader.GetComponent<RectTransform>().anchorMin = new Vector2(0.275f, 0.86f);
            CategoryHeader.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);

            CategoryHeader.SetActive(false);

            CategoryHeader.name = "Category Headers";


            GameObject.DestroyImmediate(CategoryHeader.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").gameObject);
            GameObject.DestroyImmediate(CategoryHeader.transform.Find("Scroll View").Find("Scrollbar Vertical").gameObject);


            GameObject Headers = GameObject.Instantiate(HeaderArea.gameObject, CategoryHeader.transform.Find("Scroll View").Find("Viewport"));
            Headers.name = "Categories (JUICED)";

            Headers.GetComponent<CanvasGroup>().alpha = 1;

            RectTransform[] oldButtons = Headers.GetComponentsInChildren<RectTransform>();

            for (int i = 0; i < oldButtons.Length; i++)
            {
                if (oldButtons[i] != null)
                {
                    if (oldButtons[i] != Headers.GetComponent<RectTransform>())
                    {
                        //Debug.Log($"Destroying {oldButtons[i]}");
                        GameObject.DestroyImmediate(oldButtons[i].gameObject);
                    }
                }
            }

            //foreach (var rectTransform in Headers.GetComponentsInChildren<RectTransform>())
            //{
            //    Debug.Log(rectTransform.gameObject);
            //    if (rectTransform != Headers.GetComponent<RectTransform>())
            //    {
            //        GameObject.DestroyImmediate(rectTransform.gameObject);
            //    }
            //}


            ScrollRect scrollRectScript = CategoryHeader.transform.Find("Scroll View").GetComponent<ScrollRect>();


            scrollRectScript.content = CategoryHeader.transform.Find("Scroll View").Find("Viewport").Find("Categories (JUICED)").GetComponent<RectTransform>();


            scrollRectScript.horizontal = true;
            scrollRectScript.vertical = false;


            RectTransform scrollBar = CategoryHeader.transform.Find("Scroll View").Find("Scrollbar Horizontal").gameObject.GetComponent<RectTransform>();

            scrollRectScript.horizontalScrollbar = scrollBar.GetComponent<CustomScrollbar>();

            scrollBar.anchorMin = new Vector2(0, 0);
            scrollBar.anchorMax = new Vector2(1, 0);


            ContentSizeFitter sizeFitter = Headers.AddComponent<ContentSizeFitter>();

            sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;


            HorizontalLayoutGroup HLG = Headers.GetComponent<HorizontalLayoutGroup>();

            HLG.enabled = true;

            HLG.padding = new RectOffset(4, 4, 4, 4);
            HLG.spacing = 16;
            HLG.childAlignment = TextAnchor.MiddleCenter;
            HLG.childControlWidth = true;
            HLG.childControlHeight = true;
            HLG.childForceExpandWidth = true;
            HLG.childForceExpandHeight = true;


            OptionsPanel = GameObject.Instantiate(Prefabs.MOPanelPrefab, Prefabs.MOCanvas.transform);

            OptionsPanel.GetComponent<RectTransform>().anchorMin = new Vector2(0.275f, 0);
            OptionsPanel.GetComponent<RectTransform>().anchorMax = new Vector2(0.625f, 0.82f);

            OptionsPanel.SetActive(false);

            OptionsPanel.name = "Options Panel";

            OptionDescriptionPanel = GameObject.Instantiate(Prefabs.MOPanelPrefab, Prefabs.MOCanvas.transform);

            OptionDescriptionPanel.GetComponent<RectTransform>().anchorMin = new Vector2(0.65f, 0);
            OptionDescriptionPanel.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0.82f);

            OptionDescriptionPanel.SetActive(false);

            OptionDescriptionPanel.name = "Option Description Panel";


            CreateModListButtons();
        }

        private void AddPanelsToSettings()
        {
            Transform SubPanelArea = transform.Find("SafeArea").Find("SubPanelArea");
            Transform HeaderArea = transform.Find("SafeArea").Find("HeaderContainer").Find("Header (JUICED)");

            GameObject ModOptionsPanel = GameObject.Instantiate(Prefabs.MOCanvas, SubPanelArea);

            HGHeaderNavigationController navigationController = GetComponent<HGHeaderNavigationController>();

            LanguageAPI.Add(Prefabs.HeaderButtonTextToken, "Mod Options");

            GameObject ModOptionsHeaderButton = GameObject.Instantiate(Prefabs.MOHeaderButtonPrefab, HeaderArea);

            ModOptionsHeaderButton.name = "GenericHeaderButton (Mod Options)";
            ModOptionsHeaderButton.GetComponentInChildren<LanguageTextMeshController>().token = Prefabs.HeaderButtonTextToken;
            ModOptionsHeaderButton.GetComponentInChildren<HGButton>().onClick.RemoveAllListeners();
            ModOptionsHeaderButton.GetComponentInChildren<HGButton>().onClick.AddListener(new UnityEngine.Events.UnityAction(
            delegate ()
            {
                navigationController.ChooseHeaderByButton(ModOptionsHeaderButton.GetComponentInChildren<HGButton>());
            }));

            List<HGHeaderNavigationController.Header> headers = GetComponent<HGHeaderNavigationController>().headers.ToList();

            ModOptionsHeaderButton.GetComponentInChildren<HGTextMeshProUGUI>().SetText("MOD OPTIONS");

            HGHeaderNavigationController.Header header = new HGHeaderNavigationController.Header();

            header.headerButton = ModOptionsHeaderButton.GetComponent<HGButton>();
            header.headerName = "Mod Options";
            header.tmpHeaderText = ModOptionsHeaderButton.GetComponentInChildren<HGTextMeshProUGUI>();
            header.headerRoot = ModOptionsPanel;

            headers.Add(header);

            GetComponent<HGHeaderNavigationController>().headers = headers.ToArray();
        }


        private void CreateModListButtons()
        {
            Transform ModListLayout = ModListPanel.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout");


            for (int i = 0; i < ModSettingsManager.optionContainers.Count; i++)
            {
                var Container = ModSettingsManager.optionContainers[i];

                GameObject newModButton = GameObject.Instantiate(Prefabs.ModButtonPrefab, ModListLayout);

                LanguageAPI.Add($"{ModSettingsManager.StartingText}.{Container.ModGUID}.{Container.ModName}.ModListOption".ToUpper().Replace(" ", "_"), Container.ModName);

                //newModButton.GetComponent<LanguageTextMeshController>().token = $"{ModSettingsManager.StartingText}.{Container.ModGUID}.{Container.ModName}.ModListOption".ToUpper().Replace(" ", "_");

                newModButton.GetComponentInChildren<HGTextMeshProUGUI>().text = Container.ModName;

                newModButton.GetComponent<ROOModListButton>().Description = Container.Description;
                newModButton.GetComponent<ROOModListButton>().tmp = ModDescriptionPanel.GetComponentInChildren<HGTextMeshProUGUI>();

                //Debug.Log($"Loading Mod List Button {i}");

                newModButton.GetComponent<ROOModListButton>().ContainerIndex = i;

                newModButton.GetComponent<ROOModListButton>().hoverToken = $"{ModSettingsManager.StartingText}.{Container.ModGUID}.{Container.ModName}.ModListOption".ToUpper().Replace(" ", "_");

                newModButton.name = $"ModListButton ({Container.ModName})";

                newModButton.SetActive(true);
            }
        }

        internal void LoadModOptionsFromContainer(int ContainerIndex, Transform canvas)
        {
            OptionContainer Container = ModSettingsManager.optionContainers[ContainerIndex];

            //Debug.Log($"Loading Container: {Container.ModName}");

            var MDP = canvas.Find("Mod Description Panel").gameObject;
            var CH = canvas.Find("Category Headers").gameObject;
            var OP = canvas.Find("Options Panel").gameObject;
            var ODP = canvas.Find("Option Description Panel").gameObject;

            GameObject CategoriesObject = CH.transform.Find("Scroll View").Find("Viewport").Find("Categories (JUICED)").gameObject;

            if (CH.activeSelf || OP.activeSelf || ODP.activeSelf)
            {
                UnloadExistingCategoryButtons(CategoriesObject.transform);
            }

            if (MDP.activeSelf || !CH.activeSelf || !OP.activeSelf || !ODP.activeSelf)
            {
                MDP.GetComponentInChildren<HGTextMeshProUGUI>().SetText("");

                MDP.SetActive(false);

                CH.SetActive(true);
                OP.SetActive(true);
                ODP.SetActive(true);
            }

            for (int i = 0; i < Container.GetCategoriesCached().Count; i++)
            {
                //Debug.Log($"Loading Category: {Container.GetCategoriesCached()[i].Name}");

                GameObject newCategoryButton = GameObject.Instantiate(Prefabs.MOHeaderButtonPrefab, CategoriesObject.transform);

                LayoutElement le = newCategoryButton.AddComponent<LayoutElement>();

                le.preferredWidth = 200;

                newCategoryButton.GetComponentInChildren<LanguageTextMeshController>().token = Container.GetCategoriesCached()[i].NameToken;

                newCategoryButton.GetComponentInChildren<HGTextMeshProUGUI>().SetText(Container.GetCategoriesCached()[i].Name);

                newCategoryButton.GetComponentInChildren<HGButton>().onClick.RemoveAllListeners();

                newCategoryButton.name = $"Category Button, {Container.GetCategoriesCached()[i].Name}";

                newCategoryButton.SetActive(true);
            }
        }

        internal void UnLoad(Transform canvas)
        {
            var MDP = canvas.Find("Mod Description Panel").gameObject;
            var CH = canvas.Find("Category Headers").gameObject;
            var OP = canvas.Find("Options Panel").gameObject;
            var ODP = canvas.Find("Option Description Panel").gameObject;

            if (!MDP.activeSelf)
            {
                MDP.SetActive(true);

                CH.SetActive(false);
                OP.SetActive(false);
                ODP.SetActive(false);
            }

            ResetModListButtons(canvas);
        }

        internal void ResetModListButtons(Transform canvas)
        {
            foreach (var Button in canvas.GetComponentsInChildren<ROOModListButton>())
            {
                Button.Selected = false;
                Button.showImageOnHover = true;

                Color tempColor = Button.imageOnHover.color;

                Button.imageOnHover.color = new Color(tempColor.r, tempColor.g, tempColor.b, 0f);
                Button.imageOnHover.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            }
        }

        internal void UnloadExistingCategoryButtons(Transform CH)
        {
            HGButton[] activeCategoryButtons = CH.GetComponentsInChildren<HGButton>();

            if (activeCategoryButtons.Length > 0)
            {
                for (int i = 0; i < activeCategoryButtons.Length; i++)
                {
                    if (activeCategoryButtons[i].gameObject != null)
                    {
                        //Debug.Log($"Unloading {activeCategoryButtons[i].gameObject}");

                        activeCategoryButtons[i].gameObject.SetActive(false);

                        GameObject.DestroyImmediate(activeCategoryButtons[i].gameObject);
                    }
                }
            }
        }
    }
}
