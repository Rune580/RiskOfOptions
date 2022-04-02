using RiskOfOptions.Lib;

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
        public const string NoModsHeaderToken = "RISK_OF_OPTIONS_NO_MODS_HEADER_TEXT";
        public const string NoModsDescriptionToken = "RISK_OF_OPTIONS_NO_MODS_DESCRIPTION_TEXT";
        public const string ModsHeaderToken = "RISK_OF_OPTIONS_MODS_HEADER_TEXT";
        public const string ModsDescriptionToken = "RISK_OF_OPTIONS_DESCRIPTION_TEXT";
        public const string DialogButtonToken = "RISK_OF_OPTIONS_NO_MODS_BUTTON_TEXT";

        public static void Register()
        {
            LanguageApi.Add(OptionRebindDialogTitle, "Rebind Control...");
            LanguageApi.Add(LeftPageButton, "<");
            LanguageApi.Add(RightPageButton, ">");
            LanguageApi.Add(NoModsHeaderToken, "No Supported Mods Installed");
            LanguageApi.Add(NoModsDescriptionToken, "No mods implementing RiskOfOptions found.\n" +
                                                    "This mod doesn't do anything if you don't have any mods installed that supports RiskOfOptions.\n" +
                                                    "This won't show again.");
            LanguageApi.Add(ModsHeaderToken, "You can configure your mods in game!");
            LanguageApi.Add(ModsDescriptionToken, "Mods with support for RiskOfOptions found!\n" +
                                                  "You can configure them in the \"MOD OPTIONS\" panel.\n" +
                                                  "This won't show again");
            LanguageApi.Add(DialogButtonToken, "Ok");
        }
    }
}
