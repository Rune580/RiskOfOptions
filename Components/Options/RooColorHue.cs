using UnityEngine;
using UnityEngine.EventSystems;

namespace RiskOfOptions.Components.Options
{
    public class RooColorHue : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public RectTransform handle;

        public void OnPointerClick(PointerEventData eventData)
        {
            UpdateHue(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            UpdateHue(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            UpdateHue(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            UpdateHue(eventData);
        }

        private void UpdateHue(PointerEventData eventData)
        {
            Vector2 pos = GetLocalPos(eventData);
            
            var hue = GetHueFromMousePos(pos);
            
            Debug.Log($"Hue: {hue}");
        }
        
        private Vector2 GetLocalPos(PointerEventData eventData)
        {
            return transform.InverseTransformPoint(eventData.position);
        }

        private float GetHueFromMousePos(Vector2 mousePos)
        {
            float angle = GetMouseAngle(mousePos, new Vector2(0, 0));
            
            return angle;
        }
        
        private static float GetMouseAngle(Vector2 mousePos, Vector2 center) {
            return (Mathf.Atan2(mousePos.y - center.y, mousePos.x - center.x) + Mathf.PI * 2) % (Mathf.PI * 2);
        }
    }
}