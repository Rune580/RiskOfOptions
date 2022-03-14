using BepInEx;
using BepInEx.Configuration;
using R2API.Utils;

namespace RiskOfOptions
{
    [BepInPlugin(Guid, ModName, Version)]
    [BepInDependency("com.bepis.r2api")]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    [R2APISubmoduleDependency("LanguageAPI")]
    public sealed class RiskOfOptionsPlugin : BaseUnityPlugin
    {
        private const string
            ModName = "Risk Of Options",
            Author = "rune580",
            Guid = "com." + Author + "." + "riskofoptions",
            Version = "2.3.0";

        internal static ConfigEntry<bool> SeenNoMods;
        internal static ConfigEntry<bool> SeenMods;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Awake is automatically called by Unity")]
        private void Awake()
        {
            SeenNoMods = Config.Bind("One Time Stuff", "Has seen the no mods prompt", false);
            SeenMods = Config.Bind("One Time Stuff", "Has seen the mods prompt", false);
            
            ModSettingsManager.Init();
        }
    }
}