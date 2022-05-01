using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RiskOfOptions.Components.Options
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
            background.material.SetFloat("_Hue", hue);
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

            var values = pos - _rectTransform.rect.center;

            onValueChanged?.Invoke(values.x, values.y);
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