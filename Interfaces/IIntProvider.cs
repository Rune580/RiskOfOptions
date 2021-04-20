namespace RiskOfOptions.Interfaces
{
    internal interface IIntProvider
    {
        public UnityEngine.Events.UnityAction<int> OnValueChangedChoice { get; set; }

        public int Value { get; set; }
    }
}