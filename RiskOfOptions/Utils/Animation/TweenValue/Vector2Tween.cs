using UnityEngine;
using UnityEngine.Events;

namespace RiskOfOptions.Utils.Animation.TweenValue;

internal struct Vector2Tween : ITweenValue
{
    public class Vector2TweenCallback : UnityEvent<Vector2>;

    private Vector2TweenCallback? _callback;
    
    public Vector2 StartValue { get; set; }
    
    public Vector2 TargetValue { get; set; }
    
    public bool IgnoreTimeScale { get; set; }
    
    public float Duration { get; set; }

    public void TweenValue(float floatPercentage)
    {
        if (!ValidTarget())
            return;

        var newValue = Vector2.Lerp(StartValue, TargetValue, floatPercentage);
        _callback?.Invoke(newValue);
    }

    public void AddOnChangedCallback(UnityAction<Vector2> callback)
    {
        _callback ??= new Vector2TweenCallback();
        _callback.AddListener(callback);
    }

    public bool ValidTarget() => _callback is not null;
}