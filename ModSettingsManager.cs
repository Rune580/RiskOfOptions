using System.Reflection;
using R2API;
using RiskOfOptions.Components.Panel;
using RiskOfOptions.Containers;
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
        
        internal static readonly string StartingText = "risk_of_options";

        internal static bool DoingKeyBind = false;
        
        internal static ModOptionPanelController InstanceModOptionPanelController;

        public static void Init()
        {
            Resources.Assets.LoadAssets();

            LanguageTokens.Register();
            
            SettingsModifier.Init();

            On.RoR2.PauseManager.CCTogglePause += PauseManagerOnCCTogglePause;
        }

        private static void PauseManagerOnCCTogglePause(PauseManager.orig_CCTogglePause orig, ConCommandArgs args)
        {
            if (DoingKeyBind)
                return;

            orig(args);
        }

        public static void SetModDescription(string description)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetModInfo();
            
            if (!OptionCollection.ContainsModGuid(modInfo.ModGuid))
                OptionCollection[modInfo.ModGuid] = new OptionCollection(modInfo.ModName, modInfo.ModGuid);

            OptionCollection[modInfo.ModGuid].description = description;
        }

        public static void SetModIcon(Sprite iconSprite)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetModInfo();

            if (!OptionCollection.ContainsModGuid(modInfo.ModGuid))
                OptionCollection[modInfo.ModGuid] = new OptionCollection(modInfo.ModName, modInfo.ModGuid);

            OptionCollection[modInfo.ModGuid].icon = iconSprite;
        }

        public static void AddOption(BaseOption option)
        {
            ModInfo modInfo = Assembly.GetCallingAssembly().GetModInfo();

            option.ModGuid = modInfo.ModGuid;
            option.ModName = modInfo.ModName;
            option.Identifier = $"{modInfo.ModGuid}.{option.Category}.{option.Name}.{option.OptionTypeName}".Replace(" ", "_").ToUpper();
            
            LanguageAPI.Add(option.GetNameToken(), option.Name);
            LanguageAPI.Add(option.GetDescriptionToken(), option.Description);
            
            OptionCollection.AddOption(ref option);
        }
    }
}