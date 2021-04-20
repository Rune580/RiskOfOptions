using BepInEx;
using R2API.Utils;
using RiskOfOptions.OptionConstructors;
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

            ModSettingsManager.CreateCategory("Testing new System");

            ModSettingsManager.AddOption(new CheckBox(){ Name = "Test", CategoryName = "Testing new System", DefaultValue = true, Description = "Lig ball", OnValueChanged = DoVisibility, InvokeValueChangedEventOnStart = true});

            ModSettingsManager.AddOption(new KeyBind(){ Name = "Test KeyBind" , CategoryName = "Testing new System", DefaultValue = KeyCode.G, Description = "This is yet another Description", IsVisible = false });

            ModSettingsManager.AddOption(new Slider() { Name = "Music Slider", CategoryName = "Test Sliders", DefaultValue = 50f, Min = 10, Max = 69, Description = "This is another Description", DisplayAsPercentage = false});

            ModSettingsManager.AddOption(new StepSlider() { Name = "FOV Slider Test", CategoryName = "Test Sliders", DefaultValue = 90, Min = 50, Max = 110, Increment = 1f, Description = "FOV Test Slider Description" });
            ModSettingsManager.AddOption(new StepSlider() { Name = "Other Test Step Slider", CategoryName = "Test Sliders", DefaultValue = 1.5f, Min = 1, Max = 2, Increment = 0.05f, Description = "Test slider from 1 to 2 with increments of 0.05f" });
            ModSettingsManager.AddOption(new StepSlider() { Name = "More Visible Step Slider", CategoryName = "Test Sliders", DefaultValue = 60, Min = 0, Max = 200, Increment = 25, Description = "Test slider from 0 to 200 with increments of 20", DisplayAsPercentage = true});


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

            //ModSettingsManager.AddSlider("Music Slider", "This is another Description", 50f, 10f, 69f, "Audio");

            //ModSettingsManager.AddKeyBind("Test KeyBind", "This is yet another Description", UnityEngine.KeyCode.G, "Controls", false);

            //ModSettingsManager.AddListener(new UnityAction<bool>(delegate(bool lig)
            //{
            //    ModSettingsManager.SetVisibility("Test KeyBind", "Controls", lig);
            //}), "Enable Test KeyBind", "Controls");


            //ModSettingsManager.AddCheckBox("Enable Enemy stuff", "This is a Description", false, "Enemies", true);
            //ModSettingsManager.AddCheckBox("Do something that doesn't need a restart", "This is a Description", false, "Enemies");
        }

        private void DoVisibility(bool lig)
        {
            ModSettingsManager.SetVisibility("Test KeyBind", "Testing new System", lig);
        }
    }
}
