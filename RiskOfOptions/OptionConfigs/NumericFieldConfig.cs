using System.Globalization;

namespace RiskOfOptions.OptionConfigs;

public abstract class NumericFieldConfig<TNumeric> : BaseOptionConfig
{
    public abstract TNumeric Min { get; set; }
    public abstract TNumeric Max { get; set; }
    
    public string FormatString { get; set; } = "{0}";
    
    public NumericFieldTryParse? TryParse { get; set; }
    
    public delegate bool NumericFieldTryParse(string input, CultureInfo cultureInfo, out TNumeric value);
}