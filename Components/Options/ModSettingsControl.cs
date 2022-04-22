using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using RoR2.UI;

namespace RiskOfOptions.Components.Options
{
    public abstract class ModSettingsControl<T> : ModSetting
    {
        private MPEventSystemLocator _eventSystemLocator;
        private T _originalValue;
        private bool _valueChanged;

        private BaseOptionConfig.IsDisabledDelegate _isDisabled;
        private bool _disabled;
        private bool _restartRequired;
        
        protected ITypedValueHolder<T> valueHolder;

        public void SubmitValue(T newValue)
        {
            if (!_valueChanged)
                _originalValue = GetCurrentValue();
            
            _valueChanged = true;
            
            if (_originalValue.Equals(newValue))
                _valueChanged = false;
            
            valueHolder.Value = newValue;
            
            UpdateControls();
            optionController.OptionChanged();
        }

        protected T GetCurrentValue()
        {
            return valueHolder.Value;
        }

        public override bool HasChanged()
        {
            return _valueChanged;
        }

        public override void Revert()
        {
            if (!HasChanged())
            {
                UpdateControls();
                return;
            }

            SubmitValue(_originalValue);
            _valueChanged = false;
            UpdateControls();
        }

        protected override void Awake()
        {
            base.Awake();
            
            _eventSystemLocator = GetComponent<MPEventSystemLocator>();

            if (option == null)
                return;

            valueHolder ??= (ITypedValueHolder<T>)option;

            _restartRequired = option.GetConfig().restartRequired;
            
            var isDisabled = option.GetConfig().checkIfDisabled;
            if (isDisabled == null)
                return;

            _isDisabled = isDisabled;
        }

        protected override void Start()
        {
            base.Start();
            
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
            
            if (valueHolder.ValueChanged())
            {
                optionController.AddRestartRequired(settingToken);
            }
            else
            {
                optionController.RemoveRestartRequired(settingToken);
            }
        }

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