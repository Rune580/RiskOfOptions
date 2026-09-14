using RiskOfOptions.Components.Options;
using RiskOfOptions.Config;
using RiskOfOptions.Lib;
using RiskOfOptions.OptionConfigs;
using UnityEngine;
using UnityEngine.Events;

namespace RiskOfOptions.Options;

public class GenericButtonOption : BaseOption
{
    internal readonly GenericButtonConfig config;

    public GenericButtonOption(string name, string category, UnityAction onButtonPressed) : this(
        name, category, "", "Open", onButtonPressed) { }

    public GenericButtonOption(string name, string category, string description, string buttonText, UnityAction onButtonPressed)
    {
        config = new GenericButtonConfig(name, category, description, buttonText, onButtonPressed);

        Category = category;
        Name = name;
        Description = description;
    }

    public override IConfigItem? BaseConfigItem => null;

    internal override void RegisterTokens()
    {
        base.RegisterTokens();
        
        LanguageApi.Add(GetButtonLabelToken(), config.ButtonText);
    }

    public string GetButtonLabelToken()
    {
        return $"{ModSettingsManager.StartingText}.{ModGuid}.{Category}.{Name}.sub_button.name".Replace(" ", "_").ToUpper();
    }
    
    public override GameObject CreateOptionGameObject(GameObject prefab, Transform parent)
    {
        GameObject button = Object.Instantiate(prefab, parent);

        var controller = button.GetComponentInChildren<GenericButtonController>();

        controller.nameToken = GetNameToken();
        controller.settingToken = Id;
        controller.buttonToken = GetButtonLabelToken();
        controller.OnButtonPressed = config.OnButtonPressed;

        button.name = $"Mod Option GenericButton, {Name}";

        return button;
    }

    public override BaseOptionConfig GetConfig() => config;
}