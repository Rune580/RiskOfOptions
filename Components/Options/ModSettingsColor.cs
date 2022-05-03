using RiskOfOptions.Components.ColorPicker;
using RiskOfOptions.Utils;
using RoR2.UI;
using UnityEngine;
using UnityEngine.UI;

namespace RiskOfOptions.Components.Options
{
    public class ModSettingsColor : ModSettingsControl<Color>
    {
        public HGButton button;

        private ColorBlock _colorBlock;
            
        protected override void Awake()
        {
            base.Awake();

            if (option == null)
                return;

            _colorBlock = button.colors;
            _colorBlock.normalColor = GetCurrentValue();
            button.colors = _colorBlock;
        }

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

        protected override void OnUpdateControls()
        {
            base.OnUpdateControls();
            
            if (option == null)
                return;

            _colorBlock.normalColor = GetCurrentValue();
            button.colors = _colorBlock;
        }

        public void OpenColorPicker()
        {
            ColorPickerUtil.OpenColorPicker(SubmitValue, GetCurrentValue());
        }
    }
}