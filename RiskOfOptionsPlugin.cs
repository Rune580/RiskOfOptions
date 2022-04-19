﻿using BepInEx;
using BepInEx.Configuration;

namespace RiskOfOptions
{
    [BepInPlugin(Guid, ModName, Version)]
    public sealed class RiskOfOptionsPlugin : BaseUnityPlugin
    {
        private const string
            ModName = "Risk Of Options",
            Author = "rune580",
            Guid = "com." + Author + "." + "riskofoptions",
            Version = "2.4.0";

        internal static ConfigEntry<bool> seenNoMods;
        internal static ConfigEntry<bool> seenMods;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Awake is automatically called by Unity")]
        private void Awake()
        {
            seenNoMods = Config.Bind("One Time Stuff", "Has seen the no mods prompt", false);
            seenMods = Config.Bind("One Time Stuff", "Has seen the mods prompt", false);
            
            ModSettingsManager.Init();
        }
    }
}