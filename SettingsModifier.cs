using R2API;
using RoR2.UI;
using System;
using RiskOfOptions.Components.Panel;
using RiskOfOptions.Components.RuntimePrefabs;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RiskOfOptions
{
    internal static class SettingsModifier
    {
        public static void Init()
        {
            GameObject pauseMenuPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/UI/PauseScreen.prefab").WaitForCompletion();
            
            if (pauseMenuPrefab == null)
                throw new Exception("Couldn't initialize Risk Of Options! Continue at your own risk!");

            pauseMenuPrefab.GetComponentInChildren<PauseScreenController>().settingsPanelPrefab
                .AddComponent<ModOptionPanelController>();
            
            LanguageAPI.Add(LanguageTokens.HeaderToken, "MOD OPTIONS");
            
            RuntimePrefabManager.Register<ModOptionsPanelPrefab>();
            RuntimePrefabManager.Register<CheckBoxPrefab>();
            RuntimePrefabManager.Register<SliderPrefab>();
            RuntimePrefabManager.Register<StepSliderPrefab>();
            RuntimePrefabManager.Register<KeyBindPrefab>();
            RuntimePrefabManager.Register<InputFieldPrefab>();
        }
    }
}
