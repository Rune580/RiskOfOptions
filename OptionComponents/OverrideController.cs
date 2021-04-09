using System;
using RiskOfOptions.OptionOverrides;
using RoR2.UI;
using UnityEngine;
using UnityEngine.UI;

namespace RiskOfOptions.OptionComponents
{
    public class OverrideController : MonoBehaviour
    {
        public ModOptionPanelController modOptionPanelController;

        public string overridingName;
        public string overridingCategoryName;

        public string modGuid;

        private void Awake()
        {
            
        }

        private void OnEnable()
        {
            CheckForOverride();
        }

        public void CheckForOverride()
        {
            modOptionPanelController = GetComponentInParent<ModOptionPanelController>();

            var tempOption = ModSettingsManager.GetOption(GetComponent<BaseSettingsControl>().settingName);

            if (tempOption.OptionOverride == null)
                return;

            HGButton[] buttons = GetComponentsInChildren<HGButton>();
            Slider[] sliders = GetComponentsInChildren<Slider>();

            var overridingOption = ModSettingsManager.GetOption(overridingName, overridingCategoryName, modGuid);

            if ((overridingOption.GetBool() && tempOption.OptionOverride.OverrideOnTrue) || (!overridingOption.GetBool() && !tempOption.OptionOverride.OverrideOnTrue))
            {
                foreach (var button in buttons)
                {
                    button.interactable = false;
                    var listener = button.GetComponent<BoolListener>();

                    if (listener != null)
                    {
                        listener.isOverriden = true;
                        listener.onValueChangedBool.Invoke(((CheckBoxOverride)tempOption.OptionOverride).ValueToReturnWhenOverriden);
                    }
                }

                foreach (var slider in sliders)
                {
                    slider.interactable = false;

                    slider.transform.Find("Fill Area").Find("Fill").GetComponent<UnityEngine.UI.Image>().color = slider.colors.disabledColor;
                    slider.transform.parent.Find("TextArea").GetComponent<UnityEngine.UI.Image>().color = slider.colors.disabledColor;
                }
            }
            else
            {
                foreach (var button in buttons)
                {
                    button.interactable = true;

                    var listener = button.GetComponent<BoolListener>();

                    if (listener != null)
                    {
                        listener.onValueChangedBool.Invoke(tempOption.GetBool());
                        listener.isOverriden = false;
                    }
                }

                foreach (var slider in sliders)
                {
                    slider.interactable = true;

                    slider.transform.Find("Fill Area").Find("Fill").GetComponent<UnityEngine.UI.Image>().color = slider.colors.normalColor;
                    slider.transform.parent.Find("TextArea").GetComponent<UnityEngine.UI.Image>().color = GetComponent<HGButton>().colors.normalColor;
                }
            }
        }
    }
}