namespace RiskOfOptions.OptionConfigs
{
    public class InputFieldConfig : BaseOptionConfig
    {
        public SubmitEnum submitOn = SubmitEnum.OnChar;
        
        public enum SubmitEnum
        {
            OnChar,
            OnExitOrSubmit,
            OnSubmit
        }
    }
}