using UnityEngine;

namespace RiskOfOptions.Components.Layout
{
    [DisallowMultipleComponent]
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class VerticalFlexLayoutElement : FlexLayoutElement
    {
        private const int Scale = 1080;

        public bool fill;
        public bool byPercent;
        
        [Range(0, 1)] public float desiredHeightPercentage;
        public float desiredHeight;
        
        public override void CalculateLayoutInputVertical()
        {
            var parentSize = parent.rect.size;

            if (byPercent)
            {
                HeightPercentage = desiredHeightPercentage;
                var val = parentSize.y * desiredHeightPercentage;
                minHeight = val;
                preferredHeight = val;
                flexibleHeight = 0;

                PaddingPercentage = padding;
            }
            else
            {
                var ratio = Scale / parentSize.y;

                HeightPercentage = (desiredHeight / Scale) * ratio;
                minHeight = desiredHeight;
                preferredHeight = desiredHeight;
                flexibleHeight = 0;

                PaddingPercentage = new Vector2(padding.x / Scale, padding.y / Scale);
            }
        }
    }
}