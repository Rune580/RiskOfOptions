using System;
using BepInEx.Configuration;
using R2API;
using RiskOfOptions.OptionConfigs;
using UnityEngine;

namespace RiskOfOptions.Options
{
    public abstract class BaseOption
    {
        public abstract string OptionTypeName { get; protected set; }
        public string Identifier { get; internal set; }
        public string ModGuid { get; internal set; }
        public string ModName { get; internal set; }
        public string Category { get; internal set; }
        public string Name { get; internal set; }
        public string Description { get; internal set; }
        
        internal abstract ConfigEntryBase ConfigEntry { get; }

        internal void SetCategoryName(string fallback, BaseOptionConfig config)
        {
            if (!string.IsNullOrEmpty(config.category))
            {
                Category = config.category;
                return;
            }

            Category = fallback;
        }

        internal void SetName(string fallback, BaseOptionConfig config)
        {
            if (!string.IsNullOrEmpty(config.name))
            {
                Name = config.name;
                return;
            }

            Name = fallback;
        }

        internal void SetDescription(string fallback, BaseOptionConfig config)
        {
            if (!string.IsNullOrEmpty(config.description))
            {
                Description = config.description;
                return;
            }

            Description = fallback;
        }

        internal virtual void RegisterTokens()
        {
            LanguageAPI.Add(GetNameToken(), Name);
            LanguageAPI.Add(GetDescriptionToken(), Description);
        }
        
        internal string GetNameToken()
        {
            return $"{ModSettingsManager.StartingText}.{ModGuid}.{Category}.{Name}.{OptionTypeName}.name".Replace(" ", "_").ToUpper();
        }

        internal string GetDescriptionToken()
        {
            return $"{ModSettingsManager.StartingText}.{ModGuid}.{Category}.{Name}.{OptionTypeName}.description".Replace(" ", "_").ToUpper();
        }
        
        public abstract GameObject CreateOptionGameObject(GameObject prefab, Transform parent);

        public abstract BaseOptionConfig GetConfig();

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            return obj is BaseOption other && Equals(other);
        }

        private bool Equals(BaseOption other)
        {
            return string.Equals(Identifier, other.Identifier, StringComparison.InvariantCulture);
        }

        public override int GetHashCode()
        {
            // ReSharper disable twice NonReadonlyMemberInGetHashCode
            return (Identifier != null ? StringComparer.InvariantCulture.GetHashCode(Identifier) : 0);
        }
    }

    internal static class OptionExtensions
    {
        internal static void SetProperties(this BaseOption option)
        {
            var entry = option.ConfigEntry;
            if (entry == null)
                return;

            var config = option.GetConfig();
            
            option.SetCategoryName(entry.Definition.Section, config);
            option.SetName(entry.Definition.Key, config);
            option.SetDescription(entry.Description.Description, config);
        }
    }
}