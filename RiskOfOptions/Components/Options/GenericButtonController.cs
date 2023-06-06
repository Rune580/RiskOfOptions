using RoR2.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RiskOfOptions.Components.Options
{
    public class GenericButtonController : ModSetting
    {
        public string buttonToken;
        public UnityAction OnButtonPressed;

        //private RectTransform _buttonTransform;
        //private RectTransform _buttonTextTransform;
        
        protected override void Awake()
        {
            nameLabel = GetComponent<LanguageTextMeshController>();
            nameLabel.token = nameToken;
            
            base.Awake();

            var container = transform.Find("ButtonContainer").gameObject;
            container.GetComponentInChildren<LanguageTextMeshController>().token = buttonToken;
            
            var button = container.GetComponentInChildren<HGButton>();
            button.onClick.AddListener(OnButtonPressed);
        }

        protected override void Start()
        {
            base.Start();
            
            var button = transform.Find("ButtonContainer").gameObject.GetComponentInChildren<HGButton>();
            
            var buttonLayoutElement = button.GetComponent<LayoutElement>();
            var buttonText = button.GetComponentInChildren<HGTextMeshProUGUI>();
            
            buttonLayoutElement.preferredWidth = TextWidthApproximation(buttonText.text, buttonText.font, buttonText.fontStyle) + 24;
        }

        public override bool HasChanged()
        {
            return false;
        }

        public override void Revert() { }

        public override void CheckIfDisabled() { }

        protected override void Disable()
        {
            // Todo implement
        }

        protected override void Enable()
        {
            // Todo implement
        }
        
        public float TextWidthApproximation(string text, TMP_FontAsset fontAsset, FontStyles style)
        {
            int fontSize = 24;

            // Compute scale of the target point size relative to the sampling point size of the font asset.
            float pointSizeScale = fontSize / (fontAsset.faceInfo.pointSize * fontAsset.faceInfo.scale);
            float emScale = fontSize * 0.01f;

            float styleSpacingAdjustment = (style & FontStyles.Bold) == FontStyles.Bold ? fontAsset.boldSpacing : 0;
            float normalSpacingAdjustment = fontAsset.normalSpacingOffset;

            float width = 0;

            foreach (var unicode in text)
            {
                // Make sure the given unicode exists in the font asset.
                if (fontAsset.characterLookupTable.TryGetValue(unicode, out var character))
                    width += character.glyph.metrics.horizontalAdvance * pointSizeScale + (styleSpacingAdjustment + normalSpacingAdjustment) * emScale;
            }

            return width;
        }
    }
}