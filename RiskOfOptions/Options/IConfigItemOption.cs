using RiskOfOptions.Config;

namespace RiskOfOptions.Options;

public interface IConfigItemOption<TValue>
{
    public IConfigItem<TValue> ConfigItem { get; }

    public TValue Value { get; set; }
}