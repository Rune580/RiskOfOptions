using System;
using System.Globalization;
using RiskOfOptions.Options;
using RoR2.UI;

namespace RiskOfOptions.Components.Options
{
    public class DropDownController : ModSettingsControl<object>
    {
        public RooDropdown dropdown;
        
        protected new void Awake()
        {
            dropdown = GetComponentInChildren<RooDropdown>();

            base.Awake();

            dropdown.OnValueChanged.AddListener(OnChoiceChanged);

            nameLabel.token = nameToken;
        }

        protected override void Disable()
        {
            foreach (var button in GetComponentsInChildren<HGButton>())
                button.interactable = false;
            
            GetComponentInChildren<RooDropdown>().interactable = false;
        }

        protected override void Enable()
        {
            foreach (var button in GetComponentsInChildren<HGButton>())
                button.interactable = true;
            
            GetComponentInChildren<RooDropdown>().interactable = true;
        }

        protected new void OnEnable()
        {
            base.OnEnable();
            GenerateChoices();
        }

        private void GenerateChoices()
        {
            dropdown.choices = ((ChoiceOption)option).GetNameTokens();

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
}