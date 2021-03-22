using System;
using System.Collections.Generic;
using System.Text;

namespace RiskOfOptions
{
    /// <summary>
    /// Store options from a specific mod in one container.
    /// 
    /// Probably doesn't need to be an object tbh.
    /// </summary>
    internal class OptionContainer
    {
        public string ModGUID { get; private set; }

        public List<OptionBase> Options;

        public OptionContainer(string ModGUID)
        {
            this.ModGUID = ModGUID;

            Options = new List<OptionBase>();
        }


    }
}
