using System.Collections.Generic;
using On.RoR2;

namespace RiskOfOptions.Lib
{
    // Todo more in-depth language api system
    internal static class LanguageApi
    {
        private static readonly Dictionary<string, string> LanguageEntries = new();

        internal static void Init()
        {
            Language.GetLocalizedStringByToken += GetLocalizedStringByToken;
        }

        internal static void Add(string token, string entry)
        {
            LanguageEntries[token] = entry;
        }
        
        private static string GetLocalizedStringByToken(Language.orig_GetLocalizedStringByToken orig, RoR2.Language self, string token)
        {
            return LanguageEntries.TryGetValue(token, out string localizedString) ? localizedString : orig(self, token);
        }
    }
}