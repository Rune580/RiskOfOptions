using RiskOfOptions.Options;
using UnityEngine;

namespace RiskOfOptions.Config.OptionProviders;

public class ColorPickerOptionProvider : ConfigItemOptionProvider
{
    public override IsValidRule[] Rules { get; } = [
        item => item is IConfigItem<Color>
    ];


    public override BaseOption CreateOption(IConfigItem configItem) => new ColorOption((IConfigItem<Color>)configItem);
}