using System;
using System.Collections.Generic;
using System.Reflection;
using MonoMod.RuntimeDetour;
using RoR2;

namespace RiskOfOptions.Lib
{
    // Todo more in-depth language api system
    internal static class LanguageApi
    {
        private static readonly Dictionary<string, string> LanguageEntries = new();
        private static readonly Dictionary<string, LanguageStringDelegate> DynamicLanguageEntries = new();

        private static Hook _languageHook;

        internal static void Init()
        {
            var destMethod = typeof(LanguageApi).GetMethod(nameof(GetLocalizedStringByToken), BindingFlags.NonPublic | BindingFlags.Static);
            _languageHook = HookHelper.NewHook<Language>("GetLocalizedStringByToken", destMethod);
        }

        internal static void Add(string token, string entry)
        {
            LanguageEntries[token] = entry;
        }

        internal static void AddDelegate(string token, LanguageStringDelegate stringDelegate)
        {
            DynamicLanguageEntries[token] = stringDelegate;
        }

        internal static void RemoveDelegate(string token)
        {
            DynamicLanguageEntries.Remove(token);
        }
        
        private static string GetLocalizedStringByToken(Func<Language, string, string> orig, Language self, string token)
        {
            if (token.Length > ModSettingsManager.StartingTextLength)
            {
                if (token.Substring(0, ModSettingsManager.StartingTextLength) != ModSettingsManager.StartingText)
                    return orig(self, token);
            
                if (LanguageEntries.TryGetValue(token, out var localizedString))
                    return localizedString;
                
                if (DynamicLanguageEntries.TryGetValue(token, out var action))
                    return action.Invoke();
            }

            return orig(self, token);
        }

        internal delegate string LanguageStringDelegate();
    }
}