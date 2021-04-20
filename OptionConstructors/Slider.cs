using System.Globalization;
using BepInEx.Configuration;
using RiskOfOptions.OptionOverrides;
using UnityEngine.Events;

namespace RiskOfOptions.OptionConstructors
{
    public class Slider : OptionConstructorBase
    {

        public float DefaultValue
        {
            set => this.value = value.ToString(CultureInfo.InvariantCulture);
        }
        public float Min;
        public float Max;
        public bool DisplayAsPercentage = true;
        public SliderOverride Override;
        public UnityAction<float> OnValueChanged;
        public ConfigEntry<float> ConfigEntry;
        public Slider()
        {
            ConfigEntry = null;
            DefaultValue = 0;
            Min = 0;
            Max = 100;
            Override = null;
        }
    }
}
