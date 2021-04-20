namespace RiskOfOptions.OptionConstructors
{
    public class StepSlider : Slider
    {
        public float Increment;

        public StepSlider()
        {
            Increment = 1f;
            DisplayAsPercentage = false;
        }
    }
}
