using System.Collections.Generic;
using R2API;
using RiskOfOptions.Options;

namespace RiskOfOptions.Containers
{
    internal class Category
    {
        public readonly string name;
        private readonly List<BaseOption> _options = new();
        private readonly Dictionary<string, int> _identifierOptionMap = new();
        
        public string ModGuid { get; }
        
        internal string NameToken => $"{ModSettingsManager.StartingText}.{ModGuid}.category.{name}".Replace(" ", "_").ToUpper();
        internal int OptionCount => _options.Count;
        
        internal Category(string name, string modGuid)
        {
            this.name = name;
            ModGuid = modGuid;
            
            LanguageAPI.Add(NameToken, name);
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
    }
}