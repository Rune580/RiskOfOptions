using System.Linq;
using BepInEx.Configuration;
using UnityEngine;

namespace RiskOfOptions
{
    public static class KeyboardShortcutExtensions
    {
        public static bool IsPressedInclusive(this KeyboardShortcut key)
        {
            bool modifiers = key.Modifiers.All(Input.GetKey);

            return Input.GetKey(key.MainKey) && modifiers;
        }
    }
}