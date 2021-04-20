namespace RiskOfOptions.OptionConstructors
{
    public class OptionConstructorBase
    {
        public string Name;
        public string CategoryName;
        public bool IsVisible;
        public bool InvokeValueChangedEventOnStart;

        public string Description
        {
            set
            {
                descriptionArray = new object[] {value};
            }
        }

        internal object[] descriptionArray;
        internal string value;
        protected OptionConstructorBase()
        {
            Name = "";
            CategoryName = "";
            Description = "";
            IsVisible = true;
            InvokeValueChangedEventOnStart = false;
        }
    }
}
