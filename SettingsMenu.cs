using R2API;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            GameObject PauseMenuPrefab = Resources.Load<GameObject>("prefabs/ui/pausescreen");

            Prefabs.SettingsPanelPrefab = PauseMenuPrefab.GetComponentInChildren<PauseScreenController>().settingsPanelPrefab;

            if (Prefabs.SettingsPanelPrefab == null)
                throw new Exception("Couldn't initilize Risk Of Options! Continue at your own risk!");
        }

        private static void AddToPrefab()
        {
            Prefabs.SettingsPanelPrefab.AddComponent<ModOptionPanelController>();
        }
    }
}
