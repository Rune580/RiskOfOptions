using System;
using System.Collections.Generic;
using System.Text;
using On.RoR2.UI;
using R2API.Utils;
using RoR2;

using static RiskOfOptions.ExtensionMethods;

namespace RiskOfOptions
{
    public static class BaseSettingsControlOverride
    {
        public static void Init()
        {
            On.RoR2.UI.BaseSettingsControl.SubmitSetting += SubmitSettingRoo;
            On.RoR2.UI.BaseSettingsControl.GetCurrentValue += GetCurrentValueRoo;
        }

        private static string GetCurrentValueRoo(On.RoR2.UI.BaseSettingsControl.orig_GetCurrentValue orig, RoR2.UI.BaseSettingsControl self)
        {
            return (int)self.settingSource <= 1 ? orig(self) : RoR2.Console.instance.FindConVar(self.settingName).GetString();
        }

        private static void SubmitSettingRoo(On.RoR2.UI.BaseSettingsControl.orig_SubmitSetting orig, RoR2.UI.BaseSettingsControl self, string newValue)
        {
            if ((int)self.settingSource <= 1)
                orig(self, newValue);

            var tempOption = ModSettingsManager.GetOption(self.settingName);

            Indexes indexes = ModSettingsManager.OptionContainers.GetIndexes(tempOption.ModGuid, tempOption.Name, tempOption.CategoryName);

            ModSettingsManager.OptionContainers[indexes.ContainerIndex].GetModOptionsCached()[indexes.OptionIndexInContainer].Value = newValue;

            //Debug.Log($"{tempOption.Name} set to : {newValue}");

            if (self.GetType() == typeof(RoR2.UI.SettingsSlider))
                RoR2Application.onNextUpdate += () => { self.InvokeMethod("OnUpdateControls"); };








            // Temporary shit Saves to console for now....
            RoR2.Console.instance.FindConVar(tempOption.ConsoleToken).AttemptSetString(newValue);

            if (self.GetType() == typeof(RoR2.UI.CarouselController))
                self.GetComponentInParent<ModOptionPanelController>().UpdateExistingOptionButtons();
        }
    }
}
