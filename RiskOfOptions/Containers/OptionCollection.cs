using System.Collections.Generic;
using RiskOfOptions.Lib;
using RiskOfOptions.Options;
using UnityEngine;

namespace RiskOfOptions.Containers
{
    internal class OptionCollection
    {
        private readonly List<Category> _categories = new();
        private readonly Dictionary<string, int> _nameCategoryMap = new();
        private readonly Dictionary<string, string> _identifierCategoryNameMap = new();
        public string ModName { get; }
        public string ModGuid { get; }

        public Sprite? icon = null;
        public GameObject? iconPrefab = null;

        internal int CategoryCount => _categories.Count;
        internal string NameToken => $"{ModSettingsManager.StartingText}.{ModGuid}.mod_list_button.name".Replace(" ", "_").ToUpper();

        private string? _descriptionToken;
        internal string DescriptionToken
        {
            get
            {
                _descriptionToken ??= $"{ModSettingsManager.StartingText}.{ModGuid}.mod_list_button.description".Replace(" ", "_").ToUpper();
                return _descriptionToken;
            }
            set => _descriptionToken = value;
        }

        internal OptionCollection(string modName, string modGuid)
        {
            ModName = modName;
            ModGuid = modGuid;
            
            LanguageApi.Add(NameToken, ModName);
        }

        internal void SetDescriptionText(string descriptionText)
        {
            LanguageApi.Add(DescriptionToken, descriptionText);
        }

        internal void AddOption(ref BaseOption option)
        {
            if (!_nameCategoryMap.ContainsKey(option.Category))
            {
                int categoryIndex = _categories.Count;
                _categories.Add(new Category(option.Category, ModGuid));
                _nameCategoryMap[option.Category] = categoryIndex;
            }
            
            _categories[_nameCategoryMap[option.Category]].AddOption(ref option);
            _identifierCategoryNameMap[option.Identifier] = option.Category;
        }

        internal BaseOption GetOption(string identifier)
        {
            string categoryName = _identifierCategoryNameMap[identifier];

            return _categories[_nameCategoryMap[_identifierCategoryNameMap[identifier]]].GetOption(identifier);
        }

        internal Category this[int index] => _categories[index];

        internal Category this[string categoryName] => _categories[_nameCategoryMap[categoryName]];
    }
}