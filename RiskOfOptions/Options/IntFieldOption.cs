using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.OptionConfigs;
using UnityEngine;

namespace RiskOfOptions.Options;

public class IntFieldOption : BaseOption, ITypedValueHolder<int>
{
    protected readonly int originalValue;
    private readonly ConfigEntry<int> _configEntry;
    protected readonly IntFieldConfig config;
    
    public IntFieldOption(ConfigEntry<int> configEntry) : this(configEntry, new IntFieldConfig()) { }
        
    public IntFieldOption(ConfigEntry<int> configEntry, bool restartRequired) : this(configEntry, new IntFieldConfig { restartRequired = restartRequired }) { }

    public IntFieldOption(ConfigEntry<int> configEntry, IntFieldConfig config) : this(config, configEntry.Value)
    {
        _configEntry = configEntry;
    }
    
    protected IntFieldOption(IntFieldConfig config, int originalValue)
    {
        this.originalValue = originalValue;
        this.config = config;
    }

    public override string OptionTypeName { get; protected set; } = "int_field";
    
    internal override ConfigEntryBase ConfigEntry => _configEntry;
    
    public override GameObject CreateOptionGameObject(GameObject prefab, Transform parent)
    {
        var intField = Object.Instantiate(prefab, parent);

        var settingsField = intField.GetComponentInChildren<ModSettingsIntField>();

        settingsField.nameToken = GetNameToken();
        settingsField.settingToken = Identifier;

        settingsField.min = config.Min;
        settingsField.max = config.Max;
        settingsField.formatString = config.FormatString;

        settingsField.name = $"Mod Options Int Field, {Name}";

        return intField;
    }

    public override BaseOptionConfig GetConfig() => config;

    public int GetOriginalValue() => originalValue;

    public virtual int Value
    {
        get => _configEntry.Value;
        set => _configEntry.Value = value;
    }
    
    public bool ValueChanged() => Value != GetOriginalValue();
}