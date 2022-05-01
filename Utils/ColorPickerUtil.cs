using System;
using RiskOfOptions.Components.Options;
using RiskOfOptions.Resources;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RiskOfOptions.Utils
{
    public class ColorPickerUtil : MonoBehaviour
    {
        public RooColorHue hueController;
        public RooColorPicker colorPicker;
        
        private float _hue;
        private float _saturation;
        private float _value;

        public void Awake()
        {
            hueController.onHueChanged.AddListener(UpdateHue);
            colorPicker.onValueChanged.AddListener(UpdateSatAndVal);
        }

        private void UpdateHue(float hue)
        {
            _hue = hue;
            colorPicker.SetHue(hue);
        }

        private void UpdateSatAndVal(float saturation, float value)
        {
            _saturation = saturation;
            _value = value;
        }
        
        public static void Test()
        {
            Object.Instantiate(Prefabs.ColorPickerOverlay);
        }
    }
}