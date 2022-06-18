using System;
using System.Globalization;
using RiskOfOptions.Components.ColorPicker;
using RiskOfOptions.Components.Options;
using RiskOfOptions.Lib;
using RiskOfOptions.Resources;
using RoR2;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RiskOfOptions.Utils
{
    public class ColorPickerUtil : MonoBehaviour
    {
        public RooColorHue hueController;
        public RooColorPicker colorPicker;
        public RgbaSliderController rgbaSliders;
        public Image preview;
        public TMP_InputField hexField;
        public string hexFormatString = "{0:X6}";

        private Action<Color> _onColorSelected;

        private float _hue;
        private float _saturation;
        private float _value;
        
        
        private int _hexValue;

        private Color _color = Color.white;

        public void Awake()
        {
            hueController.onHueChanged.AddListener(UpdateHue);
            colorPicker.onValueChanged.AddListener(UpdateSatAndVal);
            hexField.onSubmit.AddListener(HexFieldSubmit);
            hexField.onEndEdit.AddListener(HexFieldSubmit);
            rgbaSliders.onColorChanged.AddListener(SetColor);
        }

        public void SetColor(Color newColor)
        {
            _color = newColor;
            
            Color.RGBToHSV(_color, out var hue, out _saturation, out _value);
            _hue = hue.Remap(0, 1, 0, 6.28f);
            
            _hexValue = _color.ToRGBHex();
            UpdateControls();
            UpdateHandles();
        }

        private void UpdateHue(float hue)
        {
            _hue = hue;
            
            colorPicker.SetHue(_hue);
            UpdateColorFromHSV();
        }

        private void UpdateSatAndVal(float saturation, float value)
        {
            _saturation = saturation;
            _value = value;
            
            UpdateColorFromHSV();
        }

        private void HexFieldSubmit(string text)
        {
            if (int.TryParse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int hex))
                _hexValue = hex;
            
            SetColor(_color.FromRGBHex(_hexValue));
        }

        private void UpdateColorFromHSV()
        {
            //var color = Color.HSVToRGB(_hue.Remap(0, 6.28f, 0, 1), _saturation, _value);
            var color = ColorExtensions.ColorFromHSV(_hue, _saturation, _value);
            color.a = _color.a;
            _color = color;
            
            _hexValue = _color.ToRGBHex();

            rgbaSliders.inPicker = true;
            UpdateControls();
            rgbaSliders.inPicker = false;
        }

        private void UpdateHandles()
        {
            hueController.SetHue(_hue);
            
            colorPicker.SetHue(_hue);
            if (_color != Color.black)
                colorPicker.SetValues(_saturation, _value);
        }

        private void UpdateControls()
        {
            preview.color = _color;
            
            hexField.text = string.Format(CultureInfo.InvariantCulture, hexFormatString, _hexValue);

            rgbaSliders.SetSliders(_color);
        }

        public void Accept()
        {
            _onColorSelected.Invoke(_color);
            Close();
        }

        public void Close()
        {
            RoR2Application.unscaledTimeTimers.CreateTimer(0.1f, () => DestroyPicker(gameObject));
        }

        private static void DestroyPicker(GameObject gameObject)
        {
            DestroyImmediate(gameObject);
        }
        
        [Serializable]
        public class ColorChangedEvent : UnityEvent<Color> { }

        public static void OpenColorPicker(Action<Color> onColorSelected, Color currentColor)
        {
            var overlay = Instantiate(Prefabs.colorPickerOverlay);

            var controller = overlay.GetComponent<ColorPickerUtil>();
            controller._onColorSelected = onColorSelected;
            controller.SetColor(currentColor);
        }
    }
}