using System.Collections.Generic;

namespace RiskOfOptions.Config;

public interface IModConfigProvider
{
    public IEnumerable<IModConfig> GetModConfigs();
}