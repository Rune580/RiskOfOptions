using BepInEx;
using R2API.Utils;
using RiskOfOptions.OptionOverrides;

namespace RiskOfOptions
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    [BepInDependency("com.bepis.r2api")]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    [R2APISubmoduleDependency("LanguageAPI", "ResourcesAPI")]
    public sealed class RiskOfOptionsPlugin : BaseUnityPlugin
    {
        public const string
            MODNAME = "Risk of Options",
            AUTHOR = "rune580",
            GUID = "com." + AUTHOR + "." + "riskofoptions",
            VERSION = "2.0.0"; // Yes this update is big enough that I feel it deserves a major version change.


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Awake is automatically called by Unity")]
        private void Awake()
        {
            Debug.Init(base.Logger);

            ModSettingsManager.Init();

            //ModSettingsManager.setPanelTitle("Example Title Bitch");

            //ModSettingsManager.setPanelDescription("Example Description");

            //ModSettingsManager.CreateCategory("Audio");

            //ModSettingsManager.CreateCategory("Controls");

            //ModSettingsManager.CreateCategory("Enemies");

            //ModSettingsManager.CreateCategory("Dick", "yeah");

            //ModSettingsManager.CreateCategory("Balls", "yeah");

            //ModSettingsManager.CreateCategory("Ligma", "yeah");

            //ModSettingsManager.CreateCategory("Got em", "yeah");

            //ModSettingsManager.CreateCategory("Etc", "yeah");

            //CheckBoxOverride checkBoxOverride = new CheckBoxOverride()
            //{
            //    Name = "Enable Music",
            //    CategoryName = "Audio",
            //    OverrideOnTrue = false,
            //    ValueToReturnWhenOverriden = false
            //};

            //SliderOverride musicOverride = new SliderOverride()
            //{
            //    Name = "Enable Music",
            //    CategoryName = "Audio",
            //    OverrideOnTrue = false,
            //    ValueToReturnWhenOverriden = 0f
            //};


            //ModSettingsManager.AddCheckBox("Enable Music", "This is a Description", true, "Audio");

            //ModSettingsManager.AddCheckBox("Test CheckBox", "fuck me dude", true, "Audio", checkBoxOverride);

            //ModSettingsManager.AddSlider("Music Slider", "This is another Description", 50f, "Audio", musicOverride);

            //ModSettingsManager.AddKeyBind("Test KeyBind", "This is yet another Description", UnityEngine.KeyCode.G, "Controls");

            //ModSettingsManager.AddOption("Emote Wheel", "Coming up with all of these examples is getting hard.", UnityEngine.KeyCode.C, "Controls");



            //ModOption test = new ModOption(ModOption.OptionType.Bool, "test", "test description", "0");

            //ModSettingsManager.addOption(test);

            //OptionCategory category = new OptionCategory("com." + AUTHOR + "." + "riskofoptions");

            //category.Add(ref test);

            //category.debugOptions();

            //test.Name = "s";

            //category.debugOptions();

            //test.Name = "d";

            //category.debugOptions();

            //ModSettingsManager.setSubCategory(ModSettingsManager.getCategory("ligballs"), "ligballs but sub");
        }
    }
}
