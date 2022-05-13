using System;
using RoR2.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RiskOfOptions.Components.Panel
{
    public class ModListController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public float expandedSize = 325f;
        public float minimizedSize = 90f;

        public bool shouldMinimize = true;
        private bool minimized;
        private RectTransform _rectTransform;
        private ModOptionPanelController _mopc;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            if (!GetComponentInParent<ModOptionPanelController>().initialized)
            {
                return;
            }

            if (!_mopc)
                _mopc = GetComponentInParent<ModOptionPanelController>();

            HGHeaderNavigationController navigationController = GetComponent<HGHeaderNavigationController>();

            navigationController.headerHighlightObject.transform.SetParent(transform);
            navigationController.headerHighlightObject.SetActive(false);

            if (navigationController.currentHeaderIndex >= 0 && navigationController.headers != null)
            {
                navigationController.headers[navigationController.currentHeaderIndex].headerButton.interactable = true;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (shouldMinimize)
            {
                var sizeDelta = _rectTransform.sizeDelta;
                sizeDelta.x = expandedSize;
                _rectTransform.sizeDelta = sizeDelta;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (shouldMinimize)
            {
                var sizeDelta = _rectTransform.sizeDelta;
                sizeDelta.x = minimizedSize;
                _rectTransform.sizeDelta = sizeDelta;
            }
        }
    }
}
