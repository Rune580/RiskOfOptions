using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RiskOfOptions.Components.Layout
{
    [DisallowMultipleComponent]
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class FlexLayoutGroup : UIBehaviour, ILayoutGroup
    {
        public float spacing;
        
        private RectTransform _rectTransform;
        private FlexLayoutElement[] _childElements;

        public FlexLayoutElement[] Elements => _childElements;

        public override void Awake()
        {
            base.Awake();

            if (!_rectTransform)
                _rectTransform = GetComponent<RectTransform>();
            
            FetchChildren();
            SetDirty();
        }

        private void OnValidate()
        {
            if (!_rectTransform)
                _rectTransform = GetComponent<RectTransform>();
            
            FetchChildren();
            SetDirty();
        }

        private void OnTransformChildrenChanged()
        {
            FetchChildren();
            SetDirty();
        }

        public void SetLayoutHorizontal()
        {
            var spacingPercentage = spacing / _rectTransform.rect.size.x / 2f;
            
            float offset = 0;

            for (var i = 0; i < _childElements.Length; i++)
            {
                var element = _childElements[i];
                
                var rectTransform = element.GetComponent<RectTransform>();

                rectTransform.drivenByObject = this;
                var min = rectTransform.anchorMin;
                var max = rectTransform.anchorMax;

                min.x = offset;
                max.x = offset + element.widthPercentage;

                rectTransform.anchorMin = min;
                rectTransform.anchorMax = max;

                offset = max.x;
            }
        }

        public void SetLayoutVertical()
        {
            // Todo
        }

        private void FetchChildren()
        {
            if (_childElements != null && _childElements.Length == transform.childCount)
                return;
            
            var elements = new List<FlexLayoutElement>();

            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i).GetComponent<FlexLayoutElement>();

                if (child)
                    elements.Add(child);
            }

            _childElements = elements.ToArray();
        }

        public void SetDirty()
        {
            if (!IsActive())
                return;

            if (CanvasUpdateRegistry.IsRebuildingLayout())
            {
                StartCoroutine(DelayedSetDirty(_rectTransform));
                return;
            }
            
            LayoutRebuilder.MarkLayoutForRebuild(_rectTransform);
        }

        private IEnumerator DelayedSetDirty(RectTransform rectTransform)
        {
            yield return null;
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }
    }
}