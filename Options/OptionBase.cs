namespace RiskOfOptions.Options
{
    public class OptionBase
    {
        /// <summary>
        /// The name displayed in the mod options menu.
        /// </summary>
        public string Name;

        /// <summary>
        /// The name token used for the LanguageAPI and for internal use.
        /// </summary>
        public string NameToken;

        /// <summary>
        /// The description displayed in the mod options menu.
        /// </summary>
        public object[] Description;

        public string OptionToken { get; protected set; }

        public string ConsoleToken { get; protected set; }

        public string ModGuid { get; protected set; }
        public string ModName { get; protected set; }
    }
}
