using RiskOfOptions.Components.Panel;
using RiskOfOptions.Options;
using RoR2.UI;
using UnityEngine;

namespace RiskOfOptions.Components.Options
{
    public abstract class ModSetting : MonoBehaviour
    {
        protected static DecimalSeparator Separator => RiskOfOptionsPlugin.decimalSeparator!.Value;
        
        public string nameToken;
        public string settingToken;
        public LanguageTextMeshController nameLabel;
        public ModOptionPanelController optionController;

        protected BaseOption option;

        protected virtual void Awake()
        {
            if (nameLabel && !string.IsNullOrEmpty(nameToken))
                nameLabel.token = nameToken;
            
            if (string.IsNullOrEmpty(settingToken))
                return;

            option = ModSettingsManager.OptionCollection.GetOption(settingToken);
        }

        protected virtual void Start()
        {
            if (nameLabel && !string.IsNullOrEmpty(nameToken))
                nameLabel.token = nameToken;
        }

        public abstract bool HasChanged();

        public abstract void Revert();

        public abstract void CheckIfDisabled();

        public virtual void UpdateModifiedIndicator() { }
        
        protected abstract void Disable();

        protected abstract void Enable();
    }
}