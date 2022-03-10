using BepInEx;
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
            ModName = "Risk of Options",
            Author = "rune580",
            Guid = "com." + Author + "." + "riskofoptions",
            Version = "2.0.0";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Awake is automatically called by Unity")]
        private void Awake()
        {
            Debug.Init(Logger);

            ModSettingsManager.Init();
        }
    }
}
