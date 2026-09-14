namespace RiskOfOptions.Options;

public readonly record struct OptionId(string ModGuid, string Section, string Name)
{
    public override string ToString() => $"{ModGuid}.{Section}.{Name}".Replace(" ", "_").ToUpper();

    public static implicit operator string(OptionId id) => id.ToString();
}