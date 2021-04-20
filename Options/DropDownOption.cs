using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using R2API;
using RiskOfOptions.Interfaces;
using RiskOfOptions.OptionOverrides;
using RiskOfOptions.Structs;
using UnityEngine;
using UnityEngine.Events;

namespace RiskOfOptions.Options
{
    public class DropDownOption : RiskOfOption, IIntProvider
    {
        public UnityAction<int> OnValueChangedChoice { get; set; }

        private int _value;

        internal DropDownOption(string modGuid, string modName, string name, object[] description, string defaultValue, string categoryName, string[] choices, bool visibility, bool restartRequired, UnityAction<int> unityAction, bool invokeValueChangedEvent)
            : base(modGuid, modName, name, description, defaultValue, categoryName, null, visibility, restartRequired, invokeValueChangedEvent)
        {
            Value = (int) int.Parse(defaultValue, CultureInfo.InvariantCulture);

            OnValueChangedChoice = unityAction;

            OptionToken = $"{ModSettingsManager.StartingText}.{modGuid}.{modName}.category_{categoryName.Replace(".", "")}.{name}.dropdown".ToUpper().Replace(" ", "_");

            RegisterTokens();
            RegisterChoiceTokens(choices);
        }

        private void RegisterChoiceTokens(string[] choices)
        {
            foreach (var choice in choices)
            {
                string token = $"{OptionToken}_choice-{choice}".ToUpper().Replace(" ", "_");
                LanguageAPI.Add(token, choice);
            }
        }

        public int Value
        {
            get => _value;
            set => _value = value;
        }

        public override GameObject CreateOptionGameObject(RiskOfOption option, GameObject prefab, Transform parent)
        {
            return base.CreateOptionGameObject(option, prefab, parent);
        }

        public override string GetValueAsString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public override string GetInternalValueAsString()
        {
            return _value.ToString(CultureInfo.InvariantCulture);
        }

        public override void SetValue(string newValue)
        {
            Value = int.Parse(newValue, CultureInfo.InvariantCulture);
        }

        public override T GetValue<T>()
        {
            if (typeof(T) != typeof(int))
            {
                throw new Exception($"{Name} can only return a int! {typeof(T)} is not a valid return type!");
            }

            return (T)Convert.ChangeType(Value, typeof(T));
        }
    }
}
