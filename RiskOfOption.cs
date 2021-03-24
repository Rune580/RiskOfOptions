using BepInEx;
using R2API;
using RoR2.ConVar;
using System;
using System.Reflection;
using UnityEngine;

namespace RiskOfOptions
{
    public class RiskOfOption : OptionBase
    {
        public OptionType optionType;

        public string defaultValue;

        public UnityEngine.Events.UnityAction<bool> onValueChangedBool;
        public UnityEngine.Events.UnityAction<float> onValueChangedFloat;
        public UnityEngine.Events.UnityAction<KeyCode> onValueChangedKeyCode;

        public string CategoryName { get; internal set; }

        internal BaseConVar conVar;

        public GameObject gameObject;

        public enum OptionType
        {
            Bool,
            Slider,
            Keybinding
        }

        internal RiskOfOption(string ModGUID, string ModName, OptionType optionType, string Name, string Description, string DefaultValue, string CategoryName = "")
        {
            this.optionType = optionType;
            this.Name = Name;
            this.Description = Description;
            defaultValue = DefaultValue;

            if (CategoryName == "")
            {
                CategoryName = "Default";
            }

            this.CategoryName = CategoryName;

            this.ModGUID = ModGUID;
            this.ModName = ModName;

            OptionToken = $"{ModSettingsManager.StartingText}.{ModGUID}.category_{CategoryName}.{Name}.{optionType}".ToUpper().Replace(" ", "_");

            ConsoleToken = OptionToken.ToLower();

            NameToken = $"{OptionToken}.NAME_TOKEN";

            DescriptionToken = $"{OptionToken}.DESCRIPTION_TOKEN";

            RegisterTokens();
        }

        public RiskOfOption(OptionType optionType, string Name, string Description, string DefaultValue, string CategoryName = "")
        {
            this.optionType = optionType;
            this.Name = Name;
            this.Description = Description;
            defaultValue = DefaultValue;

            this.CategoryName = CategoryName;

            var classes = Assembly.GetCallingAssembly().GetExportedTypes();

            foreach (var item in classes)
            {
                BepInPlugin bepInPlugin = item.GetCustomAttribute<BepInPlugin>();

                if (bepInPlugin != null)
                {
                    ModGUID = bepInPlugin.GUID;
                    ModName = bepInPlugin.Name;
                }
            }

            if (CategoryName == "")
            {
                CategoryName = "Default";
            }

            OptionToken = $"{ModSettingsManager.StartingText}.{ModGUID}.category_{CategoryName}.{Name}.{optionType}".ToUpper().Replace(" ", "_");

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
            return bool.Parse(conVar.GetString());
        }

        public float GetFloat()
        {
            if (optionType != OptionType.Slider)
            {
                throw new Exception($"Option {Name} is not a Slider!");
            }
            return float.Parse(conVar.GetString());
        }

        public KeyCode GetKeyCode()
        {
            if (optionType != OptionType.Keybinding)
            {
                throw new Exception($"Option {Name} is not a KeyCode!");
            }
            return (KeyCode)int.Parse(conVar.GetString());
        }

        public static bool operator ==(RiskOfOption a, RiskOfOption b)
        {
            if (a.DescriptionToken == b.DescriptionToken)
            {
                return true;
            }
            return false;
        }

        public static bool operator !=(RiskOfOption a, RiskOfOption b)
        {
            if (a.DescriptionToken != b.DescriptionToken)
            {
                return true;
            }
            return false;
        }
    }
}
