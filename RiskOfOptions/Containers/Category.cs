using System.Collections.Generic;
using RiskOfOptions.Lib;
using RiskOfOptions.Options;

namespace RiskOfOptions.Containers;

internal class Category
{
    public string Name { get; }
    private readonly List<BaseOption> _options = [];
    private readonly Dictionary<string, int> _identifierOptionMap = [];
        
    public string ModGuid { get; }
        
    internal string NameToken
    {
        get
        {
            if (!string.IsNullOrEmpty(_customNameToken))
                return _customNameToken;
            return $"{ModSettingsManager.StartingText}.{ModGuid}.category.{Name}".Replace(" ", "_").ToUpper();
        }
    }

    internal int OptionCount => _options.Count;
        
    private string _customNameToken;
        
    internal Category(string name, string modGuid)
    {
        Name = name;
        ModGuid = modGuid;
        _customNameToken = string.Empty;
            
        LanguageApi.Add(NameToken, name);
    }

    internal void AddOption(ref BaseOption option)
    {
        int optionIndex = _options.Count;
        _options.Add(option);
        _identifierOptionMap.Add(option.Id, optionIndex);
    }

    internal BaseOption GetOption(string identifier)
    {
        return _options[_identifierOptionMap[identifier]];
    }

    internal BaseOption this[int index] => _options[index];

    private bool Equals(Category other)
    {
        return Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        return obj.GetType() == GetType() && Equals((Category)obj);
    }

    public override int GetHashCode()
    {
        return !string.IsNullOrEmpty(Name) ? Name.GetHashCode() : 0;
    }

    public static bool operator ==(Category? left, Category? right)
    {
        return left is not null && right is not null && left.Name == right.Name;
    }

    public static bool operator !=(Category? left, Category? right)
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