using R2API;
using RoR2.UI;
using RoR2.UI.SkinControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RiskOfOptions
{
    public class ModOptionPanelController : MonoBehaviour
    {
        private GameObject ModListPanel;

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

            GameObject audioPanel = SubPanelArea.Find("SettingsSubPanel, Audio").gameObject;

            Prefabs.MOPanelPrefab = GameObject.Instantiate(audioPanel);
            Prefabs.MOPanelPrefab.name = "SettingsSubPanel, Mod Options";

            Prefabs.MOHeaderButtonPrefab = GameObject.Instantiate(HeaderArea.Find("GenericHeaderButton (Audio)").gameObject);

            GameObject verticalLayout = Prefabs.MOPanelPrefab.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").gameObject;

            Prefabs.ModButtonPrefab = GameObject.Instantiate(verticalLayout.transform.Find("SettingsEntryButton, Bool (Audio Focus)").gameObject);

            UnityEngine.Object.DestroyImmediate(Prefabs.ModButtonPrefab.GetComponentInChildren<CarouselController>());

            UnityEngine.Object.DestroyImmediate(Prefabs.ModButtonPrefab.GetComponentInChildren<ButtonSkinController>());

            UnityEngine.Object.DestroyImmediate(Prefabs.ModButtonPrefab.transform.Find("CarouselRect").GetComponentInChildren<UnityEngine.UI.Image>());

            foreach (var button in Prefabs.ModButtonPrefab.transform.Find("CarouselRect").GetComponentsInChildren<HGButton>())
            {
                UnityEngine.Object.DestroyImmediate(button);
            }

            foreach (var component in Prefabs.ModButtonPrefab.transform.Find("CarouselRect").GetComponentsInChildren<Component>())
            {
                if (component.GetType() == typeof(UnityEngine.UI.Image))
                {
                    UnityEngine.Object.DestroyImmediate(component);
                }
            }

            Prefabs.ModButtonPrefab.GetComponent<HGButton>().interactable = true;
            Prefabs.ModButtonPrefab.GetComponent<HGButton>().enabled = true;
            Prefabs.ModButtonPrefab.GetComponent<HGButton>().disablePointerClick = false;
            Prefabs.ModButtonPrefab.GetComponent<HGButton>().onClick.RemoveAllListeners();

            UnityEngine.Object.DestroyImmediate(Prefabs.ModButtonPrefab.GetComponent<LanguageTextMeshController>());

            Prefabs.ModButtonPrefab.SetActive(false);
        }

        private void CreatePanel()
        {
            Prefabs.MOPanelPrefab.name = "SettingsSubPanel, Mod Options";

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

            Prefabs.MOCanvas.AddComponent<DescriptionController>();

            Prefabs.MOCanvas.name = "SettingsSubPanel, Mod Options";

            ModListPanel = GameObject.Instantiate(Prefabs.MOPanelPrefab, Prefabs.MOCanvas.transform);

            ModListPanel.GetComponent<RectTransform>().anchorMax = new Vector2(0.25f, 1f);

            ModListPanel.SetActive(true);

            ModListPanel.name = "Mod List Panel";


            GameObject CategoryHeader = GameObject.Instantiate(Prefabs.MOPanelPrefab, Prefabs.MOCanvas.transform);

            CategoryHeader.GetComponent<RectTransform>().anchorMin = new Vector2(0.275f, 0.86f);
            CategoryHeader.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);

            CategoryHeader.SetActive(true);

            CategoryHeader.name = "Category Headers";

            GameObject OptionsPanel = GameObject.Instantiate(Prefabs.MOPanelPrefab, Prefabs.MOCanvas.transform);

            OptionsPanel.GetComponent<RectTransform>().anchorMin = new Vector2(0.275f, 0);
            OptionsPanel.GetComponent<RectTransform>().anchorMax = new Vector2(0.625f, 0.82f);

            OptionsPanel.SetActive(true);

            OptionsPanel.name = "Options Panel";

            GameObject OptionDescriptionPanel = GameObject.Instantiate(Prefabs.MOPanelPrefab, Prefabs.MOCanvas.transform);

            OptionDescriptionPanel.GetComponent<RectTransform>().anchorMin = new Vector2(0.65f, 0);
            OptionDescriptionPanel.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0.82f);

            OptionDescriptionPanel.SetActive(true);

            OptionDescriptionPanel.name = "Option Description Panel";


            Transform ModListLayout = ModListPanel.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout");

            foreach (var Container in ModSettingsManager.optionContainers)
            {
                GameObject newModButton = GameObject.Instantiate(Prefabs.ModButtonPrefab, ModListLayout);

                LanguageAPI.Add($"{ModSettingsManager.StartingText}.{Container.ModGUID}.{Container.ModName}.ModListOption".ToUpper().Replace(" ", "_"), Container.ModName);

                //newModButton.GetComponent<LanguageTextMeshController>().token = $"{ModSettingsManager.StartingText}.{Container.ModGUID}.{Container.ModName}.ModListOption".ToUpper().Replace(" ", "_");
                newModButton.GetComponentInChildren<HGTextMeshProUGUI>().text = Container.ModName;
                newModButton.GetComponent<HGButton>().onClick.RemoveAllListeners();
                //newModButton.GetComponent<HGButton>().onClick.AddListener(delegate { loadModPanel(item); });
                //newModButton.GetComponent<HGButton>().hoverToken = Container.ModName;

                

                newModButton.SetActive(true);
            }
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
    }
}
