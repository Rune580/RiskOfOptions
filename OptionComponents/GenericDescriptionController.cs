using UnityEngine;

namespace RiskOfOptions.OptionComponents
{
    class GenericDescriptionController : MonoBehaviour
    {
        private GameObject _genericDescriptionPanel;
        public ModOptionPanelController Mopc { get; internal set; }
        private Transform _canvas;

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
                {
                    return;
                }

                if (_genericDescriptionPanel)
                {
                    _genericDescriptionPanel.SetActive(true);
                }

                if (!_canvas)
                    _canvas = _genericDescriptionPanel.transform.parent.Find("SettingsSubPanel, Mod Options(Clone)");

                Mopc.UnLoad(_canvas);
            }
            catch
            {
                // ignored
            }
        }
    }
}
