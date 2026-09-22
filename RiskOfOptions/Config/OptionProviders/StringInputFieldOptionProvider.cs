using RiskOfOptions.Options;

namespace RiskOfOptions.Config.OptionProviders;

public class StringInputFieldOptionProvider : ConfigItemOptionProvider
{
    public override IsValidRule[] Rules { get; } = [
        item => item is IConfigItem<string>
    ];


    public override BaseOption CreateOption(IConfigItem configItem) =>
        new StringInputFieldOption((IConfigItem<string>)configItem);
}