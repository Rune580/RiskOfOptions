using System;
using System.Collections.Generic;
using System.Text;

namespace RiskOfOptions
{
    /// <summary>
    /// </summary>
    public class OptionCategory : OptionBase
    {
        /// <summary>
        /// The name displayed in the mod options menu.
        /// </summary>
        public string Name;

        /// <summary>
        /// The name token used for the LanguageAPI and for internal use.
        /// </summary>
        public string NameToken;

        /// <summary>
        /// The description displayed in the mod options menu.
        /// </summary>
        public string Description;

        /// <summary>
        /// The description token used for the LanguageAPI and for internal use.
        /// </summary>
        public string DescriptionToken;


        /// <summary>
        /// A list of options.
        /// These are passed by reference.
        /// </summary>
        public List<OptionBase> Options;
        public OptionCategory(string ModGUID)
        {
            this.ModGUID = ModGUID;

            Options = new List<OptionBase>();
        }

        public void Add(ref OptionBase option)
        {
            Options.Add(option);
        }

        public void Add(ref ModOption option)
        {
            Options.Add(option);
        }

        public void debugOptions()
        {

        }
    }
}
