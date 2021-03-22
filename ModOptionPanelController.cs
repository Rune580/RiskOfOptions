using R2API;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RiskOfOptions
{
    public class ModOptionPanelController : MonoBehaviour
    {
        public void Start()
        {
            CreatePrefabs();
            CreatePanel();
            AddModOptionStuff();
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

            GameObject ModListPanel = GameObject.Instantiate(Prefabs.MOPanelPrefab, Prefabs.MOCanvas.transform);

            ModListPanel.GetComponent<RectTransform>().anchorMax = new Vector2(0.25f, 1f);

            ModListPanel.SetActive(true);


            GameObject CategoryHeader = GameObject.Instantiate(Prefabs.MOPanelPrefab, Prefabs.MOCanvas.transform);

            CategoryHeader.GetComponent<RectTransform>().anchorMin = new Vector2(0.28f, 0.86f);
            CategoryHeader.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);

            CategoryHeader.SetActive(true);
        }

        private void CreatePrefabs()
        {
            Transform SubPanelArea = transform.Find("SafeArea").Find("SubPanelArea");
            Transform HeaderArea = transform.Find("SafeArea").Find("HeaderContainer").Find("Header (JUICED)");

            GameObject audioPanel = SubPanelArea.Find("SettingsSubPanel, Audio").gameObject;

            Prefabs.MOPanelPrefab = GameObject.Instantiate(audioPanel);
            Prefabs.MOPanelPrefab.name = "SettingsSubPanel, Mod Options";

            Prefabs.MOHeaderButtonPrefab = GameObject.Instantiate(HeaderArea.Find("GenericHeaderButton (Audio)").gameObject);
        }

        private void AddModOptionStuff()
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
