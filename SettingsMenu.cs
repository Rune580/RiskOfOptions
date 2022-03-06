using R2API;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RiskOfOptions.Components.OptionComponents;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RiskOfOptions
{
    internal static class SettingsMenu
    {
        public static void Init()
        {
            FindPrefab();
            AddToPrefab();
        }

        private static void FindPrefab()
        {
            GameObject pauseMenuPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/UI/PauseScreen.prefab").WaitForCompletion();
            
            if (pauseMenuPrefab == null)
                throw new Exception("Couldn't initialize Risk Of Options! Continue at your own risk!");

            Prefabs.SettingsPanelPrefab = pauseMenuPrefab.GetComponentInChildren<PauseScreenController>().settingsPanelPrefab;
        }

        private static void AddToPrefab()
        {
            Prefabs.SettingsPanelPrefab.AddComponent<ModOptionPanelController>();
        }
    }
}
