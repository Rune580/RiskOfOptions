using RiskOfOptions.Options;

namespace RiskOfOptions.Config.OptionProviders;

public class FloatSliderOptionProvider : ConfigItemOptionProvider
{
    public override IsValidRule[] Rules { get; } = [
        item => item is IConfigItem<float>,
        item => item.ValidationRules.HasRange
    ];

    public override BaseOption CreateOption(IConfigItem configItem) => new SliderOption((IConfigItem<float>)configItem);
}