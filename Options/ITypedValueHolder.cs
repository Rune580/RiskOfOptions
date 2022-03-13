namespace RiskOfOptions.Options
{
    public interface ITypedValueHolder<T>
    {
        T GetOriginalValue();
        
        T Value { get; set; }

        bool ValueChanged();
    }
}