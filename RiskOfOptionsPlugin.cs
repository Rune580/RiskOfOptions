using System;
using BepInEx;
using BepInEx.Configuration;
using R2API.Utils;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using UnityEngine;

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
            VERSION = "2.0.0";

        private ConfigEntry<KeyboardShortcut> testKeyboard;
        private ConfigEntry<bool> testBool;
        private ConfigEntry<float> testFloat;
        private ConfigEntry<float> testFloatStepped;
        
        private ConfigEntry<bool> bool1;
        private ConfigEntry<bool> bool2;
        private ConfigEntry<bool> bool3;
        private ConfigEntry<bool> bool4;
        private ConfigEntry<bool> bool5;
        private ConfigEntry<bool> bool6;

        private ConfigEntry<float> testFloatOverride;


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Awake is automatically called by Unity")]
        private void Awake()
        {
            Debug.Init(base.Logger);

            ModSettingsManager.Init();

            //ModSettingsManager.setPanelTitle("Example Title Bitch");

            //ModSettingsManager.SetPanelDescription("Example Description");
            
            testKeyboard = Config.Bind("Test BepInEx Config", "testKey", new KeyboardShortcut(KeyCode.G, KeyCode.LeftShift), "lig my balls");

            testBool = Config.Bind("Test BepInEx Config", "testCheckBox", true, "This check box was made from a config");

            testFloat = Config.Bind("Test BepInEx Config", "testSlider", 50f, "lig me dude");

            testFloatStepped = Config.Bind("Test BepInEx Config", "testSliderStepped", 1.5f, "lig me dude 2");

            testFloatOverride = Config.Bind("Test BepInEx Config", "testSliderOverridden", 50f, "lig me dude 3");

            bool1 = Config.Bind("Test 1", "bool1", false, "L");
            bool2 = Config.Bind("Test 2", "bool2", false, "L");
            bool3 = Config.Bind("Test 3", "bool3", false, "L");
            bool4 = Config.Bind("Test 4", "bool4", false, "L");
            bool5 = Config.Bind("Test 5", "bool5", false, "L");
            bool6 = Config.Bind("Test 6", "bool6", false, "L");

            testBool.SettingChanged += ConfigEntryBoolTest;
            
            testFloat.SettingChanged += ConfigEntryFloatTest;

            testFloatStepped.SettingChanged += ConfigEntryFloatTest;

            testKeyboard.SettingChanged += ConfigEntryKeyBindTest;
            
            ModSettingsManager.AddOption(new CheckBoxOption(testBool, new CheckBoxConfig()));
            ModSettingsManager.AddOption(new SliderOption(testFloat, new SliderConfig { checkIfDisabled = () => !testBool.Value }));
            ModSettingsManager.AddOption(new StepSliderOption(testFloatStepped, new StepSliderConfig { min = 0, max = 2, increment = 0.1f, checkIfDisabled = () => testBool.Value }));
            ModSettingsManager.AddOption(new KeyBindOption(testKeyboard, new KeyBindConfig()));
            
            ModSettingsManager.AddOption(new CheckBoxOption(bool1));
            ModSettingsManager.AddOption(new CheckBoxOption(bool2));
            ModSettingsManager.AddOption(new CheckBoxOption(bool3));
            ModSettingsManager.AddOption(new CheckBoxOption(bool4));
            ModSettingsManager.AddOption(new CheckBoxOption(bool5));
            ModSettingsManager.AddOption(new CheckBoxOption(bool6));
            
            ModSettingsManager.SetModDescription("Ligma dude balls");
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
            Debug.Log($"Config float event invoked args {e}");
        }
        
        private void OverrideTest(bool lig)
        {
            Debug.Log(lig);
        }

        private void ChoiceTest(int choice)
        {
            Debug.Log($"Choice set to index: {choice}");
        }

        private bool TestString(string input, out string message)
        {
            message = "liggy";
            Debug.Log(input);
            return true;
        }
    }
}
