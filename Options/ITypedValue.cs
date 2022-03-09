namespace RiskOfOptions.Options
{
    public interface ITypedValue<T>
    {
        void SetValue(T value);

        T GetValue();
    }
}