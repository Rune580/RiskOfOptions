using RiskOfOptions.Options;
using TMPro;
using UnityEngine;

namespace RiskOfOptions.Components.Misc;

[CreateAssetMenu(fileName = "Input Field Validator", menuName = "RoO/DecimalSeparatorValidator")]
public class DecimalSeparatorValidator : TMP_InputValidator
{
    private static DecimalSeparator Separator => RiskOfOptionsPlugin.decimalSeparator!.Value;
    
    public override char Validate(ref string text, ref int pos, char ch)
    {
        if (pos != 0 || text.Length <= 0 || text[0] != '-')
        {
            switch (ch)
            {
                case >= '0' and <= '9':
                    text += ch;
                    pos++;
                    return ch;
                case '-' when pos == 0:
                    text += ch;
                    pos++;
                    return ch;
            }
            
            char separator = Separator.Char();
            if (ch == separator && !text.Contains(separator.ToString()))
            {
                text += ch;
                pos++;
                return ch;
            }
        }

        return '\0';
    }
}