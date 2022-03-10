using System;

namespace RiskOfOptions.OptionConfigs
{
    public class BaseOptionConfig
    {
        public string name = "";
        public string description = "";
        public string category = "";
        [Obsolete("Not yet implemented")]
        public bool restartRequired = false;
        [Obsolete("Not yet implemented")]
        public bool hidden = false;
        public IsDisabledDelegate checkIfDisabled;

        public delegate bool IsDisabledDelegate();
    }
}