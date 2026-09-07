using System.Collections;
using UnityEngine;

namespace RiskOfOptions.Utils.Animation;

internal class TweenRunner<T> where T : struct, ITweenValue
{
    private MonoBehaviour _coroutineContainer = null!;

    private IEnumerator? _tween;

    private static IEnumerator Start(T tweenValue)
    {
        if (!tweenValue.ValidTarget())
            yield break;

        var elapsedTime = 0f;
        while (elapsedTime < tweenValue.Duration)
        {
            elapsedTime += tweenValue.IgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;

            var percentage = Mathf.Clamp01(elapsedTime / tweenValue.Duration);
            tweenValue.TweenValue(percentage);
            yield return null;
        }
        
        tweenValue.TweenValue(1f);
    }

    public void Init(MonoBehaviour coroutineContainer)
    {
        _coroutineContainer = coroutineContainer;
    }

    public void StartTween(T tweenValue)
    {
        if (!_coroutineContainer)
            return;

        if (_tween is not null)
        {
            _coroutineContainer.StopCoroutine(_tween);
            _tween = null;
        }

        if (!_coroutineContainer.gameObject.activeInHierarchy)
        {
            tweenValue.TweenValue(1f);
            return;
        }

        _tween = Start(tweenValue);
        _coroutineContainer.StartCoroutine(_tween);
    }
    
    public void StopTween()
    {
        if (_tween is null || !_coroutineContainer)
            return;
        
        _coroutineContainer.StopCoroutine(_tween);
        _tween = null;
    }
}