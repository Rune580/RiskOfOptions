using BepInEx.Configuration;
using RiskOfOptions.Options;

namespace RiskOfOptions.Config.OptionProviders;

public class KeyBindOptionProvider : ConfigItemOptionProvider
{
    public override IsValidRule[] Rules { get; } = [
        item => item is IConfigItem<KeyboardShortcut>
    ];

    public override BaseOption CreateOption(IConfigItem configItem) =>
        new KeyBindOption((IConfigItem<KeyboardShortcut>)configItem);
}