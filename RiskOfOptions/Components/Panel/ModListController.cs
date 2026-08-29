using System.Collections.Generic;
using RiskOfOptions.Containers;
using RoR2;
using RoR2.UI;
using UnityEngine;

namespace RiskOfOptions.Components.Panel;

public class ModListController : MonoBehaviour
{
    public HGTextMeshProUGUI modDescriptionLabel = null!;
    public GameObject modListButtonPrefab = null!;
    public RectTransform verticalLayout = null!;

    private readonly List<ModListButton> _buttons = [];

    private void Awake()
    {
        CreateModList();
    }

    private void CreateModList()
    {
        foreach (var collection in ModSettingsManager.OptionCollection)
        {
            _buttons.Add(CreateModListButton(collection));
        }
    }

    private ModListButton CreateModListButton(OptionCollection collection)
    {
        var instance = Instantiate(modListButtonPrefab, verticalLayout, false);
        var modListButton = instance.GetComponent<ModListButton>();
        modListButton.SetMod(collection);
        modListButton.onSetModDescription += SetModDescription;

        return modListButton;
    }

    private void SetModDescription(string descriptionToken)
    {
        if (string.IsNullOrWhiteSpace(descriptionToken))
        {
            modDescriptionLabel.text = "";
            return;
        }
        
        var text = Language.currentLanguage.GetLocalizedStringByToken(descriptionToken);
        if (text == descriptionToken)
            text = "No description provided"; // TODO: Use language token instead!

        modDescriptionLabel.text = text;
    }
}