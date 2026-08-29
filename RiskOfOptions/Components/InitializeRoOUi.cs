using System.Linq;
using RiskOfOptions.Resources;
using RoR2.UI;
using UnityEngine;

namespace RiskOfOptions.Components;

public class InitializeRoOUi : MonoBehaviour
{
    private void Awake()
    {
        var panel = LoadModOptionsPanelPrefab();
        InitializeHeader(panel);
        
        DestroyImmediate(this);
    }

    private GameObject LoadModOptionsPanelPrefab()
    {
        var subPanelArea = transform.Find("SafeArea").Find("SubPanelArea");
        var panelInstance = Instantiate(Prefabs.mainPanel, subPanelArea, false);

        return panelInstance;
    }

    private void InitializeHeader(GameObject panel)
    {
        var headerArea = transform.Find("SafeArea/HeaderContainer/Header (JUICED)");
        var headerInstance = Instantiate(Prefabs.modOptionsHeaderButton, headerArea, false);
        var headerButton = headerInstance.GetComponent<HGButton>();
        
        var navigationController = GetComponent<HGHeaderNavigationController>();
        headerButton.onClick.AddListener(() =>
        {
            navigationController.ChooseHeaderByButton(headerButton);
        });
        
        var headers = navigationController.headers.ToList();
        var header = new HGHeaderNavigationController.Header
        {
            headerButton = headerButton,
            headerName = "Mod Options",
            tmpHeaderText = headerButton.GetComponentInChildren<HGTextMeshProUGUI>(),
            headerRoot = panel,
        };
        headers.Add(header);
        navigationController.headers = headers;
        
        headerArea.Find("GenericGlyph (Right)").SetAsLastSibling();
    }
}