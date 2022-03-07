using BepInEx.Configuration;

namespace RiskOfOptions.OptionConstructors
{
    public class StepSlider : Slider
    {
        public float Increment;

        public StepSlider(ConfigEntry<float> configEntry) : base(configEntry)
        {
            Increment = 1f;
            DisplayAsPercentage = false;
        }
    }
}
