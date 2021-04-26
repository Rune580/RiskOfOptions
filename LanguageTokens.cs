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

        public static string LeftPageButton =
            $"{ModSettingsManager.StartingText.ToUpper()}_LEFT_PAGE_BUTTON_TEXT_TOKEN;";

        public static string RightPageButton =
            $"{ModSettingsManager.StartingText.ToUpper()}_RIGHT_PAGE_BUTTON_TEXT_TOKEN;";

        public static string OptionInputField =
            $"{ModSettingsManager.StartingText.ToUpper()}_OPTION_INPUT_FIELD_TEXT_TOKEN";

        public static void Register()
        {
            LanguageAPI.Add(OptionRebindDialogTitle, "Rebind Control...");
            LanguageAPI.Add(LeftPageButton, "<");
            LanguageAPI.Add(RightPageButton, ">");
        }
    }
}
