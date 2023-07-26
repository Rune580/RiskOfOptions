using System;
using System.Collections.Generic;
using System.Reflection;
using MonoMod.RuntimeDetour;
using RiskOfOptions.Containers;
using RiskOfOptions.Lib;
using RiskOfOptions.Options;
using RoR2;
using UnityEngine;

using static RiskOfOptions.ExtensionMethods;
using ConCommandArgs = RoR2.ConCommandArgs;
#pragma warning disable 618

namespace RiskOfOptions
{
    public static class ModSettingsManager
    {
        private static Hook _pauseHook;
        
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
            // CursorController.Init();

            var targetMethod = typeof(PauseManager).GetMethod("CCTogglePause", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            var destMethod = typeof(ModSettingsManager).GetMethod(nameof(PauseManagerOnCCTogglePause), BindingFlags.NonPublic | BindingFlags.Static);
            _pauseHook = new Hook(targetMethod, destMethod);
        }

        private static void PauseManagerOnCCTogglePause(Action<ConCommandArgs> orig, ConCommandArgs args)
        {
            if (disablePause)
                return;

            orig(args);
        }

        public static void SetModDescription(string description)
        {
            ModMetaData modMetaData = Assembly.GetCallingAssembly().GetModMetaData();
            
            SetModDescription(description, modMetaData.Guid, modMetaData.Name);
        }

        public static void SetModDescription(string description, string modGuid, string modName)
        {
            EnsureContainerExists(modGuid, modName);
            
            OptionCollection[modGuid].SetDescriptionText(description);
        }
        
        public static void SetModDescriptionToken(string descriptionToken)
        {
            ModMetaData modMetaData = Assembly.GetCallingAssembly().GetModMetaData();
            
            SetModDescriptionToken(descriptionToken, modMetaData.Guid, modMetaData.Name);
        }

        public static void SetModDescriptionToken(string descriptionToken, string modGuid, string modName)
        {
            EnsureContainerExists(modGuid, modName);

            OptionCollection[modGuid].DescriptionToken = descriptionToken;
        }

        public static void SetModIcon(Sprite iconSprite)
        {
            ModMetaData modMetaData = Assembly.GetCallingAssembly().GetModMetaData();

            SetModIcon(iconSprite, modMetaData.Guid, modMetaData.Name);
        }
        
        public static void SetModIcon(Sprite iconSprite, string modGuid, string modName)
        {
            EnsureContainerExists(modGuid, modName);

            OptionCollection[modGuid].icon = iconSprite;
        }

        public static void AddOption(BaseOption option)
        {
            ModMetaData modMetaData = Assembly.GetCallingAssembly().GetModMetaData();
            
            AddOption(option, modMetaData.Guid, modMetaData.Name);
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