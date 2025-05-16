using System;
using BepInEx.Configuration;
using RiskOfOptions.Lib;
using RiskOfOptions.OptionConfigs;
using RoR2;
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
        public string DescriptionToken { get; internal set; }
        public string NameToken { get; internal set; }

        internal abstract ConfigEntryBase ConfigEntry { get; }

        public virtual void SetCategoryName(string fallback, BaseOptionConfig config)
        {
            if (!string.IsNullOrEmpty(config.category))
            {
                Category = config.category;
                return;
            }

            Category = fallback;
        }

        public virtual void SetName(string fallback, BaseOptionConfig config)
        {
            if (!string.IsNullOrEmpty(config.name))
            {
                Name = config.name;
                return;
            }

            Name = fallback;
        }

        public virtual void SetDescription(string fallback, BaseOptionConfig config)
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
            LanguageApi.Add(GetNameToken(), Name);
            LanguageApi.Add(GetDescriptionToken(), Description);
        }
        
        public string GetNameToken()
        {
            if (!string.IsNullOrEmpty(NameToken))
                return NameToken;
            return $"{ModSettingsManager.StartingText}.{ModGuid}.{Category}.{Name}.{OptionTypeName}.name".Replace(" ", "_").ToUpper();
        }

        public string GetDescriptionToken()
        {
            if (!string.IsNullOrEmpty(DescriptionToken))
                return DescriptionToken;
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

        protected internal virtual void SetProperties()
        {
            if (ConfigEntry == null) return;
            var config = GetConfig();
            SetCategoryName(ConfigEntry.Definition.Section, config);
            SetName(ConfigEntry.Definition.Key, config);
            SetDescription(ConfigEntry.Description.Description, config);
        }

        /// <summary>
        /// If a custom name token is present, translates it and returns the final string.
        /// Otherwise, returns the default name (should be the same as the config value).
        /// </summary>
        public string GetLocalizedName()
        {
            return Language.GetString(GetNameToken());
        }


        /// <summary>
        /// If a custom description token is present, translates it and returns the final string.
        /// Otherwise, returns the default description (should be the same as the config value).
        /// </summary>
        public string GetLocalizedDescription()
        {
            return Language.GetString(GetDescriptionToken());
        }


    }
}