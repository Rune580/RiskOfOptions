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
            
            FetchChildren(true);
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
            RecalculateElements();
            SetLayoutHorizontal();
            SetLayoutVertical();
            SetDirty();
        }

        public void SetLayoutHorizontal()
        {
            float offset = 0;
            foreach (var child in _childElements)
            {
                if (child is not HorizontalFlexLayoutElement element)
                    continue;
                
                var rectTransform = element.GetComponent<RectTransform>();

                rectTransform.drivenByObject = this;
                rectTransform.drivenProperties = DrivenTransformProperties.Anchors | DrivenTransformProperties.AnchoredPosition;
                
                var min = new Vector2(0, 0);
                var max = new Vector2(1, 1);

                min.x = offset + element.PaddingPercentage.x;
                max.x = element.fill ? 1 : offset + element.WidthPercentage + element.PaddingPercentage.x;

                rectTransform.anchorMin = min;
                rectTransform.anchorMax = max;

                offset = max.x + element.PaddingPercentage.y;
            }
        }

        public void SetLayoutVertical()
        {
            float offset = 0;
            foreach (var child in _childElements)
            {
                if (child is not VerticalFlexLayoutElement element)
                    continue;
                
                var rectTransform = element.GetComponent<RectTransform>();

                rectTransform.drivenByObject = this;
                rectTransform.drivenProperties = DrivenTransformProperties.Anchors | DrivenTransformProperties.AnchoredPosition;
                
                var min = new Vector2(0, 0);
                var max = new Vector2(1, 1);

                min.y = offset + element.PaddingPercentage.x;
                max.y = element.fill ? 1 : offset + element.HeightPercentage + element.PaddingPercentage.x;

                rectTransform.anchorMin = min;
                rectTransform.anchorMax = max;

                offset = max.y + element.PaddingPercentage.y;
            }
        }

        private void FetchChildren(bool force = false)
        {
            if (_childElements != null && _childElements.Length == transform.childCount && !force)
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

        public void RecalculateElements()
        {
            foreach (var element in _childElements)
            {
                element.CalculateLayoutInputHorizontal();
                element.CalculateLayoutInputVertical();
            }
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