using R2API;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RiskOfOptions.Components.OptionComponents;
using RiskOfOptions.Components.Options;
using RiskOfOptions.Components.Panel;
using RiskOfOptions.Components.RuntimePrefabs;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace RiskOfOptions
{
    internal static class SettingsModifier
    {
        internal const string HeaderToken = "RISK_OF_OPTIONS_MOD_OPTIONS_HEADER_BUTTON_TEXT";
        
        public static void Init()
        {
            GameObject pauseMenuPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/UI/PauseScreen.prefab").WaitForCompletion();
            
            if (pauseMenuPrefab == null)
                throw new Exception("Couldn't initialize Risk Of Options! Continue at your own risk!");

            pauseMenuPrefab.GetComponentInChildren<PauseScreenController>().settingsPanelPrefab
                .AddComponent<ModOptionPanelController>();
            
            LanguageAPI.Add(HeaderToken, "MOD OPTIONS");
            
            RuntimePrefabManager.Register<ModOptionsPanelPrefab>();
            RuntimePrefabManager.Register<CheckBoxPrefab>();
            RuntimePrefabManager.Register<SliderPrefab>();
            RuntimePrefabManager.Register<StepSliderPrefab>();
            RuntimePrefabManager.Register<KeyBindPrefab>();
        }
    }
}
