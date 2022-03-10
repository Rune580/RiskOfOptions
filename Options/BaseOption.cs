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
        public string Category { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        protected void SetCategoryName(string fallback, BaseOptionConfig config)
        {
            if (!string.IsNullOrEmpty(config.category))
            {
                Category = config.category;
                return;
            }

            Category = fallback;
        }

        protected void SetName(string fallback, BaseOptionConfig config)
        {
            if (!string.IsNullOrEmpty(config.name))
            {
                Name = config.name;
                return;
            }

            Name = fallback;
        }

        protected void SetDescription(string fallback, BaseOptionConfig config)
        {
            if (!string.IsNullOrEmpty(config.description))
            {
                Description = config.description;
                return;
            }

            Description = fallback;
        }
        
        public string GetNameToken()
        {
            return $"{ModSettingsManager.StartingText}.{ModGuid}.{Category}.{Name}.{OptionTypeName}.name".Replace(" ", "_").ToUpper();
        }

        public string GetDescriptionToken()
        {
            return $"{ModSettingsManager.StartingText}.{ModGuid}.{Category}.{Name}.{OptionTypeName}.description".Replace(" ", "_").ToUpper();
        }
        
        public abstract GameObject CreateOptionGameObject(GameObject prefab, Transform parent);

        public abstract BaseOptionConfig GetConfig();
    }
}