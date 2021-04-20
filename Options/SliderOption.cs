using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using BepInEx.Configuration;
using RiskOfOptions.Interfaces;
using RiskOfOptions.OptionOverrides;
using RoR2.UI;
using UnityEngine;
using UnityEngine.Events;

using static RiskOfOptions.ExtensionMethods;

namespace RiskOfOptions.Options
{
    public class SliderOption : RiskOfOption, IFloatProvider
    {
        public List<UnityAction<float>> Events { get; set; } = new List<UnityAction<float>>();

        private float _value;

        public float Min;
        public float Max;

        internal ConfigEntry<float> configEntry;

        internal SliderOption(string modGuid, string modName, string name, object[] description, string defaultValue, float min, float max, string categoryName, OptionOverride optionOverride, bool visibility, UnityAction<float> unityAction, bool invokeEventOnStart)
            : base(modGuid, modName, name, description, defaultValue, categoryName, optionOverride, visibility, false, invokeEventOnStart)
        {
            Min = min;
            Max = max;

            if (unityAction != null)
            {
                Events.Add(unityAction);
            }

            Value = float.Parse(defaultValue, CultureInfo.InvariantCulture);

            OptionToken = $"{ModSettingsManager.StartingText}.{modGuid}.{modName}.category_{categoryName.Replace(".", "")}.{name}.slider".ToUpper().Replace(" ", "_");
            RegisterTokens();
        }

        public float Value
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
                            return ((SliderOverride)OptionOverride).ValueToReturnWhenOverriden;
                        }
                    }
                }

                return _value;
            }
            set
            {
                _value = value;

                if (configEntry != null)
                {
                    configEntry.Value = Value;
                }

                InvokeListeners();
            }

        }

        public override GameObject CreateOptionGameObject(RiskOfOption option, GameObject prefab, Transform parent)
        {
            GameObject button = GameObject.Instantiate(prefab, parent);

            var slider = button.GetComponentInChildren<SettingsSlider>();

            slider.settingName = option.ConsoleToken;
            slider.nameToken = option.NameToken;

            slider.settingSource = RooSettingSource;

            slider.minValue = Min;
            slider.maxValue = Max;

            button.name = $"Mod Option Slider, {option.Name}";

            return button;
        }

        public override void InvokeListeners()
        {
            foreach (var action in Events)
            {
                action.Invoke(Value);
            }
        }

        public void InvokeListeners(float value)
        {
            foreach (var action in Events)
            {
                action.Invoke(value);
            }
        }

        public override string GetValueAsString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public override string GetInternalValueAsString()
        {
            return _value.ToString(CultureInfo.InvariantCulture);
        }

        public override void SetValue(string newValue)
        {
            Value = float.Parse(newValue);
        }

        public override T GetValue<T>()
        {
            if (typeof(T) != typeof(float))
            {
                throw new Exception($"{Name} can only return a float! {typeof(T)} is not a valid return type!");
            }

            return (T)Convert.ChangeType(Value, typeof(T));
        }
    }
}
