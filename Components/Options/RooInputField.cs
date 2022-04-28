using System;
using RoR2.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RiskOfOptions.Components.Options
{
    public class RooInputField : TMP_InputField
    {
        public bool inField;
        
        private RectTransform _rectTransform;
        private RectTransform _textTransform;

        public override void Awake()
        {
            base.Awake();
            
            _rectTransform = GetComponent<RectTransform>();
            _textTransform = GetComponentInChildren<HGTextMeshProUGUI>().gameObject.GetComponent<RectTransform>();
        }

        private void Update()
        {
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, LayoutUtility.GetPreferredHeight(_textTransform));
            
            bool submitKey = Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter);
            bool validKey = Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Escape);

            if ((validKey && !inField) || submitKey)
                gameObject.SetActive(false);
        }

        public override void OnPointerEnter(PointerEventData pointerEventData)
        {
            inField = true;
            
            base.OnPointerEnter(pointerEventData);
        }

        public override void OnPointerExit(PointerEventData pointerEventData)
        {
            inField = false;

            base.OnPointerExit(pointerEventData);
        }
    }
}