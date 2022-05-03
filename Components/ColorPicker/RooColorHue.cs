using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace RiskOfOptions.Components.Options
{
    public class RooColorHue : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public RectTransform handle;
        public float radius = 170f;
        
        public HueEvent onHueChanged = new();

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

        public void SetHue(float hue)
        {
            SetHandleByHue(hue);
        }

        private void UpdateHue(PointerEventData eventData)
        {
            Vector2 pos = GetLocalPos(eventData);
            
            var hue = GetHueFromMousePos(pos);
            
            SetHandleByHue(hue);

            onHueChanged?.Invoke(hue);
        }

        private void SetHandleByHue(float hue)
        {
            float x = Mathf.Cos(hue) * radius;
            float y = Mathf.Sin(hue) * radius;

            handle.localPosition = new Vector3(x, y, 0);
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

        [Serializable]
        public class HueEvent : UnityEvent<float>
        {
            
        }
    }
}