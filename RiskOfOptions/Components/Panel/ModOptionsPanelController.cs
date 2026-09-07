using System;
using UnityEngine;

namespace RiskOfOptions.Components.Panel;

public class ModOptionsPanelController : MonoBehaviour
{
    public ModOptionsSidebar sidebar = null!;
    public ModListController modListController = null!;

    private GameObject _genericDescriptionPanel = null!;

    private void Awake()
    {
        if (modListController)
            modListController.mainController = this;

        if (!_genericDescriptionPanel)
            _genericDescriptionPanel = transform.parent.Find("GenericDescriptionPanel").gameObject;
    }

    private void OnEnable()
    {
        _genericDescriptionPanel.SetActive(false);
        BackToModList();
    }

    private void OnDisable()
    {
        _genericDescriptionPanel.SetActive(true);
        BackToModList();
    }

    public void ModSelected(string modGuid, RectTransform buttonTransform)
    {
        Debug.Log($"Mod Selected: {modGuid}");
        
        if (string.IsNullOrWhiteSpace(modGuid))
            return;

        var mod = ModSettingsManager.OptionCollection[modGuid];
        
        Debug.Log($"Previous View: {sidebar.CurrentView.GetType()}");
        sidebar.CurrentView = ModOptionsSidebar.View.ModSelected(mod, buttonTransform);
        Debug.Log($"Current View: {sidebar.CurrentView.GetType()}");
    }

    public void BackToModList()
    {
        sidebar.CurrentView = ModOptionsSidebar.View.ModList();
    }
}