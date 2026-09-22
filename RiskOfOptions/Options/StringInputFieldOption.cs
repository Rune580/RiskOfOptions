using System;
using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.Config;
using RiskOfOptions.OptionConfigs;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RiskOfOptions.Options;

public class StringInputFieldOption : BaseOption, IConfigItemOption<string>
{
    public IConfigItem<string> ConfigItem { get; }

    public string InitialValue { get; }

    protected readonly InputFieldConfig config;
    
    [Obsolete]
    public StringInputFieldOption(ConfigEntry<string> configEntry) : this(configEntry, new InputFieldConfig()) { }
    
    [Obsolete]
    public StringInputFieldOption(ConfigEntry<string> configEntry, bool restartRequired) : this(configEntry, new InputFieldConfig { restartRequired =  restartRequired }) { }
    
    [Obsolete]
    public StringInputFieldOption(ConfigEntry<string> configEntry, InputFieldConfig config) : this(new BepInExConfigItem<string>(configEntry), config) { }
    
    public StringInputFieldOption(IConfigItem<string> configItem) : this(configItem, new InputFieldConfig { restartRequired = configItem.Flags.HasFlag(ConfigItemFlags.RestartRequired) }) { }
    
    public StringInputFieldOption(IConfigItem<string> configItem, InputFieldConfig config)
    {
        ConfigItem = configItem;
        this.config = config;

        InitialValue = ConfigItem.Value;
    }

    public override IConfigItem BaseConfigItem => ConfigItem;

    public override GameObject CreateOptionGameObject(GameObject prefab, Transform parent)
    {
        GameObject button = Object.Instantiate(prefab, parent);

        var controller = button.GetComponentInChildren<InputFieldController>();

        controller.nameToken = GetNameToken();
        controller.optionId = Id;

        controller.submitOn = config.submitOn;
        controller.lineType = config.lineType;
        controller.richText = config.richText;
        controller.characterValidation = TMP_InputField.CharacterValidation.None;
            
        button.name = $"Mod Option Input Field, {Name}";

        return button;
    }

    public override BaseOptionConfig GetConfig() => config;
    
    public string DefaultValue => ConfigItem.DefaultValue;
    
    public virtual string Value
    {
        get => ConfigItem.Value;
        set => ConfigItem.Value = value;
    }
}