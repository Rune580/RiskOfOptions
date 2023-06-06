using RoR2.UI;
using UnityEngine;
using UnityEngine.UI;

namespace RiskOfOptions.Components.Options
{
    public class ModSettingsBool : ModSettingsControl<bool>
    {
        public Image checkBox;
        public Sprite checkBoxFalse;
        public Sprite checkBoxTrue;

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
            bool value = GetCurrentValue();
            value = !value;

            SubmitValue(value);
        }

        protected override void OnUpdateControls()
        {
            base.OnUpdateControls();

            if (!this)
                return;

            if (!checkBox)
                return;

            checkBox.sprite = IsChecked ? checkBoxTrue : checkBoxFalse;
        }
    }
}