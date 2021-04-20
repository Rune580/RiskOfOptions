using System;
using System.Collections.Generic;
using System.Text;
using R2API;

namespace RiskOfOptions
{
    internal static class LanguageTokens
    {
        public static string OptionRebindDialogTitle =
            $"{ModSettingsManager.StartingText.ToUpper()}_OPTION_REBIND_DIALOG_TITLE_TOKEN";

        public static string OptionRebindDialogDescription =
            $"{ModSettingsManager.StartingText.ToUpper()}_OPTION_REBIND_DIALOG_DESCRIPTION_TOKEN";

        public static void Register()
        {
            LanguageAPI.Add(OptionRebindDialogTitle, "Rebind Control...");
        }
    }
}
