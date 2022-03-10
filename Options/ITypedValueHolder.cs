namespace RiskOfOptions.Options
{
    public interface ITypedValueHolder<T>
    {
        void SetValue(T value);

        T GetValue();

        T GetOriginalValue();
    }
}