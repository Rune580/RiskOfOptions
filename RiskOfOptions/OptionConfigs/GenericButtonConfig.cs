using UnityEngine.Events;

namespace RiskOfOptions.OptionConfigs
{
    public class GenericButtonConfig : BaseOptionConfig // Since the button option doesn't take in a ConfigEntry we need to require certain parameters to be passed.
    {
        /// <summary>
        /// Invoked when the GenericButton is pressed.
        /// </summary>
        public readonly UnityAction OnButtonPressed;

        public readonly string ButtonText;
        
        public GenericButtonConfig(string name, string category, string description, string buttonText, UnityAction onButtonPressed)
        {
            this.name = name;
            this.category = category;
            this.description = description;
            ButtonText = buttonText;
            OnButtonPressed = onButtonPressed;
        }
    }
}