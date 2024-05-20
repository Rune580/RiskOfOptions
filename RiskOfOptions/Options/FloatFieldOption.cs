using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.OptionConfigs;
using UnityEngine;

namespace RiskOfOptions.Options;

public class FloatFieldOption : BaseOption, ITypedValueHolder<float>
{
    protected readonly float originalValue;
    private readonly ConfigEntry<float> _configEntry;
    protected readonly FloatFieldConfig config;
    
    public FloatFieldOption(ConfigEntry<float> configEntry) : this(configEntry, new FloatFieldConfig()) { }
        
    public FloatFieldOption(ConfigEntry<float> configEntry, bool restartRequired) : this(configEntry, new FloatFieldConfig { restartRequired = restartRequired }) { }

    public FloatFieldOption(ConfigEntry<float> configEntry, FloatFieldConfig config) : this(config, configEntry.Value)
    {
        _configEntry = configEntry;
    }
    
    protected FloatFieldOption(FloatFieldConfig config, float originalValue)
    {
        this.originalValue = originalValue;
        this.config = config;
    }

    public override string OptionTypeName { get; protected set; } = "float_field";
    
    internal override ConfigEntryBase ConfigEntry => _configEntry;
    
    public override GameObject CreateOptionGameObject(GameObject prefab, Transform parent)
    {
        var floatField = Object.Instantiate(prefab, parent);

        var settingsField = floatField.GetComponentInChildren<ModSettingsFloatField>();

        settingsField.nameToken = GetNameToken();
        settingsField.settingToken = Identifier;

        settingsField.min = config.Min;
        settingsField.max = config.Max;
        settingsField.formatString = config.FormatString;

        settingsField.name = $"Mod Options Float Field, {Name}";

        return floatField;
    }

    public override BaseOptionConfig GetConfig() => config;

    public float GetOriginalValue() => originalValue;

    public virtual float Value
    {
        get => _configEntry.Value;
        set => _configEntry.Value = value;
    }
    
    // ReSharper disable once CompareOfFloatsByEqualityOperator
    public bool ValueChanged() => Value != GetOriginalValue();
}