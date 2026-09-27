using RiskOfOptions.Options;

namespace RiskOfOptions.Config.OptionProviders;

public class IntFieldOptionProvider : ConfigItemOptionProvider
{
    public override IsValidRule[] Rules { get; } = [
        item => item is IConfigItem<int>
    ];

    public override BaseOption CreateOption(IConfigItem configItem) => new IntFieldOption((IConfigItem<int>)configItem);
}