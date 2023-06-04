using UnityEngine;

namespace RiskOfOptions.Components.Panel
{
    internal class GenericDescriptionController : MonoBehaviour
    {
        private GameObject _genericDescriptionPanel;
        public ModOptionPanelController Mopc { get; internal set; }

        private void OnEnable()
        {
            if (!_genericDescriptionPanel)
                _genericDescriptionPanel = GameObject.Find("GenericDescriptionPanel");

            _genericDescriptionPanel.SetActive(false);
        }

        private void OnDisable()
        {
            try
            {
                if (!Mopc)
                    Mopc = GetComponentInParent<ModOptionPanelController>();

                if (!Mopc.initialized)
                    return;

                if (_genericDescriptionPanel)
                    _genericDescriptionPanel.SetActive(true);

                Mopc.UnLoad();
            }
            catch
            {
                // ignored
            }
        }
    }
}
