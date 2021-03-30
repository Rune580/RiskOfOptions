using BepInEx;
using R2API.Utils;

namespace RiskOfOptions
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    [BepInDependency("com.bepis.r2api")]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    [R2APISubmoduleDependency("LanguageAPI")]
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

            ModSettingsManager.setPanelTitle("Example Title Bitch");

            ModSettingsManager.setPanelDescription("Example Description");

            ModSettingsManager.CreateCategory("Audio", "yeah");

            ModSettingsManager.CreateCategory("Controls", "yeah");

            ModSettingsManager.CreateCategory("Enemies", "yeah");

            ModSettingsManager.CreateCategory("Dick", "yeah");

            ModSettingsManager.CreateCategory("Balls", "yeah");

            ModSettingsManager.CreateCategory("Ligma", "yeah");

            ModSettingsManager.CreateCategory("Got em", "yeah");

            ModSettingsManager.CreateCategory("Etc", "yeah");


            ModSettingsManager.AddOption("Test CheckBox", "This is a Description", true);

            ModSettingsManager.AddOption("Test Slider", "This is another Description", 0f, "Audio");

            ModSettingsManager.AddOption("Test KeyBind", "This is yet another Description", UnityEngine.KeyCode.G, "Controls");



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
