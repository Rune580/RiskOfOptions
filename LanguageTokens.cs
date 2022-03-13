﻿using System;
using System.Collections.Generic;
using System.Text;
using R2API;

namespace RiskOfOptions
{
    internal static class LanguageTokens
    {
        public static readonly string OptionRebindDialogTitle =
            $"{ModSettingsManager.StartingText.ToUpper()}_OPTION_REBIND_DIALOG_TITLE_TOKEN";

        public static readonly string OptionRebindDialogDescription =
            $"{ModSettingsManager.StartingText.ToUpper()}_OPTION_REBIND_DIALOG_DESCRIPTION_TOKEN";

        public static readonly string LeftPageButton =
            $"{ModSettingsManager.StartingText.ToUpper()}_LEFT_PAGE_BUTTON_TEXT_TOKEN;";

        public static readonly string RightPageButton =
            $"{ModSettingsManager.StartingText.ToUpper()}_RIGHT_PAGE_BUTTON_TEXT_TOKEN;";
        

        public const string HeaderToken = "RISK_OF_OPTIONS_MOD_OPTIONS_HEADER_BUTTON_TEXT";

        public static void Register()
        {
            LanguageAPI.Add(OptionRebindDialogTitle, "Rebind Control...");
            LanguageAPI.Add(LeftPageButton, "<");
            LanguageAPI.Add(RightPageButton, ">");
        }
    }
}
