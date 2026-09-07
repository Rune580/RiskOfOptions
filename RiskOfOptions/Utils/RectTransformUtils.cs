using UnityEngine;

namespace RiskOfOptions.Utils;

internal static class RectTransformUtils
{
    private static readonly Vector3[] Corners = new Vector3[4];
    
    extension(RectTransform t)
    {
        public Rect GetWorldRect()
        {
            t.GetWorldCorners(Corners);
            
            var min = Corners[0];
            var max = Corners[2];
            
            var width = max.x - min.x;
            var height = max.y - min.y;

            return new Rect(min.x, min.y, width, height);
        }

        public Rect GetChildInHierarchyRect(RectTransform child)
        {
            var rootRect = t.GetWorldRect();
            var childRect = child.GetWorldRect();

            var rootPos = rootRect.min;
            var childPos = childRect.min;

            return new Rect(childPos - rootPos, childRect.size);
        }
    }
}