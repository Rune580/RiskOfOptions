using BepInEx;
using R2API;
using RoR2.ConVar;
using System;
using System.Reflection;
using UnityEngine;

using static RiskOfOptions.ExtensionMethods;
// ReSharper disable IdentifierTypo

namespace RiskOfOptions
{
    public class RiskOfOption : OptionBase
    {
        public OptionType optionType;

        public string DefaultValue;

        public UnityEngine.Events.UnityAction<bool> OnValueChangedBool;
        public UnityEngine.Events.UnityAction<float> OnValueChangedFloat;
        public UnityEngine.Events.UnityAction<KeyCode> OnValueChangedKeyCode;

        internal ModSettingsManager.OptionInfo overridingOption;
        public string CategoryName { get; internal set; }

        internal BaseConVar ConVar;

        internal string Value;

        public enum OptionType
        {
            Bool,
            Slider,
            Keybinding
        }

        internal RiskOfOption(string modGuid, string modName, OptionType optionType, string name, string description, string defaultValue, string categoryName = "")
        {
            this.optionType = optionType;
            Name = name;
            Description = description;
            DefaultValue = defaultValue;

            if (categoryName == "")
            {
                categoryName = "Main";
            }

            CategoryName = categoryName;

            ModGuid = modGuid;
            ModName = modName;

            OptionToken = $"{ModSettingsManager.StartingText}.{modGuid}.{modName}.category_{categoryName.Replace(".", "")}.{name}.{optionType}".ToUpper().Replace(" ", "_");

            ConsoleToken = OptionToken.ToLower();

            NameToken = $"{OptionToken}.NAME_TOKEN";

            DescriptionToken = $"{OptionToken}.DESCRIPTION_TOKEN";

            RegisterTokens();
        }

        public RiskOfOption(OptionType optionType, string name, string description, string defaultValue, string categoryName = "")
        {
            this.optionType = optionType;
            Name = name;
            Description = description;
            DefaultValue = defaultValue;

            if (categoryName == "")
            {
                categoryName = "Main";
            }

            CategoryName = categoryName;

            ModInfo modInfo = Assembly.GetCallingAssembly().GetExportedTypes().GetModInfo();

            ModGuid = modInfo.ModGuid;
            ModName = modInfo.ModName;

            OptionToken = $"{ModSettingsManager.StartingText}.{ModGuid}.{ModName}.category_{categoryName}.{name}.{optionType}".ToUpper().Replace(" ", "_");

            ConsoleToken = OptionToken.ToLower();

            NameToken = $"{OptionToken}.NAME_TOKEN";

            DescriptionToken = $"{OptionToken}.DESCRIPTION_TOKEN";

            RegisterTokens();
        }

        internal void RegisterTokens()
        {
            LanguageAPI.Add(NameToken, Name);
            LanguageAPI.Add(DescriptionToken, Description);
        }

        public bool GetBool()
        {
            if (optionType != OptionType.Bool)
            {
                throw new Exception($"Option {Name} is not a Bool!");
            }

            return ConVar != null ? (int.Parse(ConVar.GetString()) == 1) : bool.Parse(Value);
        }

        public float GetFloat()
        {
            if (optionType != OptionType.Slider)
            {
                throw new Exception($"Option {Name} is not a Slider!");
            }

            return float.Parse(ConVar != null ? ConVar.GetString() : Value);
        }

        public KeyCode GetKeyCode()
        {
            if (optionType != OptionType.Keybinding)
            {
                throw new Exception($"Option {Name} is not a KeyCode!");
            }

            if (ConVar != null)
            {
                return (KeyCode)int.Parse(ConVar.GetString());
            }

            return (KeyCode)int.Parse(Value);
        }

        public static bool operator ==(RiskOfOption a, RiskOfOption b)
        {
            return a?.DescriptionToken == b?.DescriptionToken;
        }

        public static bool operator !=(RiskOfOption a, RiskOfOption b)
        {
            return a?.DescriptionToken != b?.DescriptionToken;
        }
    }
}
