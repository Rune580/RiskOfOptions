using BepInEx;

namespace RiskOfOptions
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public sealed class RiskOfOptionsPlugin : BaseUnityPlugin
    {
        public const string
            MODNAME = "Risk of Options",
            AUTHOR = "rune580",
            GUID = "com." + AUTHOR + "." + "riskofoptions",
            VERSION = "1.1.0";


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Awake is automatically called by Unity")]
        private void Awake()
        {
            ModSettingsManager.Init();


            ModOption test = new ModOption(ModOption.OptionType.Bool, "test", "test2", "0");

            OptionCategory category = new OptionCategory("com." + AUTHOR + "." + "riskofoptions");

            category.Add(ref test);

            test.name = "s";
        }
    }
}
