using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RiskOfOptions.Components.ColorPicker
{
    public class RooColorPicker : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public RectTransform handle;
        public Image background;
        public ColorPickerEvent onValueChanged = new();

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void SetHue(float hue)
        {
            background.color = Color.HSVToRGB(hue.Remap(0, 6.28f, 0, 1), 1, 1);
        }

        public void SetValues(float saturation, float value)
        {
            var rect = _rectTransform.rect;

            var x = saturation.Remap(0, 1, -(rect.width / 2), rect.width / 2);
            var y = value.Remap(0, 1, -(rect.height / 2), rect.height / 2);
            
            ClampHandle(new Vector2(x, y));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            UpdateValues(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            UpdateValues(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            UpdateValues(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            UpdateValues(eventData);
        }

        private void UpdateValues(PointerEventData eventData)
        {
            Vector2 pos = ClampHandle(transform.InverseTransformPoint(eventData.position)); ;

            var rect = _rectTransform.rect;
            var values = pos - rect.center;

            var saturation = values.x.Remap(-(rect.width / 2), rect.width / 2, 0, 1);
            var value = values.y.Remap(-(rect.height / 2), rect.height / 2, 0, 1);

            onValueChanged?.Invoke(saturation, value);
        }

        private Vector2 ClampHandle(Vector2 pos)
        {
            var rect = _rectTransform.rect;

            var x = Mathf.Clamp(pos.x, -(rect.width / 2), rect.width / 2);
            var y = Mathf.Clamp(pos.y, -(rect.height / 2), rect.height / 2);

            Vector2 clampedPos = new Vector2(x, y);

            handle.transform.localPosition = clampedPos;

            return clampedPos;
        }

        [Serializable]
        public class ColorPickerEvent : UnityEvent<float, float>
        {
            
        }
    }
}