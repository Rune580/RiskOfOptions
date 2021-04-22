using System;
using BepInEx;
using BepInEx.Configuration;
using R2API.Utils;
using RiskOfOptions.OptionConstructors;
using RiskOfOptions.OptionOverrides;
using RiskOfOptions.Options;
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

        private ConfigEntry<KeyboardShortcut> testKeyboard;
        private ConfigEntry<bool> testBool;
        private ConfigEntry<float> testFloat;
        private ConfigEntry<float> testFloatStepped;

        private ConfigEntry<float> testFloatOverride;


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Awake is automatically called by Unity")]
        private void Awake()
        {
            Debug.Init(base.Logger);

            ModSettingsManager.Init();

            //ModSettingsManager.setPanelTitle("Example Title Bitch");

            ModSettingsManager.SetPanelDescription("Example Description");

            ModSettingsManager.CreateCategory("Test BepInEx Config");

            ModSettingsManager.CreateCategory("Test Sliders");

            ModSettingsManager.CreateCategory("Testing New System");

            ModSettingsManager.CreateCategory("Extra 1");
            ModSettingsManager.CreateCategory("Extra 2");
            ModSettingsManager.CreateCategory("Extra 3");
            ModSettingsManager.CreateCategory("Extra 4");
            ModSettingsManager.CreateCategory("Extra 5");

            ModSettingsManager.AddOption(new CheckBox(){ Name = "Test", CategoryName = "Testing New System", DefaultValue = true, Description = "Lig ball", OnValueChanged = DoVisibility, InvokeValueChangedEventOnStart = true });

            ModSettingsManager.AddOption(new KeyBind(){ Name = "Test KeyBind" , CategoryName = "Testing New System", DefaultValue = KeyCode.G, Description = "This is yet another Description", IsVisible = false });

            ModSettingsManager.AddOption(new Slider() { Name = "Music Slider", CategoryName = "Test Sliders", DefaultValue = 50f, Min = 10, Max = 69, Description = "This is another Description", DisplayAsPercentage = false});

            ModSettingsManager.AddOption(new StepSlider() { Name = "FOV Slider Test", CategoryName = "Test Sliders", DefaultValue = 90, Min = 50, Max = 110, Increment = 1f, Description = "FOV Test Slider Description" });
            ModSettingsManager.AddOption(new StepSlider() { Name = "Other Test Step Slider", CategoryName = "Test Sliders", DefaultValue = 1.5f, Min = 1, Max = 2, Increment = 0.05f, Description = "Test slider from 1 to 2 with increments of 0.05f" });
            ModSettingsManager.AddOption(new StepSlider() { Name = "More Visible Step Slider", CategoryName = "Test Sliders", DefaultValue = 60, Min = 0, Max = 200, Increment = 25, Description = "Test slider from 0 to 200 with increments of 20", DisplayAsPercentage = true});

            testKeyboard = Config.Bind("Test BepInEx Config", "testKey", new KeyboardShortcut(KeyCode.G), "lig my balls");

            testBool = Config.Bind("Test BepInEx Config", "testCheckBox", true, "This check box was made from a config");

            testFloat = Config.Bind("Test BepInEx Config", "testSlider", 50f, "lig me dude");

            testFloatStepped = Config.Bind("Test BepInEx Config", "testSliderStepped", 1.5f, "lig me dude 2");

            testFloatOverride = Config.Bind("Test BepInEx Config", "testSliderOverridden", 50f, "lig me dude 3");

            testBool.SettingChanged += ConfigEntryBoolTest;

            testKeyboard.SettingChanged += ConfigEntryKeyBindTest;

            ModSettingsManager.AddOption(new DropDown() { Name = "Test Drop Down", CategoryName = "Testing New System", DefaultValue = 0, Choices = new[] {"Choice 1", "Choice 2", "Choice 3", "Choice 4", "Choice 5", "Choice 6", "Choice 7", "Choice 8", "Choice 9", "Choice 10" }, Description = "Test Drop Down with 5 choices" , OnValueChanged = ChoiceTest });

            ModSettingsManager.AddOption(new CheckBox() { ConfigEntry = testBool });

            ModSettingsManager.AddOption(new KeyBind() { ConfigEntry = testKeyboard });

            ModSettingsManager.AddOption(new Slider() { ConfigEntry = testFloat, Min = 50, Max = 69 });

            ModSettingsManager.AddOption(new StepSlider() { ConfigEntry = testFloatStepped, Min = 1, Max = 2, Increment = 0.05f });

            ModSettingsManager.AddOption(new CheckBox() { Name = "Test Override", CategoryName = "Testing New System", DefaultValue = false, Description = "Lig ball" });

            CheckBoxOverride checkBoxOverride = new CheckBoxOverride()
            {
                Name = "Test Override",
                CategoryName = "Testing New System",
                OverrideOnTrue = true,
                ValueToReturnWhenOverriden = false
            };

            ModSettingsManager.AddOption(new CheckBox() { Name = "To Be Overridden", CategoryName = "Testing New System", DefaultValue = true, Description = "Lig ball but disabled", Override = checkBoxOverride , OnValueChanged = OverrideTest });


            SliderOverride musicOverride = new SliderOverride()
            {
                Name = "Test Override",
                CategoryName = "Testing New System",
                OverrideOnTrue = true,
                ValueToReturnWhenOverriden = 0f
            };


            ModSettingsManager.AddOption(new Slider() {ConfigEntry = testFloatOverride, Name = "Better Name", CategoryName = "Testing New System" , Override = musicOverride});

            //ModSettingsManager.AddSlider("Music Slider", "This is another Description", 50f, 10f, 69f, "Audio");

            //ModSettingsManager.AddKeyBind("Test KeyBind", "This is yet another Description", UnityEngine.KeyCode.G, "Controls", false);

            //ModSettingsManager.AddListener(new UnityAction<bool>(delegate(bool lig)
            //{
            //    ModSettingsManager.SetVisibility("Test KeyBind", "Controls", lig);
            //}), "Enable Test KeyBind", "Controls");


            //ModSettingsManager.AddCheckBox("Enable Enemy stuff", "This is a Description", false, "Enemies", true);
            //ModSettingsManager.AddCheckBox("Do something that doesn't need a restart", "This is a Description", false, "Enemies");
        }

        private void ConfigEntryKeyBindTest(object sender, EventArgs e)
        {
            Debug.Log($"Config keyboard event invoked args {e.ToString()}");
        }

        private void ConfigEntryBoolTest(object sender, EventArgs e)
        {
            Debug.Log($"Config bool event invoked args {e.ToString()}");
        }

        private void ConfigEntryFloatTest(object sender, EventArgs e)
        {
            Debug.Log($"Config bool event invoked args {e.ToString()}");
        }

        private void DoVisibility(bool lig)
        {
            ModSettingsManager.SetVisibility("Test KeyBind", "Testing New System", lig);
        }
        
        private void OverrideTest(bool lig)
        {
            Debug.Log(lig);
        }

        private void ChoiceTest(int choice)
        {
            Debug.Log($"Choice set to index: {choice}");
        }
    }
}
