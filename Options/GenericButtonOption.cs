using R2API;
using RiskOfOptions.Components.Options;
using RiskOfOptions.OptionConfigs;
using UnityEngine;
using UnityEngine.Events;

namespace RiskOfOptions.Options
{
    public class GenericButtonOption : BaseOption
    {
        private GenericButtonConfig Config { get; }

        public GenericButtonOption(string name, string category, UnityAction onButtonPressed) : this(
            name, category, "", "Open", onButtonPressed) { }

        public GenericButtonOption(string name, string category, string description, string buttonText, UnityAction onButtonPressed)
        {
            Config = new GenericButtonConfig(name, category, description, buttonText, onButtonPressed);

            Category = category;
            Name = name;
            Description = description;
        }

        internal override void RegisterTokens()
        {
            base.RegisterTokens();
            
            LanguageAPI.Add(GetButtonLabelToken(), Config.ButtonText);
        }

        public string GetButtonLabelToken()
        {
            return $"{ModSettingsManager.StartingText}.{ModGuid}.{Category}.{Name}.{OptionTypeName}.sub_button.name".Replace(" ", "_").ToUpper();
        }
        
        public override string OptionTypeName { get; protected set; } = "generic_button";

        public override GameObject CreateOptionGameObject(GameObject prefab, Transform parent)
        {
            GameObject button = Object.Instantiate(prefab, parent);

            var controller = button.GetComponentInChildren<GenericButtonController>();

            controller.nameToken = GetNameToken();
            controller.settingToken = Identifier;
            controller.buttonToken = GetButtonLabelToken();
            controller.OnButtonPressed = Config.OnButtonPressed;

            button.name = $"Mod Option GenericButton, {Name}";

            return button;
        }

        public override BaseOptionConfig GetConfig()
        {
            return Config;
        }

        public override bool ValueChanged()
        {
            return false;
        }
    }
}