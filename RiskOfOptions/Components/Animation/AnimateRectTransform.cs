using RiskOfOptions.Utils.Animation;
using RiskOfOptions.Utils.Animation.TweenValue;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RiskOfOptions.Components.Animation;

[RequireComponent(typeof(RectTransform))]
public class AnimateRectTransform : UIBehaviour
{
    private readonly TweenRunner<Vector2Tween> _anchoredPositionRunner = new();
    private readonly TweenRunner<Vector2Tween> _sizeDeltaRunner = new();

    public float duration = 0.2f;
    public bool animateAnchoredPosition;
    public bool animateSizeDelta;
    
    public RectTransform RectTransform => (RectTransform)transform;

    public AnimateRectTransform()
    {
        _anchoredPositionRunner.Init(this);
        _sizeDeltaRunner.Init(this);
    }

    public void Animate(Rect startRect, Rect targetRect)
    {
        if (animateAnchoredPosition)
        {
            var tween = new Vector2Tween
            {
                Duration = duration,
                IgnoreTimeScale = true,
                StartValue = startRect.min,
                TargetValue = targetRect.min
            };
            tween.AddOnChangedCallback(AnchoredPositionChanged);
            _anchoredPositionRunner.StartTween(tween);
        }

        if (animateSizeDelta)
        {
            var tween = new Vector2Tween
            {
                Duration = duration,
                IgnoreTimeScale = true,
                StartValue = startRect.size,
                TargetValue = targetRect.size
            };
            tween.AddOnChangedCallback(SizeDeltaChanged);
            _sizeDeltaRunner.StartTween(tween);
        }
    }

    private void AnchoredPositionChanged(Vector2 newPos)
    {
        Debug.Log($"Anchored Position Updated: {RectTransform.anchoredPosition} -> {newPos}");
        RectTransform.anchoredPosition = newPos;
    }
    
    private void SizeDeltaChanged(Vector2 newSize)
    {
        RectTransform.sizeDelta = newSize;
    }
}