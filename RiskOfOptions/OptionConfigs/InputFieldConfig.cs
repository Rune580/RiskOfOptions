using System;
using TMPro;

namespace RiskOfOptions.OptionConfigs
{
    public class InputFieldConfig : BaseOptionConfig
    {
        public SubmitEnum submitOn = SubmitEnum.OnChar;
        
        public TMP_InputField.LineType lineType = TMP_InputField.LineType.MultiLineSubmit;

        /// <summary>
        /// Sets the <see cref="TMP_InputField.richText"/> bool on the <see cref="TMP_InputField"/> of this option.
        /// <remarks>Defaults to true for backwards compatibility sake</remarks>
        /// </summary>
        public bool richText = true;
        
        /// <summary>
        /// Defines how an input field should determine when to submit the changes.
        /// </summary>
        /// <example>
        /// To have an input field submit on either `Char` or `Exit`.
        /// <code>
        ///     var submitEnum = SubmitEnum.OnChar | SubmitEnum.OnExit;
        /// </code>
        /// </example>
        [Flags]
        public enum SubmitEnum
        {
            OnChar = 1,
            OnSubmit = 2,
            OnExit = 4,
            /// <summary>
            /// Convenience/Legacy member.
            /// </summary>
            OnExitOrSubmit = 6,
            /// <summary>
            /// Convenience member for all of the flags.
            /// </summary>
            All = 7,
        }
    }
}