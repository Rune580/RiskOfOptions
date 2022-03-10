using System.Globalization;
using RoR2.UI;

namespace RiskOfOptions.Components.Options
{
    public class DropDownController : BaseSettingsControl
    {
        public RooDropdown dropdown;

        public string[] choices;

        protected new void Awake()
        {
            settingSource = (SettingSource)2;

            if (settingName == "")
                return;

            dropdown = GetComponentInChildren<RooDropdown>();

            base.Awake();

            dropdown.OnValueChanged.AddListener(OnChoiceChanged);

            nameLabel.token = nameToken;
        }

        protected new void OnEnable()
        {
            base.OnEnable();
            GenerateOptions();
        }

        private void GenerateOptions()
        {
            if (choices == null || choices.Length == 0)
                return;

            //Debug.Log(settingName);

            dropdown.choices = choices;

            ////dropdown.ClearOptions();

            //List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

            //foreach (var choice in choices)
            //{
            //    options.Add(new TMP_Dropdown.OptionData(choice));
            //}

            int currentIndex = int.Parse(GetCurrentValue(), CultureInfo.InvariantCulture);

            dropdown.SetChoice(currentIndex);

            //dropdown.value = currentIndex;
        }

        private void OnChoiceChanged(int newValue)
        {
            base.SubmitSetting($"{newValue}");
        }
    }
}