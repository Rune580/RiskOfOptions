using RiskOfOptions.Options;

namespace RiskOfOptions.Config.OptionProviders;

public class FloatStepSliderOptionProvider : ConfigItemOptionProvider
{
    public override IsValidRule[] Rules { get; } = [
        item => item is IConfigItem<float>,
        item => item.ValidationRules.HasRange,
        item => item.ValidationRules.HasStep
    ];

    public override BaseOption CreateOption(IConfigItem configItem) =>
        new StepSliderOption((IConfigItem<float>)configItem);
}