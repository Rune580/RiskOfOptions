using RiskOfOptions.Options;

namespace RiskOfOptions.Config.OptionProviders;

public class EnumDropDownOptionProvider : ConfigItemOptionProvider
{
    public override IsValidRule[] Rules { get; } = [
        item => item.ValueType.IsEnum
    ];

    public override BaseOption CreateOption(IConfigItem configItem) =>
        new ChoiceOption((IConfigItem<object>)configItem);
}