using RoR2.UI;
using System;
using RiskOfOptions.Components.Panel;
using RiskOfOptions.Components.RuntimePrefabs;
using RiskOfOptions.Lib;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RiskOfOptions
{
    internal static class SettingsModifier
    {
        public static void Init()
        {
            var settingsPanelTitle = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/UI/SettingsPanelTitle.prefab").WaitForCompletion();
            var settingsPanel = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/UI/SettingsPanel.prefab").WaitForCompletion();
            
            if (settingsPanelTitle == null || settingsPanel == null)
                throw new Exception("Couldn't initialize Risk Of Options! Continue at your own risk!");

            settingsPanelTitle.AddComponent<ModOptionPanelController>();
            settingsPanel.AddComponent<ModOptionPanelController>();
            
            LanguageApi.Add(LanguageTokens.HeaderToken, "MOD OPTIONS");
            
            RuntimePrefabManager.Register<ModOptionsPanelPrefab>();
            RuntimePrefabManager.Register<CheckBoxPrefab>();
            RuntimePrefabManager.Register<SliderPrefab>();
            RuntimePrefabManager.Register<StepSliderPrefab>();
            RuntimePrefabManager.Register<IntSliderPrefab>();
            RuntimePrefabManager.Register<FloatFieldPrefab>();
            RuntimePrefabManager.Register<IntFieldPrefab>();
            RuntimePrefabManager.Register<KeyBindPrefab>();
            RuntimePrefabManager.Register<InputFieldPrefab>();
            RuntimePrefabManager.Register<ChoicePrefab>();
            RuntimePrefabManager.Register<GenericButtonPrefab>();
        }
    }
}
