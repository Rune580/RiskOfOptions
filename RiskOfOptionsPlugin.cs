using BepInEx;
using BepInEx.Configuration;
using R2API.Utils;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using UnityEngine;

namespace RiskOfOptions
{
    [BepInPlugin(Guid, ModName, Version)]
    [BepInDependency("com.bepis.r2api")]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    [R2APISubmoduleDependency("LanguageAPI")]
    public sealed class RiskOfOptionsPlugin : BaseUnityPlugin
    {
        private const string
            ModName = "Risk of Options",
            Author = "rune580",
            Guid = "com." + Author + "." + "riskofoptions",
            Version = "2.0.0";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Awake is automatically called by Unity")]
        private void Awake()
        {
            ModSettingsManager.Init();

            ConfigEntry<bool> testBool = Config.Bind("Test", "Description Test", false, "");

            string description = "";

            for (int i = 0; i < 100; i++)
                description += "The quick brown fox jumps over the lazy dog, some more random bullshit so this line becomes ridiculously long and stuff and things and stuff.\n";
            
            ModSettingsManager.AddOption(new CheckBoxOption(testBool, new CheckBoxConfig { description = description }));
            
            ConfigEntry<bool> testBool1 = Config.Bind("Test", "Restart Required 1", false, "test");
            ModSettingsManager.AddOption(new CheckBoxOption(testBool1, true));
            
            ConfigEntry<bool> testBool2 = Config.Bind("Test", "No Restart Required", false, "test");
            ModSettingsManager.AddOption(new CheckBoxOption(testBool2, false));
            
            ConfigEntry<bool> testBool3 = Config.Bind("Test", "Restart Required 2", false, "test");
            ModSettingsManager.AddOption(new CheckBoxOption(testBool3, true));
            
            ModSettingsManager.SetModDescription(description);
        }
    }
}