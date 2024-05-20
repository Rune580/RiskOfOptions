namespace RiskOfOptions.OptionConfigs;

public class IntFieldConfig : NumericFieldConfig<int>
{
    public override int Min { get; set; } = int.MinValue;
    public override int Max { get; set; } = int.MaxValue;
}