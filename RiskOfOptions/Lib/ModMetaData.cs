using System.Reflection;

namespace RiskOfOptions.Lib;

public record struct ModMetaData
{
    public string Guid;
    public string Name;
    public Assembly SourceAssembly;
}