using System.Collections.Generic;
using RiskOfOptions.Components.Gizmos;
using RiskOfOptions.Components.Recycle;
using RiskOfOptions.Containers;
using RiskOfOptions.Utils;
using RoR2;
using RoR2.UI;
using UnityEngine;

namespace RiskOfOptions.Components.Panel;

public class ModListController : RecycleListView<ModListButton, OptionCollection>
{
    public ModOptionsPanelController mainController = null!;
    public HGTextMeshProUGUI modDescriptionLabel = null!;
    
    protected override IList<OptionCollection> ListData => [.. ModSettingsManager.OptionCollection];

    private readonly List<(ModListButton, DrawRect)> _buttons = [];

    protected override void OnInstantiatePoolItem(ModListButton instance)
    {
        instance.onSetModDescription += SetModDescription;
        instance.onModSelected += mainController.ModSelected;
        
        _buttons.Add((instance, DrawRect.Create(Color.red, instance.RectTransform.GetWorldRect())));
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

    private void Update()
    {
        foreach (var (button, drawRect) in _buttons)
        {
            var rect = button.RectTransform.GetWorldRect();
            drawRect.rect = rect;
        }
    }
}