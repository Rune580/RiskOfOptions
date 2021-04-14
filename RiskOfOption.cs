using BepInEx;
using R2API;
using RoR2.ConVar;
using System;
using System.Globalization;
using System.Reflection;
using RiskOfOptions.OptionOverrides;
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

        public OptionOverride OptionOverride;

        public bool Visibility;

        public bool RestartRequired;

        public string CategoryName { get; internal set; }

        internal BaseConVar ConVar;

        internal string Value;

        //internal bool isOverride = false;

        public enum OptionType
        {
            Bool,
            Slider,
            Keybinding
        }

        internal RiskOfOption(string modGuid, string modName, OptionType optionType, string name, string description, string defaultValue, string categoryName, OptionOverride optionOverride, bool visibility, bool restartRequired)
        {
            this.optionType = optionType;
            Name = name;
            Description = description;
            DefaultValue = defaultValue;

            Visibility = visibility;
            RestartRequired = restartRequired;

            if (categoryName == "")
            {
                categoryName = "Main";
            }

            if (optionOverride != null)
            {
                OptionOverride = optionOverride;
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

            if (OptionOverride != null)
            {
                Indexes indexes = ModSettingsManager.OptionContainers.GetIndexes(ModGuid, OptionOverride.Name, OptionOverride.CategoryName);

                bool overrideValue = ModSettingsManager.OptionContainers[indexes.ContainerIndex].GetModOptionsCached()[indexes.OptionIndexInContainer].GetBool();

                if ((overrideValue && OptionOverride.OverrideOnTrue) || (!overrideValue && !OptionOverride.OverrideOnTrue))
                {
                    Value = $"{(((CheckBoxOverride)OptionOverride).ValueToReturnWhenOverriden ? "1" : "0")}";
                    return ((CheckBoxOverride) OptionOverride).ValueToReturnWhenOverriden;
                }
            }

            return ConVar != null ? (int.Parse(ConVar.GetString()) == 1) : bool.Parse(Value);
        }

        public float GetFloat()
        {
            if (optionType != OptionType.Slider)
            {
                throw new Exception($"Option {Name} is not a Slider!");
            }

            if (OptionOverride != null)
            {
                Indexes indexes = ModSettingsManager.OptionContainers.GetIndexes(ModGuid, OptionOverride.Name, OptionOverride.CategoryName);

                bool overrideValue = ModSettingsManager.OptionContainers[indexes.ContainerIndex].GetModOptionsCached()[indexes.OptionIndexInContainer].GetBool();

                if ((overrideValue && OptionOverride.OverrideOnTrue) || (!overrideValue && !OptionOverride.OverrideOnTrue))
                {
                    Value = ((SliderOverride)OptionOverride).ValueToReturnWhenOverriden.ToString(CultureInfo.InvariantCulture);
                    return ((SliderOverride)OptionOverride).ValueToReturnWhenOverriden;
                }
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

        public string GetValue()
        {
            switch (optionType)
            {
                case OptionType.Bool:
                    return $"{(GetBool() ? "1" : "0")}";
                case OptionType.Slider:
                    return GetFloat().ToString(CultureInfo.InvariantCulture);
                case OptionType.Keybinding:
                    return $"{(int) GetKeyCode()}";
            }

            throw new Exception("Option doesn't have value");
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
