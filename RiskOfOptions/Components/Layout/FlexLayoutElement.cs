using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RiskOfOptions.Components.Layout
{
    [DisallowMultipleComponent]
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public abstract class FlexLayoutElement : UIBehaviour, ILayoutElement
    {
        public float minWidth { get; protected set; }
        public float preferredWidth { get; protected set; }
        public float flexibleWidth { get; protected set; }
        public float minHeight { get; protected set; }
        public float preferredHeight { get; protected set; }
        public float flexibleHeight { get; protected set; }
        public int layoutPriority { get; protected set; }
        public float WidthPercentage { get; protected set; }
        public float HeightPercentage { get; protected set; }
        public Vector2 PaddingPercentage { get; protected set; }

        public Vector2 padding;

        protected RectTransform rectTransform;
        protected RectTransform parent;

        public override void Awake()
        {
            base.Awake();

            if (!rectTransform)
                rectTransform = GetComponent<RectTransform>();

            if (!parent)
                parent = transform.parent as RectTransform;

            layoutPriority = 10;
        }

        public virtual void OnValidate()
        {
            if (!rectTransform)
                rectTransform = GetComponent<RectTransform>();
            
            if (!parent)
                parent = transform.parent as RectTransform;
            
            layoutPriority = 10;
            
            CalculateLayoutInputHorizontal();
            CalculateLayoutInputVertical();
        }
        
        public virtual void CalculateLayoutInputHorizontal() { }
        public virtual void CalculateLayoutInputVertical() { }
    }
}