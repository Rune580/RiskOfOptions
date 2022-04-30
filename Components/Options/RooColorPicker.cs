using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace RiskOfOptions.Components.Options
{
    public class RooColorPicker : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public RectTransform handle;

        public UnityEvent<float, float> onValueChanged;

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
            Vector2 localPos = transform.InverseTransformPoint(eventData.position);
            var values = GetPos(eventData);

            handle.localPosition = new Vector3(localPos.x, localPos.y, 0);

            onValueChanged?.Invoke(values.x, values.y);
        }

        private Vector2 GetPos(PointerEventData eventData)
        {
            Vector2 localPos = transform.InverseTransformPoint(eventData.position);

            RectTransform rectTransform = GetComponent<RectTransform>();

            return localPos - rectTransform.rect.center;
        }
    }
}