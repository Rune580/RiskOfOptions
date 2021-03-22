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

        public OptionCategory(string CategoryName, string ModGUID, string Description = "")
        {
            this.Name = CategoryName;
            this.ModGUID = ModGUID;

            Options = new List<OptionBase>();

            OptionToken = $"{ModSettingsManager.StartingText}.{ModGUID}.{(CategoryName != "" ? "category_" + CategoryName + "." : CategoryName)}{Name}.CATEGORY".ToUpper().Replace(" ", "_");


            NameToken = $"{OptionToken}.NAME_TOKEN";

            DescriptionToken = $"{OptionToken}.DESCRIPTION_TOKEN";

            LanguageAPI.Add(NameToken, Name);

            if (Description != "")
            {
                LanguageAPI.Add(NameToken, Description);
            }
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
