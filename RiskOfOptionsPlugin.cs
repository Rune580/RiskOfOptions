using BepInEx;

namespace RiskOfOptions
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public sealed class RiskOfOptionsPlugin : BaseUnityPlugin
    {
        public const string
            MODNAME = "Risk of Options",
            AUTHOR = "rune580",
            GUID = "com." + AUTHOR + "." + "riskofoptions",
            VERSION = "1.0.4";


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Awake is automatically called by Unity")]
        private void Awake()
        {
            ModSettingsManager.Init();
        }
    }
}
