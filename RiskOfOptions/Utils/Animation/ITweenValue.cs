namespace RiskOfOptions.Utils.Animation;

internal interface ITweenValue
{
    public bool IgnoreTimeScale { get; }
    
    public float Duration { get; }
    
    public void TweenValue(float percentage);

    public bool ValidTarget();
}