using BepInEx.Configuration;

namespace RiskOfOptions.Config;

public class BepInExConfigItem<TValue>(ConfigEntry<TValue> entry) : IConfigItem<TValue>
{
    public string Section => entry.Definition.Section;
    
    public string Name => entry.Definition.Key;
    
    public string Description => entry.Description.Description;
    
    public TValue DefaultValue => (TValue)entry.DefaultValue;

    public TValue Value
    {
        get => entry.Value;
        set => entry.Value = value;
    }

    public static implicit operator BepInExConfigItem<TValue>(ConfigEntry<TValue> entry) => new(entry);
}

public class BepInExConfigItem(ConfigEntryBase entry) : IConfigItem<object>
{
    public string Section => entry.Definition.Section;
    
    public string Name => entry.Definition.Key;
    
    public string Description => entry.Description.Description;
    
    public object DefaultValue => entry.DefaultValue;
    
    public object Value
    {
        get => entry.BoxedValue;
        set => entry.BoxedValue = value;
    }
}