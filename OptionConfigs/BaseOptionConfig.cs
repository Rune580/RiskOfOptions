namespace RiskOfOptions.OptionConfigs
{
    public class BaseOptionConfig
    {
        public string name = "";
        public string description = "";
        public string category = "";
        public bool restartRequired = false;
        public bool hidden = false;
        public IsDisabledDelegate checkIfDisabled;

        public delegate bool IsDisabledDelegate();
    }
}