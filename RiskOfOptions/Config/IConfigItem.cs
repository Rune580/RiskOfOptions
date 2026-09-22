using System;

namespace RiskOfOptions.Config;

public interface IConfigItem<TValue> : IConfigItem
{
    public TValue DefaultValue { get; }

    public TValue Value { get; set; }

    Type IConfigItem.ValueType => Value!.GetType();
}

public interface IConfigItem
{
    public string Section { get; }
    
    public string Name { get; }
    
    public string Description { get; }
    
    public Type ValueType { get; }
    
    public ConfigItemFlags Flags { get; }
    
    public ConfigItemValueValidationRules ValidationRules { get; }
}