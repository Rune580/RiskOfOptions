using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace RiskOfOptions.Components.OptionComponents
{
    public class RooInputFieldOverlay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private RooOverlayPointerEnterEvent _onPointerEnterEvent;
        private RooOverlayPointerExitEvent _onPointerExitEvent;
        private RooOverlayPointerDownEvent _onPointerDownEvent;
        private RooOverlayDragBeginEvent _onDragBeginEvent;
        private RooOverlayDragEvent _onDragEvent;
        private RooOverlayEndDragEvent _onEndDragEvent;

        public RooOverlayPointerEnterEvent OnPointerEnterCanvas => _onPointerEnterEvent ??= new RooOverlayPointerEnterEvent();
        public RooOverlayPointerExitEvent OnPointerExitCanvas => _onPointerExitEvent ??= new RooOverlayPointerExitEvent();
        public RooOverlayPointerDownEvent OnPointerDownCanvas => _onPointerDownEvent ??= new RooOverlayPointerDownEvent();
        public RooOverlayDragBeginEvent OnBeginDragCanvas => _onDragBeginEvent ??= new RooOverlayDragBeginEvent();
        public RooOverlayDragEvent OnDragCanvas => _onDragEvent ??= new RooOverlayDragEvent();
        public RooOverlayEndDragEvent OnEndDragCanvas => _onEndDragEvent ??= new RooOverlayEndDragEvent();



        public void OnPointerEnter(PointerEventData eventData)
        {
            OnPointerEnterCanvas.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnPointerExitCanvas.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownCanvas.Invoke(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnBeginDragCanvas.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnDragCanvas.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnEndDragCanvas.Invoke(eventData);
        }

        public class RooOverlayPointerEnterEvent : UnityEvent
        {

        }

        public class RooOverlayPointerExitEvent : UnityEvent
        {

        }

        public class RooOverlayPointerDownEvent : UnityEvent<PointerEventData>
        {

        }

        public class RooOverlayDragBeginEvent : UnityEvent<PointerEventData>
        {

        }

        public class RooOverlayDragEvent : UnityEvent<PointerEventData>
        {

        }

        public class RooOverlayEndDragEvent : UnityEvent<PointerEventData>
        {

        }
    }
}