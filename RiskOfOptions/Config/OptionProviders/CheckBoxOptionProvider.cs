using RiskOfOptions.Options;

namespace RiskOfOptions.Config.OptionProviders;

public class CheckBoxOptionProvider : ConfigItemOptionProvider
{
    public override IsValidRule[] Rules { get; } = [
        item => item is IConfigItem<bool>
    ];

    public override BaseOption CreateOption(IConfigItem configItem) =>
        new CheckBoxOption((IConfigItem<bool>)configItem);
}