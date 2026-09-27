using System;
using RiskOfOptions.Options;
using RoR2.UI;

namespace RiskOfOptions.Components.Options;

public class ModSettingsEnumDropDown : ModSettingsControl<object>
{
    public RooDropdown dropdown;
    
    protected override void Awake()
    {
        if (!dropdown)
            dropdown = GetComponentInChildren<RooDropdown>();
        
        base.Awake();
        
        dropdown.OnValueChanged.AddListener(OnChoiceChanged);
        
        nameLabel.token = nameToken;
    }
    
    protected override void Disable()
    {
        foreach (var button in GetComponentsInChildren<HGButton>())
            button.interactable = false;
            
        GetComponentInChildren<RooDropdown>().Interactable = false;
    }

    protected override void Enable()
    {
        foreach (var button in GetComponentsInChildren<HGButton>())
            button.interactable = true;
            
        GetComponentInChildren<RooDropdown>().Interactable = true;
    }

    protected new void OnEnable()
    {
        base.OnEnable();
        GenerateChoices();
    }

    private void GenerateChoices()
    {
        if (!dropdown || option is not ChoiceOption choiceOption || choiceOption.GetNameTokens().Length == 0)
            return;
        
        dropdown.choices = choiceOption.GetNameTokens();

        UpdateControls();
    }
    
    private void OnChoiceChanged(int newValue)
    {
        SubmitValue((Enum) Enum.Parse(GetCurrentValue().GetType(), $"{newValue}")); // this is cursed
    }

    protected override void OnUpdateControls()
    {
        base.OnUpdateControls();
            
        int currentIndex = Convert.ToInt32(GetCurrentValue());

        dropdown.SetChoice(currentIndex);
    }
}