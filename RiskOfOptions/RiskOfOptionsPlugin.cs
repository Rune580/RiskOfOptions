using System.Security;
using System.Security.Permissions;
using BepInEx;
using BepInEx.Configuration;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using RiskOfOptions.Resources;

#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete
[module: UnverifiableCode]

namespace RiskOfOptions;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public sealed class RiskOfOptionsPlugin : BaseUnityPlugin
{
    internal static ConfigEntry<bool>? seenNoMods;
    internal static ConfigEntry<bool>? seenMods;

    public static ConfigEntry<DecimalSeparator>? decimalSeparator;
    public static ConfigEntry<bool>? showModifiedIndicator;
    public static ConfigEntry<UnityEngine.Color>? nonDefaultModifiedColor;
    public static ConfigEntry<UnityEngine.Color>? hasChangedModifiedColor;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Awake is automatically called by Unity")]
    private void Awake()
    {
        const string ONE_TIME_STUFF = "One Time Stuff";
        seenNoMods = Config.Bind(ONE_TIME_STUFF, "Has seen the no mods prompt", false);
        seenMods = Config.Bind(ONE_TIME_STUFF, "Has seen the mods prompt", false);

        const string DISPLAY = "Display";
        decimalSeparator = Config.Bind(DISPLAY, "DecimalSeparator", DecimalSeparator.Period, "Changes how numbers are displayed across RoO.\nPeriod: 1,000.00\nComma: 1.000,00");
        showModifiedIndicator = Config.Bind(DISPLAY, "Show modified indicator", true, "Show a colored marker on the left of mod options that have been modified");
        nonDefaultModifiedColor = Config.Bind(DISPLAY, "Modified color (non-default)", new UnityEngine.Color(122f/255, 176f/255, 255f/255), "Color of the modified indicator when a mod option has a value different from its default");
        hasChangedModifiedColor = Config.Bind(DISPLAY, "Modified color (revertable)", new UnityEngine.Color(255f/255, 230f/255, 73f/255), "Color of the modified indicator when a mod option has changed but is still able to be reverted");

        ModSettingsManager.Init();
        
        ModSettingsManager.SetModIcon(Prefabs.animatedIcon);
        ModSettingsManager.AddOption(new ChoiceOption(decimalSeparator));
        ModSettingsManager.AddOption(new CheckBoxOption(showModifiedIndicator));
        ColorOptionConfig modifiedIndicatorDependent = new ColorOptionConfig { checkIfDisabled = DisabledModifiedIndicator };
        ModSettingsManager.AddOption(new ColorOption(nonDefaultModifiedColor, modifiedIndicatorDependent));
        ModSettingsManager.AddOption(new ColorOption(hasChangedModifiedColor, modifiedIndicatorDependent));
    }

    private bool DisabledModifiedIndicator() => !showModifiedIndicator!.Value;
}