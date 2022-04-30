using UnityEngine;

namespace RiskOfOptions.Components.Options
{
    public class ModSettingsColor : ModSettingsControl<Color>
    {
        public RooColorHue hueWheel;
        public RooColorPicker colorPicker;

        private float _hue;
        private float _saturation;
        private float _value;

        protected override void Awake()
        {
            base.Awake();

            if (option == null)
                return;
            
            hueWheel.onHueChanged.AddListener(UpdateHue);
            colorPicker.onValueChanged.AddListener(UpdateSatAndVal);
        }

        protected override void Disable()
        {
            // Todo
        }

        protected override void Enable()
        {
            // Todo
        }

        private void UpdateHue(float hue)
        {
            _hue = hue;
        }

        private void UpdateSatAndVal(float saturation, float value)
        {
            _saturation = saturation;
            _value = value;
        }
    }
}