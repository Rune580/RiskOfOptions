using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RiskOfOptions.Components.Layout
{
    [DisallowMultipleComponent]
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class FlexLayoutElement : UIBehaviour, ILayoutElement
    {
        public float minWidth { get; private set; }
        public float preferredWidth { get; private set; }
        public float flexibleWidth { get; private set; }
        public float minHeight { get; private set; }
        public float preferredHeight { get; private set; }
        public float flexibleHeight { get; private set; }
        public int layoutPriority { get; private set; }

        public Vector2 padding;

        [Range(0, 1)] public float widthPercentage;
        [Range(0, 1)] public float heightPercentage;

        private RectTransform _rectTransform;
        private RectTransform _parent;

        public override void Awake()
        {
            base.Awake();

            if (!_rectTransform)
                _rectTransform = GetComponent<RectTransform>();

            if (!_parent)
                _parent = transform.parent as RectTransform;

            layoutPriority = 10;
        }

        private void OnValidate()
        {
            if (!_rectTransform)
                _rectTransform = GetComponent<RectTransform>();
            
            if (!_parent)
                _parent = transform.parent as RectTransform;
            
            layoutPriority = 10;
        }

        public void CalculateLayoutInputHorizontal()
        {
            var parentSize = _parent.GetParentSize();
            var width = parentSize.x * widthPercentage;

            minWidth = width;
            preferredWidth = width;
            flexibleWidth = 0;
        }

        public void CalculateLayoutInputVertical()
        {
            var parentSize = _parent.GetParentSize();
            var height = parentSize.y * heightPercentage;

            minHeight = height;
            preferredHeight = height;
            flexibleHeight = 0;
        }
    }
}