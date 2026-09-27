using System;

namespace RiskOfOptions.Config;

[Flags]
public enum ConfigItemFlags : uint
{
    None = 0,
    RestartRequired = 1,
    /// <summary>
    /// Indicates that the option's value should be hidden by default.
    /// Useful for passwords or api tokens.
    /// </summary>
    SecretContent = 1 << 1,
}