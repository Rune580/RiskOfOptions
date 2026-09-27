using RiskOfOptions.Options;

namespace RiskOfOptions.Config.OptionProviders;

public class FloatFieldOptionProvider : ConfigItemOptionProvider
{
    public override IsValidRule[] Rules { get; } = [
        item => item is IConfigItem<float>
    ];

    public override BaseOption CreateOption(IConfigItem configItem) =>
        new FloatFieldOption((IConfigItem<float>)configItem);
}