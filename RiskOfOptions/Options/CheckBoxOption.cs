using System;
using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.Config;
using RiskOfOptions.OptionConfigs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RiskOfOptions.Options;

public class CheckBoxOption : BaseOption, IConfigItemOption<bool>
{
    public IConfigItem<bool> ConfigItem { get; }

    public bool InitialValue { get; }

    protected readonly CheckBoxConfig config;

    [Obsolete]
    public CheckBoxOption(ConfigEntry<bool> configEntry) : this(configEntry, new CheckBoxConfig()) { }
    
    [Obsolete]
    public CheckBoxOption(ConfigEntry<bool> configEntry, bool restartRequired) : this(configEntry, new CheckBoxConfig { restartRequired = restartRequired }) { }

    [Obsolete]
    public CheckBoxOption(ConfigEntry<bool> configEntry, CheckBoxConfig config) : this(new BepInExConfigItem<bool>(configEntry), config) { }
        
    public CheckBoxOption(IConfigItem<bool> configItem) : this(configItem, new CheckBoxConfig()) { }
        
    public CheckBoxOption(IConfigItem<bool> configItem, bool restartRequired) : this(configItem, new CheckBoxConfig { restartRequired = restartRequired }) { }
    
    public CheckBoxOption(IConfigItem<bool> configItem, CheckBoxConfig config)
    {
        ConfigItem = configItem;
        this.config = config;
        InitialValue = ConfigItem.Value;
    }

    public override IConfigItem BaseConfigItem => ConfigItem;

    public override GameObject CreateOptionGameObject(GameObject prefab, Transform parent)
    {
        GameObject button = Object.Instantiate(prefab, parent);

        var controller = button.GetComponentInChildren<ModSettingsBool>();

        controller.nameToken = GetNameToken();
        controller.optionId = Id;
            
        button.name = $"Mod Option CheckBox, {Name}";

        return button;
    }
        
    public override BaseOptionConfig GetConfig() => config;

    public virtual bool Value
    {
        get => ConfigItem.Value; 
        set => ConfigItem.Value = value;
    }
}