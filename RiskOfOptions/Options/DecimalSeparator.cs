using System;
using System.Globalization;

namespace RiskOfOptions.Options;

public enum DecimalSeparator
{
    Period,
    Comma
}

public static class DecimalSeparatorExtensions
{
    private static CultureInfo? _commaCultureInstance;
    private static CultureInfo CommaCulture
    {
        get
        {
            if (_commaCultureInstance is null)
            {
                _commaCultureInstance = (CultureInfo) CultureInfo.InvariantCulture.Clone();
                _commaCultureInstance.NumberFormat.NumberDecimalSeparator = ",";
                _commaCultureInstance.NumberFormat.NumberGroupSeparator = ".";
            }

            return _commaCultureInstance;
        }
    }
    
    public static char Char(this DecimalSeparator separator)
    {
        return separator switch
        {
            DecimalSeparator.Period => '.',
            DecimalSeparator.Comma => ',',
            _ => throw new ArgumentOutOfRangeException(nameof(separator), separator, null)
        };
    }

    public static CultureInfo GetCultureInfo(this DecimalSeparator separator)
    {
        return separator switch
        {
            DecimalSeparator.Period => CultureInfo.InvariantCulture,
            DecimalSeparator.Comma => CommaCulture,
            _ => throw new ArgumentOutOfRangeException(nameof(separator), separator, null)
        };
    }
}