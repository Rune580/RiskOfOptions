using System;
using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.Config;
using RiskOfOptions.OptionConfigs;
using RoR2.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RiskOfOptions.Options;

public class KeyBindOption : BaseOption, IConfigItemOption<KeyboardShortcut>
{
    public IConfigItem<KeyboardShortcut> ConfigItem { get; }

    public KeyboardShortcut InitialValue { get; }

    protected readonly KeyBindConfig config;
    
    [Obsolete]
    public KeyBindOption(ConfigEntry<KeyboardShortcut> configEntry) : this(configEntry, new KeyBindConfig()) { }
    
    [Obsolete]
    public KeyBindOption(ConfigEntry<KeyboardShortcut> configEntry, bool restartRequired) : this(configEntry, new KeyBindConfig { restartRequired = restartRequired }) { }

    [Obsolete]
    public KeyBindOption(ConfigEntry<KeyboardShortcut> configEntry, KeyBindConfig config) : this(new BepInExConfigItem<KeyboardShortcut>(configEntry), config) { }
    
    public KeyBindOption(IConfigItem<KeyboardShortcut> configItem) : this(configItem, new KeyBindConfig()) { }
        
    public KeyBindOption(IConfigItem<KeyboardShortcut> configItem, bool restartRequired) : this(configItem, new KeyBindConfig { restartRequired = restartRequired }) { }
    
    public KeyBindOption(IConfigItem<KeyboardShortcut> configItem, KeyBindConfig config)
    {
        ConfigItem = configItem;
        this.config = config;

        InitialValue = ConfigItem.Value;
    }

    public override IConfigItem BaseConfigItem => ConfigItem;

    public override GameObject CreateOptionGameObject(GameObject prefab, Transform parent)
    {
        GameObject keyBind = Object.Instantiate(prefab, parent);

        ModSettingsKeyBind controller = keyBind.GetComponentInChildren<ModSettingsKeyBind>();

        controller.nameToken = GetNameToken();
        controller.optionId = Id;
            
        keyBind.transform.Find("ButtonText").GetComponent<HGTextMeshProUGUI>().SetText(GetLocalizedName());
        keyBind.name = $"Mod Option KeyBind, {Name}";

        // foreach (var button in keyBind.GetComponentsInChildren<HGButton>())
        //     button.onClick.AddListener(delegate { controller.StartListening(); });

        return keyBind;
    }

    public override BaseOptionConfig GetConfig() => config;
    
    public KeyboardShortcut DefaultValue => ConfigItem.DefaultValue;
    
    public virtual KeyboardShortcut Value
    {
        get => ConfigItem.Value;
        set => ConfigItem.Value = value;
    }
}