using System;
using System.Collections.Generic;
using System.Text;
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
        public UnityAction<bool> OnValueChangedBool { get; set; }

        private bool _value;

        internal CheckBoxOption(string modGuid, string modName, string name, object[] description, string defaultValue, string categoryName, OptionOverride optionOverride, bool visibility, bool restartRequired)
            : base(modGuid, modName, name, description, defaultValue, categoryName, optionOverride, visibility, restartRequired)
        {
            Value = (int.Parse(defaultValue) == 1);

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
            set => _value = value;
        }

        public override GameObject CreateOptionGameObject(RiskOfOption option, GameObject prefab, Transform parent)
        {
            GameObject button = GameObject.Instantiate(prefab, parent);

            var controller = button.GetComponentInChildren<CarouselController>();

            controller.settingName = option.ConsoleToken;
            controller.nameToken = option.NameToken;

            controller.settingSource = RooSettingSource;

            if (OnValueChangedBool != null)
            {
                button.AddComponent<BoolListener>().AddListener(OnValueChangedBool);
            }


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

        public override bool GetBool()
        {
            return Value;
        }
    }
}
