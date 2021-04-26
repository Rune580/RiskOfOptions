using System;
using System.Collections.Generic;
using System.Text;
using BepInEx.Configuration;
using RiskOfOptions.Events;
using RiskOfOptions.Interfaces;
using RiskOfOptions.OptionComponents;
using RiskOfOptions.OptionOverrides;
using RoR2.UI;
using UnityEngine;
using UnityEngine.Events;

using static RiskOfOptions.ExtensionMethods;

namespace RiskOfOptions.Options
{
    public class CheckBoxOption : RiskOfOption, IBoolProvider
    {
        public BoolEvent OnValueChanged { get; set; } = new BoolEvent();

        private bool _value;

        internal ConfigEntry<bool> ConfigEntry;

        internal CheckBoxOption(string modGuid, string modName, string name, object[] description, string defaultValue, string categoryName, OptionOverride optionOverride, bool visibility, bool restartRequired, UnityAction<bool> unityAction, bool invokeEventOnStart)
            : base(modGuid, modName, name, description, defaultValue, categoryName, optionOverride, visibility, restartRequired, invokeEventOnStart)
        {
            Value = (int.Parse(defaultValue) == 1);

            if (unityAction != null)
            {
                OnValueChanged.AddListener(unityAction);
            }

            OptionToken = $"{ModSettingsManager.StartingText}.{modGuid}.{modName}.category_{categoryName.Replace(".", "")}.{name}.checkbox".ToUpper().Replace(" ", "_");

            RegisterTokens();
        }

        public bool Value
        {
            get
            {
                if (OptionOverride != null)
                {
                    Indexes indexes = ModSettingsManager.OptionContainers.GetIndexes(ModGuid, OptionOverride.Name, OptionOverride.CategoryName);

                    if (ModSettingsManager.OptionContainers[indexes.ContainerIndex].GetModOptionsCached()[indexes.OptionIndexInContainer] is IBoolProvider boolProvider)
                    {
                        bool overrideValue = boolProvider.Value;

                        if ((overrideValue && OptionOverride.OverrideOnTrue) || (!overrideValue && !OptionOverride.OverrideOnTrue))
                        {
                            return ((CheckBoxOverride)OptionOverride).ValueToReturnWhenOverriden;
                        }
                    }
                }

                return _value;
            }
            set
            {
                if (_value == value)
                    return;

                _value = value;

                if (ConfigEntry != null)
                {
                    ConfigEntry.Value = Value;
                }

                OnValueChanged.Invoke(Value);
            }
        }

        public override GameObject CreateOptionGameObject(RiskOfOption option, GameObject prefab, Transform parent)
        {
            GameObject button = GameObject.Instantiate(prefab, parent);

            var controller = button.GetComponentInChildren<CarouselController>();

            controller.settingName = option.ConsoleToken;
            controller.nameToken = option.NameToken;

            controller.settingSource = RooSettingSource;

            button.name = $"Mod Option CheckBox, {option.Name}";

            return button;
        }

        public override string GetValueAsString()
        {
            return $"{(Value ? "1" : "0")}";
        }

        public override string GetInternalValueAsString()
        {
            return $"{(_value ? "1" : "0")}";
        }

        public override void SetValue(string newValue)
        {
            Value = (int.Parse(newValue) == 1);
        }

        public override void SetInternalValue(string newValue)
        {
            _value = (int.Parse(newValue) == 1);
        }

        public override T GetValue<T>()
        {
            if (typeof(T) != typeof(bool))
            {
                throw new Exception($"{Name} can only return a bool! {typeof(T)} is not a valid return type!");
            }

            return (T) Convert.ChangeType(Value, typeof(T));
        }

        internal override void Invoke<T>(T value)
        {
            OnValueChanged.Invoke((bool) Convert.ChangeType(value, typeof(bool)));
        }

        internal override void Invoke()
        {
            OnValueChanged.Invoke(Value);
        }
    }
}
