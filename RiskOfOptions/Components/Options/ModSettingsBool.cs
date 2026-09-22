using RoR2.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace RiskOfOptions.Components.Options;

public class ModSettingsBool : ModSettingsControl<bool>
{
    public GameObject? checkBoxFalse;
    public GameObject? checkBoxTrue;

    public bool IsChecked => GetCurrentValue();

    protected override void Disable()
    {
        foreach (var button in GetComponentsInChildren<HGButton>())
            button.interactable = false;
    }

    protected override void Enable()
    {
        foreach (var button in GetComponentsInChildren<HGButton>())
            button.interactable = true;
    }

    public void Toggle()
    {
        var value = GetCurrentValue();
        value = !value;

        SubmitValue(value);
    }

    protected override void OnUpdateControls()
    {
        base.OnUpdateControls();

        if (!this)
            return;

        if (!checkBoxFalse || !checkBoxTrue)
            return;

        checkBoxTrue!.SetActive(IsChecked);
        checkBoxFalse!.SetActive(!IsChecked);
    }
}