using System;
using System.Collections.Generic;
using System.Linq;
using R2API;
using RiskOfOptions.Options;

namespace RiskOfOptions.Containers
{
    internal class OptionCollection
    {
        private readonly List<Category> _categories = new();
        private readonly Dictionary<string, int> _nameCategoryMap = new();
        private readonly Dictionary<string, string> _identifierCategoryNameMap = new();
        public string ModName { get; }
        public string ModGuid { get; }

        public string description = "";
        internal int CategoryCount => _categories.Count;
        internal string NameToken => $"{ModSettingsManager.StartingText}.{ModGuid}.mod_list_button".Replace(" ", "_").ToUpper();

        internal OptionCollection(string modName, string modGuid)
        {
            ModName = modName;
            ModGuid = modGuid;
            
            LanguageAPI.Add(NameToken, ModName);
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