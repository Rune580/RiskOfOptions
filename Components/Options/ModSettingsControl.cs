using RoR2.UI;
using UnityEngine;

namespace RiskOfOptions.Components.Options
{
    public class ModSettingsControl : MonoBehaviour
    {
        public string settingToken;
        public string nameToken;
        public LanguageTextMeshController nameLabel;
        
        private MPEventSystemLocator _eventSystemLocator;
        private string _originalValue;

        public bool HasChanged => _originalValue != null;

        public void SubmitSetting(string newValue)
        {
            _originalValue ??= GetCurrentValue();
            
            if (_originalValue == newValue)
                _originalValue = null;
            
            // Todo implement setting value using settingToken.
            
            UpdateControls();
        }

        public string GetCurrentValue()
        {
            // Todo implement retrieving value from MSM using settingToken.
            return "";
        }

        public void Revert()
        {
            if (!HasChanged)
                return;
            
            SubmitSetting(_originalValue);
            _originalValue = null;
        }

        protected void Awake()
        {
            _eventSystemLocator = GetComponent<MPEventSystemLocator>();
            
            if (nameLabel && !string.IsNullOrEmpty(nameToken))
                nameLabel.token = nameToken;
        }

        protected void Start()
        {
            UpdateControls();
        }

        protected void OnEnable()
        {
            UpdateControls();
        }

        protected bool InUpdateControls { get; private set; }

        protected void UpdateControls()
        {
            if (!this)
                return;

            if (InUpdateControls)
                return;

            InUpdateControls = true;
            OnUpdateControls();
            InUpdateControls = false;
        }
        
        protected virtual void OnUpdateControls() {}

        
    }
}