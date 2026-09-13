using System;
using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.Config;
using RiskOfOptions.OptionConfigs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RiskOfOptions.Options;

public class ColorOption : BaseOption, IConfigItemOption<Color>
{
    public IConfigItem<Color> ConfigItem { get; }
    
    protected readonly ColorOptionConfig config;
    
    [Obsolete]
    public ColorOption(ConfigEntry<Color> configEntry) : this(configEntry, new ColorOptionConfig()) { }
    
    [Obsolete]
    public ColorOption(ConfigEntry<Color> configEntry, bool restartRequired) : this(configEntry, new ColorOptionConfig { restartRequired = true }) { }
    
    [Obsolete]
    public ColorOption(ConfigEntry<Color> configEntry, ColorOptionConfig config) : this(new BepInExConfigItem<Color>(configEntry), config) { }
    
    public ColorOption(IConfigItem<Color> configItem) : this(configItem, new ColorOptionConfig()) { }
        
    public ColorOption(IConfigItem<Color> configItem, bool restartRequired) : this(configItem, new ColorOptionConfig { restartRequired = true }) { }
    
    public ColorOption(IConfigItem<Color> configItem, ColorOptionConfig config)
    {
        ConfigItem = configItem;
        this.config = config;
    }

    public override IConfigItem BaseConfigItem => ConfigItem;

    public override GameObject CreateOptionGameObject(GameObject prefab, Transform parent)
    {
        GameObject button = Object.Instantiate(prefab, parent);

        var controller = button.GetComponentInChildren<ModSettingsColor>();

        controller.nameToken = GetNameToken();
        controller.settingToken = Identifier;

        button.name = $"Mod Option Color, {Name}";

        return button;
    }

    public override BaseOptionConfig GetConfig() => config;
    
    public Color DefaultValue => ConfigItem.DefaultValue;
    
    public virtual Color Value
    {
        get => ConfigItem.Value;
        set => ConfigItem.Value = value;
    }
}