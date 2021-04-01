using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using R2API;

namespace RiskOfOptions
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
        public List<OptionBase> Options;

        private List<RiskOfOption> _modOptions;

        private int _lastAmount = 0;

        public OptionCategory(string categoryName, string modGuid)
        {
            this.Name = categoryName;
            this.ModGuid = modGuid;

            Options = new List<OptionBase>();

            OptionToken = $"{ModSettingsManager.StartingText}.{modGuid}.{(categoryName != "" ? "category_" + categoryName + "." : categoryName)}{Name}.CATEGORY".ToUpper().Replace(" ", "_");


            NameToken = $"{OptionToken}.NAME_TOKEN";

            DescriptionToken = $"{OptionToken}.DESCRIPTION_TOKEN";

            LanguageAPI.Add(NameToken, Name);
        }

        internal List<RiskOfOption> GetModOptionsCached()
        {
            if (_modOptions != null && Options.Count == _lastAmount)
                return _modOptions;


            _modOptions = Options.Where(a => a.GetType() == typeof(RiskOfOption)).Cast<RiskOfOption>().ToList();

            _lastAmount = Options.Count;


            return _modOptions;
        }

        public void Add(ref OptionBase option)
        {
            Options.Add(option);
        }

        public void Add(ref RiskOfOption option)
        {
            Options.Add(option);
        }
    }
}
