using System;
using System.Collections.Generic;
using System.Text;

namespace RiskOfOptions
{
    public class OptionBase
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

        public string OptionToken { get; protected set; }

        public string ConsoleToken { get; protected set; }

        public string ModGUID { get; protected set; }
        public string ModName { get; protected set; }
    }
}
