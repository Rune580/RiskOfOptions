namespace RiskOfOptions.Config;

public class ConfigItemValueValidationRules<TValue> : ConfigItemValueValidationRules
{
    public TValue? MinValue { get; set; }
    public TValue? MaxValue { get; set; }
    public TValue? Step { get; set; }
    
    /// <param name="min">Inclusive min</param>
    public ConfigItemValueValidationRules<TValue> SetValueMin(TValue min)
    {
        MinValue = min;
        return this;
    }
    
    /// <param name="min">Inclusive min</param>
    public ConfigItemValueValidationRules<TValue> SetValueMin(object min)
    {
        MinValue = (TValue?)min;
        return this;
    }
    
    /// <param name="max">Inclusive max</param>
    public ConfigItemValueValidationRules<TValue> SetValueMax(TValue max)
    {
        MaxValue = max;
        return this;
    }
    
    /// <param name="max">Inclusive max</param>
    public ConfigItemValueValidationRules<TValue> SetValueMax(object max)
    {
        MaxValue = (TValue?)max;
        return this;
    }
    
    /// <param name="min">Inclusive min</param>
    /// <param name="max">Inclusive max</param>
    public ConfigItemValueValidationRules<TValue> SetValueRange(TValue min, TValue max)
    {
        MinValue = min;
        MaxValue = max;
        return this;
    }
    
    public ConfigItemValueValidationRules<TValue> SetValueRange(object min, object max)
    {
        MinValue = (TValue?)min;
        MaxValue = (TValue?)max;
        return this;
    }

    public ConfigItemValueValidationRules<TValue> SetValueStep(TValue step)
    {
        Step = step;
        return this;
    }

    public override bool HasRange => MinValue is not null && MaxValue is not null;
    
    public override bool HasStep => Step is not null;
}

public abstract class ConfigItemValueValidationRules
{
    public abstract bool HasRange { get; }
    
    public abstract bool HasStep { get; }
}
