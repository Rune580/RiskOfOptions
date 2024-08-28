using RiskOfOptions.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace RiskOfOptions.Components.Panel
{
    public class FetchIconWhenReady : MonoBehaviour
    {
        public string modGuid;
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void Start()
        {
            _image.sprite = RiskOfOptions.Resources.Assets.Load<Sprite>("assets/RiskOfOptions/missing_icon.png");

            SetTexture();
        }

        private void SetTexture()
        {
            var icon = ModSettingsManager.OptionCollection[modGuid].icon;
            
            if (icon)
                _image.sprite = icon;
        }
    }
}
