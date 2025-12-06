#if DEBUG
using BepInEx;
using BepInEx.Configuration;
using RiskOfOptions.Options;

namespace RiskOfOptions.Test
{
    [BepInDependency(RiskOfOptions.PluginInfo.PLUGIN_GUID)]
    [BepInPlugin(GUID, NAME, VERSION)]
    internal sealed class TestPlugin : BaseUnityPlugin
    {
        public const string GUID = "com.rune580.riskofoptions.test";
        public const string NAME = "RiskOfOptions.Test";
        public const string VERSION = "0.0.0";

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public static new Config Config { get; private set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        private void Awake()
        {
            Config = new Config(base.Config);

            ModSettingsManager.AddOption(new CheckBoxOption(Config.checkBox));
            ModSettingsManager.AddOption(new SliderOption(Config.slider, new OptionConfigs.SliderConfig { min = -50, max = 50 }));
            ModSettingsManager.AddOption(new StepSliderOption(Config.stepSlider, new OptionConfigs.StepSliderConfig { min = -50, max = 50, increment = 0.5f }));
            ModSettingsManager.AddOption(new StepSliderOption(Config.stepSliderFree, new OptionConfigs.StepSliderConfig { min = -50, max = 50, remapManualInputToStep = false, increment = 0.5f }));
            ModSettingsManager.AddOption(new IntSliderOption(Config.intSlider, new OptionConfigs.IntSliderConfig { min = -50, max = 50 }));
            ModSettingsManager.AddOption(new FloatFieldOption(Config.floatField));
            ModSettingsManager.AddOption(new IntFieldOption(Config.intField));
            ModSettingsManager.AddOption(new KeyBindOption(Config.keyBind));
            ModSettingsManager.AddOption(new StringInputFieldOption(Config.inputField));
            ModSettingsManager.AddOption(new ChoiceOption(Config.dropDown));
            ModSettingsManager.AddOption(new ColorOption(Config.colorPicker));
        }
    }

    public sealed class Config
    {
        private readonly ConfigFile file;

        internal readonly ConfigEntry<bool> checkBox;
        internal readonly ConfigEntry<float> slider;
        internal readonly ConfigEntry<float> stepSlider;
        internal readonly ConfigEntry<float> stepSliderFree;
        internal readonly ConfigEntry<int> intSlider;
        internal readonly ConfigEntry<float> floatField;
        internal readonly ConfigEntry<int> intField;
        internal readonly ConfigEntry<KeyboardShortcut> keyBind;
        internal readonly ConfigEntry<string> inputField;
        internal readonly ConfigEntry<UnityEngine.KeyCode> dropDown;
        internal readonly ConfigEntry<UnityEngine.Color> colorPicker;

        internal Config(ConfigFile config)
        {
            file = config;

            const string Section = "_";
            checkBox = config.Bind(Section, nameof(checkBox), false, "");
            slider = config.Bind(Section, nameof(slider), 1.28f, "");
            stepSlider = config.Bind(Section, nameof(stepSlider), 1.28f, "Increment = 0.5f");
            stepSliderFree = config.Bind(Section, nameof(stepSliderFree), 1.28f, "Increment = 0.5f");
            intSlider = config.Bind(Section, nameof(intSlider), 2, "");
            floatField = config.Bind(Section, nameof(floatField), 1.28f);
            intField = config.Bind(Section, nameof(intField), 2);
            keyBind = config.Bind(Section, nameof(keyBind), KeyboardShortcut.Empty, "");
            inputField = config.Bind(Section, nameof(inputField), "a", "");
            dropDown = config.Bind(Section, nameof(dropDown), UnityEngine.KeyCode.Space, "");
            colorPicker = config.Bind(Section, nameof(colorPicker), UnityEngine.Color.green, "");
        }
    }
}
#endif
