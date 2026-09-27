using System;
using RiskOfOptions.Components;
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
            
            settingsPanelTitle.AddComponent<InitializeRoOUi>();
            settingsPanel.AddComponent<InitializeRoOUi>();
            
            LanguageApi.Add(LanguageTokens.HeaderToken, "MOD OPTIONS");
        }
    }
}
