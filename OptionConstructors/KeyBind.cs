using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.Events;

namespace RiskOfOptions.OptionConstructors
{
    public class KeyBind : OptionConstructorBase
    {
        public KeyCode DefaultValue
        {
            set => this.value = $"{(int) value}";
        }

        public UnityAction<KeyCode> OnValueChanged;

        public ConfigEntry<KeyboardShortcut> ConfigEntry;

        public KeyBind()
        {
            ConfigEntry = null;
            OnValueChanged = null;
            DefaultValue = KeyCode.None;
        }
    }
}
