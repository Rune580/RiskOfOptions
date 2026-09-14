using System;
using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.Config;
using RiskOfOptions.Lib;
using RiskOfOptions.OptionConfigs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RiskOfOptions.Options;

public class ChoiceOption : BaseOption, IConfigItemOption<object>
{
    public IConfigItem<object> ConfigItem { get; }
    protected readonly ChoiceConfig config;
    private string[] _nameTokens = [];
        
    [Obsolete]
    public ChoiceOption(ConfigEntryBase configEntry) : this(configEntry, new ChoiceConfig()) { }

    [Obsolete]
    public ChoiceOption(ConfigEntryBase configEntry, bool restartRequired) : this(configEntry, new ChoiceConfig { restartRequired = restartRequired }) { }

    [Obsolete]
    public ChoiceOption(ConfigEntryBase configEntry, ChoiceConfig config) : this(new BepInExConfigItem(configEntry), config) { }
        
    public ChoiceOption(IConfigItem<object> configItem) : this(configItem, new ChoiceConfig()) { }

    public ChoiceOption(IConfigItem<object> configItem, bool restartRequired) : this(configItem, new ChoiceConfig { restartRequired = restartRequired }) { }
    
    public ChoiceOption(IConfigItem<object> configItem, ChoiceConfig config)
    {
        if (!configItem.ValueType.IsEnum)
            throw new InvalidCastException($"T in IConfigItem<T> must be of type Enum, Type found: {configItem.ValueType.Name}");
        
        ConfigItem = configItem;
        this.config = config;
    }
    
    public override IConfigItem BaseConfigItem => ConfigItem;
    
    internal override void RegisterTokens()
    {
        base.RegisterTokens();
        RegisterChoiceTokens();
    }

    internal void RegisterChoiceTokens()
    {
        string[] names = Enum.GetNames(Value.GetType());

        _nameTokens = new string[names.Length];

        for (int i = 0; i < names.Length; i++)
        {
            var token = $"{ModSettingsManager.StartingText}.{ModGuid}.{Category}.{Name}.item.{names[i]}".Replace(" ", "_").ToUpper();

            _nameTokens[i] = token;
                
            LanguageApi.Add(token, names[i]);
        }
    }

    public override GameObject CreateOptionGameObject(GameObject prefab, Transform parent)
    {
        var button = Object.Instantiate(prefab, parent);

        var controller = button.GetComponentInChildren<ModSettingsEnumDropDown>();

        controller.nameToken = GetNameToken();
        controller.settingToken = Id;
            
        button.name = $"Mod Option Choice, {Name}";

        return button;
    }

    public override BaseOptionConfig GetConfig() => config;
    
    public object DefaultValue => ConfigItem.DefaultValue;

    public virtual object Value
    {
        get => ConfigItem.Value;
        set => ConfigItem.Value = Enum.Parse(ConfigItem.ValueType, value.ToString());
    }

    internal string[] GetNameTokens()
    {
        return _nameTokens;
    }
}