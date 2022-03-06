using RoR2.UI;
using UnityEngine;

namespace RiskOfOptions.Components.OptionComponents
{
    public class ModSettingsControl : MonoBehaviour
    {
        public string settingToken;
        public string nameToken;
        public LanguageTextMeshController nameLabel;

        public bool HasChanged => m_originalValue != null;

        public void SubmitSetting(string newValue)
        {
            if (m_originalValue == null)
                m_originalValue = GetCurrentValue();
            
            if (m_originalValue == newValue)
                m_originalValue = null;
            
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
            if (HasChanged)
            {
                SubmitSetting(m_originalValue);
                m_originalValue = null;
            }
        }

        protected void Awake()
        {
            m_eventSystemLocator = GetComponent<MPEventSystemLocator>();
            
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

        private MPEventSystemLocator m_eventSystemLocator;
        private string m_originalValue;
    }
}