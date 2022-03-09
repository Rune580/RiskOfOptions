using RiskOfOptions.Components.Panel;
using UnityEngine;

namespace RiskOfOptions.Components.Options
{
    public abstract class ModSetting : MonoBehaviour
    {
        public ModOptionPanelController optionController;
        public string settingToken;
        public abstract bool HasChanged();

        public abstract void Revert();

        public abstract void CheckIfDisabled();
    }
}