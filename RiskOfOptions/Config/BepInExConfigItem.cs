using System;
using BepInEx.Configuration;

namespace RiskOfOptions.Config;

public class BepInExConfigItem<TValue> : BepInExConfigItem, IConfigItem<TValue>
{
    private readonly ConfigEntry<TValue> _entry;

    public BepInExConfigItem(ConfigEntry<TValue> entry) : this(entry, ConfigItemFlags.RestartRequired)
    {
        var acceptableValues = entry.Description.AcceptableValues;
        var rules = new ConfigItemValueValidationRules<TValue>();

        ValidationRules = acceptableValues switch
        {
            AcceptableValueRange<byte> values => rules.SetValueRange(values.MinValue, values.MaxValue),
            AcceptableValueRange<sbyte> values => rules.SetValueRange(values.MinValue, values.MaxValue),
            AcceptableValueRange<ushort> values => rules.SetValueRange(values.MinValue, values.MaxValue),
            AcceptableValueRange<short> values => rules.SetValueRange(values.MinValue, values.MaxValue),
            AcceptableValueRange<uint> values => rules.SetValueRange(values.MinValue, values.MaxValue),
            AcceptableValueRange<int> values => rules.SetValueRange(values.MinValue, values.MaxValue),
            AcceptableValueRange<ulong> values => rules.SetValueRange(values.MinValue, values.MaxValue),
            AcceptableValueRange<float> values => rules.SetValueRange(values.MinValue, values.MaxValue),
            AcceptableValueRange<double> values => rules.SetValueRange(values.MinValue, values.MaxValue),
            _ => rules
        };
    }
    
    public BepInExConfigItem(ConfigEntry<TValue> entry, ConfigItemFlags flags)
    {
        _entry = entry;
        Flags = flags;
    }

    public BepInExConfigItem(ConfigEntry<TValue> entry, ConfigItemValueValidationRules<TValue> validationRules) : this(entry, ConfigItemFlags.RestartRequired, validationRules) { }

    public BepInExConfigItem(ConfigEntry<TValue> entry, ConfigItemFlags flags, ConfigItemValueValidationRules<TValue> validationRules)
    {
        _entry = entry;
        Flags = flags;
        ValidationRules = validationRules;
    }

    public override string Section => _entry.Definition.Section;
    
    public override string Name => _entry.Definition.Key;
    
    public override string Description => _entry.Description.Description;

    public override Type ValueType => _entry.SettingType;
    
    public TValue DefaultValue => (TValue)_entry.DefaultValue;

    public TValue Value
    {
        get => _entry.Value;
        set => _entry.Value = value;
    }

    public static implicit operator BepInExConfigItem<TValue>(ConfigEntry<TValue> entry) => new(entry);
}

public class BepInExObjectConfigItem : BepInExConfigItem, IConfigItem<object>
{
    private readonly ConfigEntryBase _entry;

    public BepInExObjectConfigItem(ConfigEntryBase entry, ConfigItemFlags flags = ConfigItemFlags.RestartRequired)
    {
        _entry = entry;
        Flags = flags;
        ValidationRules = new ConfigItemValueValidationRules<object>();
    }

    public override string Section => _entry.Definition.Section;
    
    public override string Name => _entry.Definition.Key;
    
    public override string Description => _entry.Description.Description;

    public override Type ValueType => _entry.SettingType;
    
    public object DefaultValue => _entry.DefaultValue;
    
    public object Value
    {
        get => _entry.BoxedValue;
        set => _entry.BoxedValue = value;
    }
}

public abstract class BepInExConfigItem : IConfigItem
{
    public abstract string Section { get; }
    
    public abstract string Name { get; }
    
    public abstract string Description { get; }
    
    public abstract Type ValueType { get; }
    
    public ConfigItemFlags Flags { get; protected init; }
    
    public ConfigItemValueValidationRules ValidationRules { get; protected init; }
}