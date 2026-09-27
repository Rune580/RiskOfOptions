namespace RiskOfOptions.Lib;

public struct StringOrToken
{
    public string Value { get; } = "";
    public bool IsToken { get; } = false;
    
    private StringOrToken(string value, bool isToken)
    {
        Value = value;
        IsToken = isToken;
    }

    public static StringOrToken FromString(string text) => new(text, false);

    public static StringOrToken FromToken(string token) => new(token, true);

    public static implicit operator StringOrToken(string text) => FromString(text);

    public static implicit operator string(StringOrToken value) => value.Value;
}