using System;
using System.Collections.Generic;
using System.Text;
using RiskOfOptions.OptionComponents;
using RiskOfOptions.OptionOverrides;
using RoR2.UI;
using UnityEngine;

namespace RiskOfOptions.Options
{
    public class StepSliderOption : SliderOption
    {
        public float Increment;

        internal StepSliderOption(string modGuid, string modName, string name, object[] description, string defaultValue, float min, float max, float increment, string categoryName, OptionOverride optionOverride, bool visibility, bool restartRequired)
            : base(modGuid, modName, name, description, defaultValue, min, max, categoryName, optionOverride, visibility, restartRequired)
        {
            Increment = increment;

            double stepsHighAccuracy = (Math.Abs(min - max) / increment);

            //int steps = (int)Math.Round(stepsHighAccuracy);

            //Debug.Log($"Difference: {Math.Abs(min - max)}, amount of steps: {steps}, remainder: {stepsHighAccuracy - Math.Truncate(stepsHighAccuracy)}");

            if (stepsHighAccuracy - Math.Truncate(stepsHighAccuracy) != 0)
            {
                if (stepsHighAccuracy - Math.Truncate(stepsHighAccuracy) < 0.9999) // Checking if accuracy error or actually invalid.
                {
                    throw new Exception($"Cannot make Stepped Slider with increment of {increment}! Causes a remainder of {stepsHighAccuracy - Math.Truncate(stepsHighAccuracy)}");
                }
            }

            OptionToken = $"{ModSettingsManager.StartingText}.{modGuid}.{modName}.category_{categoryName.Replace(".", "")}.{name}.stepslider".ToUpper().Replace(" ", "_");

            RegisterTokens();
        }

        public override GameObject CreateOptionGameObject(RiskOfOption option, GameObject prefab, Transform parent)
        {
            GameObject button = GameObject.Instantiate(prefab, parent);

            GameObject.DestroyImmediate(button.GetComponent<SettingsSlider>());

            var stepSlider = button.AddComponent<SettingsStepSlider>();

            stepSlider.settingName = option.ConsoleToken;
            stepSlider.nameToken = option.NameToken;

            stepSlider.settingSource = RooSettingSource;

            if (OnValueChangedFloat != null)
                stepSlider.AddListener(OnValueChangedFloat);

            stepSlider.minValue = 0;
            stepSlider.maxValue = 100;

            stepSlider.internalMinValue = Min;
            stepSlider.internalMaxValue = Max;
            stepSlider.increment = Increment;

            button.name = $"Mod Option Step Slider, {option.Name}";

            return button;
        }
    }
}
