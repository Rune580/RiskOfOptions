using BepInEx;
using UnityEngine;

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
            VERSION = "1.0.0";


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Awake is automatically called by Unity")]
        private void Awake()
        {
            ModSettingsManager.Init();

            ModSettingsManager.setPanelDescription("Testing stuff");

            ModSettingsManager.setPanelTitle("Risk of Options Testing Stuff");

            ModSettingsManager.addOption(new ModOption(ModOption.OptionType.Slider, "Test Slider", "This is a Slider test."));

            ModSettingsManager.addListener(ModSettingsManager.getOption("Test Slider"), new UnityEngine.Events.UnityAction<float>(testerino));

            ModSettingsManager.addOption(new ModOption(ModOption.OptionType.Bool, "Test Bool", "This is a Bool test."));

            ModSettingsManager.addListener(ModSettingsManager.getOption("Test Bool"), new UnityEngine.Events.UnityAction<bool>(test2));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Start is automatically called by Unity")]
        private void Start() //Called at the first frame of the game.
        {
            
        }

        public void testerino(float dum)
        {
            //Debug.Log($"{dum}");
        }

        public void test2(bool free)
        {
            //Debug.Log($"{free}");
        }
    }
}
