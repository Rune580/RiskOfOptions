using System;
using UnityEngine;
using UnityEngine.Events;

namespace RiskOfOptions.Components.ColorPicker
{
    public class RgbaSliderController : MonoBehaviour
    {
        public RooSliderInput r;
        public RooSliderInput g;
        public RooSliderInput b;
        public RooSliderInput a;

        public ColorEvent onColorChanged = new();
        public bool inPicker = false;

        private Color _rgba;

        public Color Rgba
        {
            get => _rgba;
            set
            {
                if (_rgba == value)
                    return;
                
                _rgba = value;

                if (inPicker)
                    return;
                
                onColorChanged.Invoke(_rgba);
            }
        }

        private void Awake()
        {
            r.onValueChanged.AddListener(SetRed);
            g.onValueChanged.AddListener(SetGreen);
            b.onValueChanged.AddListener(SetBlue);
            a.onValueChanged.AddListener(SetAlpha);
        }

        public void SetSliders(Color color)
        {
            r.Value = color.r;
            g.Value = color.g;
            b.Value = color.b;
            a.Value = color.a;
        }

        public void SetRgba(Color color)
        {
            Rgba = color;

            r.Value = Rgba.r;
            g.Value = Rgba.g;
            b.Value = Rgba.b;
            a.Value = Rgba.a;
        }

        public void SetRed(float rValue)
        {
            var newRgba = Rgba;
            newRgba.r = rValue;
            Rgba = newRgba;
        }
        
        public void SetGreen(float gValue)
        {
            var newRgba = Rgba;
            newRgba.g = gValue;
            Rgba = newRgba;
        }
        
        public void SetBlue(float bValue)
        {
            var newRgba = Rgba;
            newRgba.b = bValue;
            Rgba = newRgba;
        }
        
        public void SetAlpha(float aValue)
        {
            var newRgba = Rgba;
            newRgba.a = aValue;
            Rgba = newRgba;
        }

        [Serializable]
        public class ColorEvent : UnityEvent<Color>
        {
            
        }
    }
}