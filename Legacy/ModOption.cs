using System;
using System.Reflection;
using RoR2.ConVar;
using UnityEngine;
using static RiskOfOptions.ExtensionMethods;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable IdentifierTypo
#pragma warning disable 660,661

namespace RiskOfOptions.Legacy
{
    [Obsolete("ModOption is now obsolete. please use addOption(...)")]
    public class ModOption
    {
        public OptionType optionType;
        public string longName;
        public string owner { get; private set; }
        public string modName { get; private set; }
        public string name;
        public string description;

        public string defaultValue;

        public UnityEngine.Events.UnityAction<bool> onValueChangedBool;
        public UnityEngine.Events.UnityAction<float> onValueChangedFloat;

        public BaseConVar conVar;

        public GameObject gameObject;

        public enum OptionType
        {
            Bool,
            Slider,
            Keybinding
        }

        public ModOption(OptionType _optionType, string _name, string _description, string _defaultValue = null)
        {
            optionType = _optionType;
            name = _name;
            description = _description;
            defaultValue = _defaultValue;

            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            owner = modInfo.ModGuid;
            modName = modInfo.ModName;
        }

        internal void SetOwner(string modGuid)
        {
            owner = modGuid;
        }

        public static bool operator ==(ModOption a, ModOption b)
        {
            return a?.longName == b?.longName;
        }

        public static bool operator !=(ModOption a, ModOption b)
        {
            return a?.longName != b?.longName;
        }


    }
}