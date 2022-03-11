using RiskOfOptions.Components.Panel;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using RoR2.UI;
using UnityEngine;

namespace RiskOfOptions.Components.Options
{
    public abstract class ModSettingsControl<T> : ModSetting
    {
        public string nameToken;
        public LanguageTextMeshController nameLabel;
        
        private MPEventSystemLocator _eventSystemLocator;
        private ITypedValueHolder<T> _valueHolder;
        private BaseOption _option;
        private T _originalValue;
        private bool _valueChanged;

        private BaseOptionConfig.IsDisabledDelegate _isDisabled;
        private bool _disabled;
        private bool _restartRequired;

        public void SubmitValue(T newValue)
        {
            if (!_valueChanged)
                _originalValue = GetCurrentValue();
            
            _valueChanged = true;
            
            if (_originalValue.Equals(newValue))
                _valueChanged = false;
            
            _valueHolder.SetValue(newValue);
            
            UpdateControls();
            optionController.OptionChanged();
        }

        protected T GetCurrentValue()
        {
            return _valueHolder.GetValue();
        }

        public override bool HasChanged()
        {
            return _valueChanged;
        }

        public override void Revert()
        {
            if (!HasChanged())
                return;
            
            SubmitValue(_originalValue);
            _valueChanged = false;
            UpdateControls();
        }

        protected virtual void Awake()
        {
            _eventSystemLocator = GetComponent<MPEventSystemLocator>();
            
            if (nameLabel && !string.IsNullOrEmpty(nameToken))
                nameLabel.token = nameToken;

            if (string.IsNullOrEmpty(settingToken))
                return;

            _option = ModSettingsManager.OptionCollection.GetOption(settingToken);
            
            _valueHolder ??= (ITypedValueHolder<T>)_option;

            _restartRequired = _option.GetConfig().restartRequired;
            
            var isDisabled = _option.GetConfig().checkIfDisabled;
            if (isDisabled == null)
                return;

            _isDisabled = isDisabled;
        }

        protected void Start()
        {
            if (nameLabel && !string.IsNullOrEmpty(nameToken))
                nameLabel.token = nameToken;
            
            UpdateControls();
        }

        protected void OnEnable()
        {
            UpdateControls();
        }

        public override void CheckIfDisabled()
        {
            if (string.IsNullOrEmpty(settingToken))
                return;
            
            if (_isDisabled == null)
                return;

            var disabled = _isDisabled.Invoke();

            if (disabled && !_disabled)
            {
                Disable();
                _disabled = true;
            }
            else if (!disabled && _disabled)
            {
                Enable();
                _disabled = false;
            }
        }

        private void RestartRequiredCheck()
        {
            if (!_restartRequired)
                return;
            
            if (_option.ValueChanged())
            {
                optionController.AddRestartRequired(settingToken);
            }
            else
            {
                optionController.RemoveRestartRequired(settingToken);
            }
        }

        protected abstract void Disable();

        protected abstract void Enable();

        protected bool InUpdateControls { get; private set; }

        protected void UpdateControls()
        {
            if (!this)
                return;

            if (string.IsNullOrEmpty(settingToken))
                return;

            if (InUpdateControls)
                return;

            CheckIfDisabled();
            RestartRequiredCheck();

            InUpdateControls = true;
            OnUpdateControls();
            InUpdateControls = false;
        }
        
        protected virtual void OnUpdateControls() {}
    }
}