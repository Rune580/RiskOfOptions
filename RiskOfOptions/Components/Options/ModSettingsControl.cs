using System;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using RoR2.UI;
using UnityEngine;

namespace RiskOfOptions.Components.Options;

public abstract class ModSettingsControl<TValue> : ModSettingsControl<TValue, BaseOptionConfig>;

public abstract class ModSettingsControl<TValue, TOptionConfig> : ModSetting
    where TOptionConfig : BaseOptionConfig
{
    private MPEventSystemLocator _eventSystemLocator;
    private TValue _originalValue;
    private bool _valueChanged;

    private BaseOptionConfig.IsDisabledDelegate? _isDisabled;
    private bool _disabled;
    private bool _restartRequired;

    public Action<OptionId>? optionValueChanged;
    
    protected TOptionConfig? Config { get; private set; }
        
    protected IConfigItemOption<TValue>? configItemOption;

    [SerializeField]
    private UnityEngine.UI.RawImage modifiedIndicator;

    public void SubmitValue(TValue newValue)
    {
        if (!_valueChanged)
            _originalValue = GetCurrentValue();
            
        _valueChanged = true;
            
        if (_originalValue.Equals(newValue))
            _valueChanged = false;

        configItemOption?.Value = newValue;
        
        UpdateControls();

        if (option is not null)
            optionValueChanged?.Invoke(option.Id);
        
        RestartRequiredCheck();
    }

    protected TValue GetCurrentValue() => configItemOption!.Value;

    protected TValue GetDefaultValue() => configItemOption!.ConfigItem.DefaultValue;

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

    public void ResetToDefault()
    {
        if (option is null)
            return;

        SubmitValue(GetDefaultValue());
    }

    protected override void Awake()
    {
        base.Awake();
            
        _eventSystemLocator = GetComponent<MPEventSystemLocator>();

        if (option is not IConfigItemOption<TValue> itemOption)
            return;

        configItemOption ??= itemOption;

        Config = (TOptionConfig)option.GetConfig();
        
        UpdateModifiedIndicator();
        
        _restartRequired = Config.restartRequired;
            
        var isDisabled = Config.checkIfDisabled;
        if (isDisabled is null)
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
        if (!optionId.IsValid())
            return;
            
        if (_isDisabled is null)
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
        if (!_restartRequired || configItemOption is null || configItemOption.Value is null)
            return;
        
        if (configItemOption.Value.Equals(configItemOption.InitialValue))
        {
            ModSettingsManager.RestartRequiredOptions.Add(optionId);
        }
        else
        {
            ModSettingsManager.RestartRequiredOptions.Remove(optionId);
        }
    }

    public override void UpdateModifiedIndicator()
    {
        if (modifiedIndicator)
        {
            var nonDefault = !GetCurrentValue().Equals(GetDefaultValue());
            modifiedIndicator.enabled = (nonDefault || HasChanged()) && RiskOfOptionsPlugin.showModifiedIndicator!.Value;
            modifiedIndicator.color = HasChanged() ? RiskOfOptionsPlugin.hasChangedModifiedColor!.Value : RiskOfOptionsPlugin.nonDefaultModifiedColor!.Value;
        }
    }

    protected bool InUpdateControls { get; private set; }

    protected void UpdateControls()
    {
        if (!this)
            return;

        if (string.IsNullOrEmpty(optionId))
            return;

        if (InUpdateControls)
            return;

        CheckIfDisabled();
        // RestartRequiredCheck();
        UpdateModifiedIndicator();

        InUpdateControls = true;
        OnUpdateControls();
        InUpdateControls = false;
    }
        
    protected virtual void OnUpdateControls() {}
}