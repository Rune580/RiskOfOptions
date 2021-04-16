using R2API;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RiskOfOptions.OptionComponents;
using UnityEngine;

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
            GameObject pauseMenuPrefab = Resources.Load<GameObject>("prefabs/ui/pausescreen");

            Prefabs.SettingsPanelPrefab = pauseMenuPrefab.GetComponentInChildren<PauseScreenController>().settingsPanelPrefab;

            if (Prefabs.SettingsPanelPrefab == null)
                throw new Exception("Couldn't initilize Risk Of Options! Continue at your own risk!");
        }

        private static void AddToPrefab()
        {
            Prefabs.SettingsPanelPrefab.AddComponent<ModOptionPanelController>();
        }
    }
}
