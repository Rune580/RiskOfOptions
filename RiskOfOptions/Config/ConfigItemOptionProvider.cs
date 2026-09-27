using System.Linq;
using RiskOfOptions.Options;

namespace RiskOfOptions.Config;

public abstract class ConfigItemOptionProvider
{
    /// <summary>
    /// A collection of validation rules that determine whether the provider
    /// can handle an <see cref="IConfigItem"/>. The more rules a provider has,
    /// the higher the priority it gets.
    /// </summary>
    public abstract IsValidRule[] Rules { get; }

    /// <summary>
    /// The more rules a provider has, the higher the priority it gets.
    /// </summary>
    public int Specificity => Rules.Length;
    
    public bool CanHandle(IConfigItem configItem) => Rules.All(rule => rule.Invoke(configItem));

    /// <summary>
    /// Called after <see cref="CanHandle"/> returns true.
    /// </summary>
    /// <returns>An option that inherits <see cref="BaseOption"/></returns>
    public abstract BaseOption CreateOption(IConfigItem configItem);

    public delegate bool IsValidRule(IConfigItem configItem);
}