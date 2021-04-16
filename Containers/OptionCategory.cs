using System.Collections.Generic;
using System.Linq;
using R2API;
using RiskOfOptions.Options;

namespace RiskOfOptions.Containers
{
    /// <summary>
    /// 
    /// </summary>
    public class OptionCategory : OptionBase
    {
        /// <summary>
        /// A list of options.
        /// These are passed by reference.
        /// </summary>
        private readonly OptionStorage _options;


        public OptionCategory(string categoryName, string modGuid)
        {
            this.Name = categoryName;
            this.ModGuid = modGuid;

            _options = new OptionStorage();


            OptionToken = $"{ModSettingsManager.StartingText}.{modGuid}.{(categoryName != "" ? "category_" + categoryName + "." : categoryName)}{Name}.CATEGORY".ToUpper().Replace(" ", "_");


            NameToken = $"{OptionToken}.NAME_TOKEN";

            LanguageAPI.Add(NameToken, Name);
        }

        internal List<RiskOfOption> GetModOptionsCached()
        {
            return _options.GetModOptionsCached();
        }

        public void Add(ref OptionBase option)
        {
            _options.Add(ref option);
        }

        public void Add(ref RiskOfOption option)
        {
            _options.Add(ref option);
        }
    }
}
