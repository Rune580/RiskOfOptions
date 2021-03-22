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
            CreatePrefabs();
            AddToPrefab();
        }

        private static void CreatePrefabs()
        {
            GameObject PauseMenuPrefab = Resources.Load<GameObject>("prefabs/ui/pausescreen");

            Prefabs.SettingsPanelPrefab = PauseMenuPrefab.GetComponentInChildren<PauseScreenController>().settingsPanelPrefab;

            if (Prefabs.SettingsPanelPrefab == null)
                throw new Exception("Couldn't initilize Risk Of Options! Continue at your own risk!");


            Transform SubPanelArea = Prefabs.SettingsPanelPrefab.transform.Find("SafeArea").Find("SubPanelArea");
            Transform HeaderArea = Prefabs.SettingsPanelPrefab.transform.Find("SafeArea").Find("HeaderContainer").Find("Header (JUICED)");

            GameObject audioPanel = SubPanelArea.Find("SettingsSubPanel, Audio").gameObject;

            Prefabs.MOPanelPrefab = GameObject.Instantiate(audioPanel);
            Prefabs.MOPanelPrefab.name = "SettingsSubPanel, Mod Options";

            Prefabs.MOHeaderButtonPrefab = GameObject.Instantiate(HeaderArea.Find("GenericHeaderButton (Audio)").gameObject);
        }

        private static void AddToPrefab()
        {
            Prefabs.SettingsPanelPrefab.AddComponent<ModOptionPanelController>();

            //Prefabs.SettingsPanelPrefab.transform.Find("SafeArea").Find("SubPanelArea").gameObject.AddComponent<DescriptionController>();
        }
    }
}
