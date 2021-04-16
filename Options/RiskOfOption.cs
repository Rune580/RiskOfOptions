using System;
using R2API;
using RiskOfOptions.OptionOverrides;
using RoR2.ConVar;
using RoR2.UI;
using UnityEngine;
using static RiskOfOptions.ExtensionMethods;
// ReSharper disable IdentifierTypo

namespace RiskOfOptions.Options
{
    public class RiskOfOption : OptionBase
    {
        public string DefaultValue;

        public OptionOverride OptionOverride;

        public bool Visibility;

        public bool RestartRequired;

        public string CategoryName { get; internal set; }

        public BaseConVar ConVar;

        //internal string Value;

        internal const BaseSettingsControl.SettingSource RooSettingSource = (BaseSettingsControl.SettingSource)2;

        //internal bool isOverride = false;


        internal RiskOfOption(string modGuid, string modName, string name, object[] description, string defaultValue, string categoryName, OptionOverride optionOverride, bool visibility, bool restartRequired)
        {
            Name = name;
            Description = description;
            DefaultValue = defaultValue;

            Visibility = visibility;
            RestartRequired = restartRequired;

            if (optionOverride != null)
            {
                OptionOverride = optionOverride;
            }

            CategoryName = categoryName;

            ModGuid = modGuid;
            ModName = modName;

            //OptionToken = $"{ModSettingsManager.StartingText}.{modGuid}.{modName}.category_{categoryName.Replace(".", "")}.{name}.{optionType}".ToUpper().Replace(" ", "_");
        }
        // quote
        internal void RegisterTokens()
        {

            ConsoleToken = OptionToken.Replace("'", "-singlequote-").Replace("\"", "-doublequote-").ToLower();

            NameToken = $"{OptionToken}.NAME_TOKEN";

            LanguageAPI.Add(NameToken, Name);
        }

        public string GetDescriptionAsString()
        {
            string temp = "";

            foreach (var o in Description)
            {
                switch (o)
                {
                    case string _:
                        temp += o.ToString();
                        break;
                    case Sprite sprite:
                        temp += $"<Image:{sprite.name}>";
                        break;
                }
            }

            return temp;
        }

        //public bool GetBool()
        //{
        //    if (optionType != OptionType.Bool)
        //    {
        //        throw new Exception($"Option {Name} is not a Bool!");
        //    }

        //    if (OptionOverride != null)
        //    {
        //        Indexes indexes = ModSettingsManager.OptionContainers.GetIndexes(ModGuid, OptionOverride.Name, OptionOverride.CategoryName);

        //        bool overrideValue = ModSettingsManager.OptionContainers[indexes.ContainerIndex].GetModOptionsCached()[indexes.OptionIndexInContainer].GetBool();

        //        if ((overrideValue && OptionOverride.OverrideOnTrue) || (!overrideValue && !OptionOverride.OverrideOnTrue))
        //        {
        //            Value = $"{(((CheckBoxOverride)OptionOverride).ValueToReturnWhenOverriden ? "1" : "0")}";
        //            return ((CheckBoxOverride) OptionOverride).ValueToReturnWhenOverriden;
        //        }
        //    }

        //    return ConVar != null ? (int.Parse(ConVar.GetString()) == 1) : bool.Parse(Value);
        //}

        //public float GetFloat()
        //{
        //    if (optionType != OptionType.Slider)
        //    {
        //        throw new Exception($"Option {Name} is not a Slider!");
        //    }

        //    if (OptionOverride != null)
        //    {
        //        Indexes indexes = ModSettingsManager.OptionContainers.GetIndexes(ModGuid, OptionOverride.Name, OptionOverride.CategoryName);

        //        bool overrideValue = ModSettingsManager.OptionContainers[indexes.ContainerIndex].GetModOptionsCached()[indexes.OptionIndexInContainer].GetBool();

        //        if ((overrideValue && OptionOverride.OverrideOnTrue) || (!overrideValue && !OptionOverride.OverrideOnTrue))
        //        {
        //            Value = ((SliderOverride)OptionOverride).ValueToReturnWhenOverriden.ToString(CultureInfo.InvariantCulture);
        //            return ((SliderOverride)OptionOverride).ValueToReturnWhenOverriden;
        //        }
        //    }

        //    return float.Parse(ConVar != null ? ConVar.GetString() : Value);
        //}

        //public KeyCode GetKeyCode()
        //{
        //    if (optionType != OptionType.Keybinding)
        //    {
        //        throw new Exception($"Option {Name} is not a KeyCode!");
        //    }

        //    if (ConVar != null)
        //    {
        //        return (KeyCode)int.Parse(ConVar.GetString());
        //    }

        //    return (KeyCode)int.Parse(Value);
        //}

        //public string GetValue()
        //{
        //    switch (optionType)
        //    {
        //        case OptionType.Bool:
        //            return $"{(GetBool() ? "1" : "0")}";
        //        case OptionType.Slider:
        //            return GetFloat().ToString(CultureInfo.InvariantCulture);
        //        case OptionType.Keybinding:
        //            return $"{(int) GetKeyCode()}";
        //    }

        //    throw new Exception("Option doesn't have value");
        //}

        public static bool operator ==(RiskOfOption a, RiskOfOption b)
        {
            return a?.OptionToken == b?.OptionToken;
        }

        public static bool operator !=(RiskOfOption a, RiskOfOption b)
        {
            return a?.OptionToken != b?.OptionToken;
        }

        public virtual GameObject CreateOptionGameObject(RiskOfOption option, GameObject prefab, Transform parent)
        {
            throw new NotImplementedException();
        }

        public virtual string GetValueAsString()
        {
            throw new NotImplementedException();
        }

        public virtual string GetInternalValueAsString()
        {
            throw new NotImplementedException();
        }

        public virtual void SetValue(string newValue)
        {
            throw new NotImplementedException();
        }
    }
}
