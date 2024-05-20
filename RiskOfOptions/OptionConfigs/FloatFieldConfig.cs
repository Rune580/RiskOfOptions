namespace RiskOfOptions.OptionConfigs;

public class FloatFieldConfig : NumericFieldConfig<float>
{
    public override float Min { get; set; } = float.MinValue;
    public override float Max { get; set; } = float.MaxValue;
}