namespace RiskOfOptions.OptionConfigs;

public class StepSliderConfig : SliderConfig
{
    public float increment = 1;

    public override string FormatString { get; set; } = "{0}";
}