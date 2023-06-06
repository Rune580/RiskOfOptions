using UnityEngine;

namespace RiskOfOptions.Components.Layout
{
    [DisallowMultipleComponent]
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class HorizontalFlexLayoutElement : FlexLayoutElement
    {
        private const int Scale = 1920;

        public bool fill;
        public bool byPercent;
        
        [Range(0, 1)] public float desiredWidthPercentage;
        public float desiredWidth;
        
        public override void CalculateLayoutInputHorizontal()
        {
            var parentSize = parent.rect.size;

            if (byPercent)
            {
                WidthPercentage = desiredWidthPercentage;
                var val = parentSize.x * desiredWidthPercentage;
                minWidth = val;
                preferredWidth = val;
                flexibleWidth = 0;

                PaddingPercentage = padding;
            }
            else
            {
                var ratio = Scale / parentSize.x;

                WidthPercentage = (desiredWidth / Scale) * ratio;
                minWidth = desiredWidth;
                preferredWidth = desiredWidth;
                flexibleWidth = 0;

                PaddingPercentage = new Vector2(padding.x / Scale, padding.y / Scale);
            }
        }
    }
}