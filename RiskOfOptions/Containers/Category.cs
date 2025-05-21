using System.Collections.Generic;
using RiskOfOptions.Lib;
using RiskOfOptions.Options;

namespace RiskOfOptions.Containers
{
    internal class Category
    {
        public readonly string name;
        private readonly List<BaseOption> _options = new();
        private readonly Dictionary<string, int> _identifierOptionMap = new();
        
        public string ModGuid { get; }
        
        internal string NameToken
        {
            get
            {
                if (!string.IsNullOrEmpty(_customNameToken))
                    return _customNameToken;
                return $"{ModSettingsManager.StartingText}.{ModGuid}.category.{name}".Replace(" ", "_").ToUpper();
            }
        }

        internal int OptionCount => _options.Count;
        
        private string _customNameToken;
        
        internal Category(string name, string modGuid)
        {
            this.name = name;
            ModGuid = modGuid;
            _customNameToken = string.Empty;
            
            LanguageApi.Add(NameToken, name);
        }

        internal void AddOption(ref BaseOption option)
        {
            int optionIndex = _options.Count;
            _options.Add(option);
            _identifierOptionMap.Add(option.Identifier, optionIndex);
        }

        internal BaseOption GetOption(string identifier)
        {
            return _options[_identifierOptionMap[identifier]];
        }

        internal BaseOption this[int index] => _options[index];

        private bool Equals(Category other)
        {
            return name == other.name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == GetType() && Equals((Category)obj);
        }

        public override int GetHashCode()
        {
            return name != null ? name.GetHashCode() : 0;
        }

        public static bool operator ==(Category left, Category right)
        {
            return right is not null && left is not null && left.name == right.name;
        }

        public static bool operator !=(Category left, Category right)
        {
            return !(left == right);
        }
        
        /// <summary>
        /// Sets a custom name token for the category.
        /// Pass in an empty string to remove the custom token (returns to default token).
        /// </summary>
        /// <param name="nameToken">Token to set</param>
        public void SetNameToken(string nameToken)
        {
            _customNameToken = nameToken;
        }
    }
}