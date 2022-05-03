using System.Collections.Generic;
using System.Reflection;
using RiskOfOptions.Containers;
using RiskOfOptions.Lib;
using RiskOfOptions.Options;
using UnityEngine;

using static RiskOfOptions.ExtensionMethods;
using ConCommandArgs = RoR2.ConCommandArgs;
using PauseManager = On.RoR2.PauseManager;

#pragma warning disable 618

namespace RiskOfOptions
{
    public static class ModSettingsManager
    {
        internal static readonly ModIndexedOptionCollection OptionCollection = new();

        internal const string StartingText = "RISK_OF_OPTIONS";
        internal const int StartingTextLength = 15;

        internal static bool disablePause = false;
        
        internal static readonly List<string> RestartRequiredOptions = new();
        

        internal static void Init()
        {
            LanguageApi.Init();
            
            Resources.Assets.LoadAssets();
            Resources.Prefabs.Init();

            LanguageTokens.Register();
            
            SettingsModifier.Init();

            PauseManager.CCTogglePause += PauseManagerOnCCTogglePause;
        }

        private static void PauseManagerOnCCTogglePause(PauseManager.orig_CCTogglePause orig, ConCommandArgs args)
        {
            if (disablePause)
                return;

            orig(args);
        }

        public static void SetModDescription(string description)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetModInfo();
            
            SetModDescription(description, modInfo.ModGuid, modInfo.ModName);
        }

        public static void SetModDescription(string description, string modGuid, string modName)
        {
            EnsureContainerExists(modGuid, modName);
            
            OptionCollection[modGuid].description = description;
        }

        public static void SetModIcon(Sprite iconSprite)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetModInfo();

            SetModIcon(iconSprite, modInfo.ModGuid, modInfo.ModName);
        }
        
        public static void SetModIcon(Sprite iconSprite, string modGuid, string modName)
        {
            EnsureContainerExists(modGuid, modName);

            OptionCollection[modGuid].icon = iconSprite;
        }

        public static void AddOption(BaseOption option)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetModInfo();
            
            AddOption(option, modInfo.ModGuid, modInfo.ModName);
        }

        public static void AddOption(BaseOption option, string modGuid, string modName)
        {
            option.SetProperties();

            option.ModGuid = modGuid;
            option.ModName = modName;
            option.Identifier = $"{modGuid}.{option.Category}.{option.Name}.{option.OptionTypeName}".Replace(" ", "_").ToUpper();
            
            option.RegisterTokens();
            
            OptionCollection.AddOption(ref option);
        }

        private static void EnsureContainerExists(string modGuid, string modName)
        {
            if (!OptionCollection.ContainsModGuid(modGuid))
                OptionCollection[modGuid] = new OptionCollection(modName, modGuid);
        }
    }
}