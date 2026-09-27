using RiskOfOptions.Options;

namespace RiskOfOptions.Config.OptionProviders;

public class IntSliderOptionProvider : ConfigItemOptionProvider
{
    public override IsValidRule[] Rules { get; } = [
        item => item is IConfigItem<int>,
        item => item.ValidationRules.HasRange
    ];

    public override BaseOption CreateOption(IConfigItem configItem) =>
        new IntSliderOption((IConfigItem<int>)configItem);
}