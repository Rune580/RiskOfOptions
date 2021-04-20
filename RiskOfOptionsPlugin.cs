using BepInEx;
using R2API.Utils;
using RiskOfOptions.OptionOverrides;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

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



            ModSettingsManager.SetPanelDescription("Example Description");

            ModSettingsManager.CreateCategory("Test Sliders");

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

            //ModSettingsManager.AddCheckBox("Enable Test KeyBind", "This is a Description", false, "Controls");
            //ModSettingsManager.AddCheckBox("Test CheckBox", "fuck me dude", true, "Audio", checkBoxOverride);

            //ModSettingsManager.AddSlider("Music Slider", "This is another Description", 50f, 10f, 69f, "Audio");

            ModSettingsManager.AddStepSlider("FOV Slider Test", "FOV Test Slider Description", 90, 50, 110, 1f, "Test Sliders");

            ModSettingsManager.AddStepSlider("Other Test Step Slider", "Test slider from 1 to 2 with increments of 0.05f", 1.5f, 1, 2, 0.05f, "Test Sliders");

            ModSettingsManager.AddStepSlider("More Visible Step Slider", "Test slider from 0 to 200 with increments of 20", 60, 0, 200, 20, "Test Sliders");

            //ModSettingsManager.AddKeyBind("Test KeyBind", "This is yet another Description", UnityEngine.KeyCode.G, "Controls", false);

            //ModSettingsManager.AddListener(new UnityAction<bool>(delegate(bool lig)
            //{
            //    ModSettingsManager.SetVisibility("Test KeyBind", "Controls", lig);
            //}), "Enable Test KeyBind", "Controls");


            //ModSettingsManager.AddCheckBox("Enable Enemy stuff", "This is a Description", false, "Enemies", true);
            //ModSettingsManager.AddCheckBox("Do something that doesn't need a restart", "This is a Description", false, "Enemies");


            //TMP_Asset asset = ScriptableObject.CreateInstance<TMP_Asset>();

            //asset.


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
