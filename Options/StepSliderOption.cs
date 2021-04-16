using System;
using System.Collections.Generic;
using System.Text;
using RiskOfOptions.OptionOverrides;

namespace RiskOfOptions.Options
{
    public class StepSliderOption : SliderOption
    {
        public float Increment;

        internal StepSliderOption(string modGuid, string modName, string name, object[] description, string defaultValue, float min, float max, float increment, string categoryName, OptionOverride optionOverride, bool visibility, bool restartRequired)
            : base(modGuid, modName, name, description, defaultValue, min, max, categoryName, optionOverride, visibility, restartRequired)
        {
            Increment = increment;

            if ((min - max) % increment != 0)
            {
                throw new Exception($"Cannot make Stepped Slider with increment of {increment}!");
            }

            OptionToken = $"{ModSettingsManager.StartingText}.{modGuid}.{modName}.category_{categoryName.Replace(".", "")}.{name}.stepslider".ToUpper().Replace(" ", "_");

            RegisterTokens();
        }
    }
}
