using R2API;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RiskOfOptions.Components.OptionComponents;
using RiskOfOptions.Components.Options;
using RiskOfOptions.Components.RuntimePrefabs;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

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
            
            RuntimePrefabManager.Register<ModOptionsPanelPrefab>();
        }
    }
}
