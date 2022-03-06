using R2API.Utils;
using RoR2.UI;
using TMPro;
using Language = On.RoR2.Language;

namespace RiskOfOptions.Components.OptionComponents
{
    public class InputFieldController : BaseSettingsControl
    {
        private RooInputField _inputField;

        private LanguageTextMeshController _previewLanguage;

        private HGTextMeshProUGUI _previewLabel;

        public TMP_InputValidator validator;
        public TMP_InputField.CharacterValidation characterValidation;
        public bool validateOnSubmit = false;

        protected new void Awake()
        {
            settingSource = (SettingSource)2;

            if (settingName == "")
                return;

            _inputField = GetComponentInChildren<RooInputField>();

            _inputField.onSubmit.AddListener(GetInput);
            _inputField.onValueChanged.AddListener(OnValueChanged);

            _previewLanguage = transform.Find("Text Preview").Find("Text Area").Find("Text").GetComponent<LanguageTextMeshController>();

            _previewLanguage.token = LanguageTokens.OptionInputField;

            _previewLabel = _previewLanguage.GetComponent<HGTextMeshProUGUI>();

            nameLabel = GetComponent<LanguageTextMeshController>();

            nameLabel.token = nameToken;

            base.Awake();

            _inputField.text = GetCurrentValue();
            _previewLanguage.InvokeMethod("Start");
        }

        protected new void OnEnable()
        {
            base.OnEnable();

            HookLanguage();

            FixTextLabel();
        }

        protected void OnDisable()
        {
            UnHookLanguage();
        }

        private void GetInput(string input)
        {
            _previewLanguage.InvokeMethod("Start");
            SubmitSetting(input);
        }

        private void OnValueChanged(string input)
        {
            _previewLanguage.InvokeMethod("Start");
            if (validateOnSubmit)
            {
                SubmitSetting(input);
            }
        }

        private void FixTextLabel()
        {
            _previewLabel.autoSizeTextContainer = false;
            _previewLabel.fontSize = 24;

            _inputField.textComponent.autoSizeTextContainer = false;
            _inputField.textComponent.fontSize = 24;

            if (validator != null)
            {
                _inputField.inputValidator = validator;
            }

            _inputField.characterValidation = characterValidation;
        }

        private void HookLanguage()
        {
            On.RoR2.Language.GetString_string += LanguageOnGetString_string;
        }

        private void UnHookLanguage()
        {
            On.RoR2.Language.GetString_string -= LanguageOnGetString_string;
        }

        private string LanguageOnGetString_string(Language.orig_GetString_string orig, string token)
        {
            if (token != LanguageTokens.OptionInputField)
                return orig(token);

            string text = TrimStringWithinWidth(_inputField.text, 238.7f);

            return text;
        }

        public string TrimStringWithinWidth(string input, float width)
        {
            string trimmedText = $"{input}";

            float currentWidth = TextWidthApproximation(trimmedText);

            if (currentWidth < width)
            {
                return trimmedText;
            }

            trimmedText += "...";

            while (currentWidth > width)
            {
                trimmedText = trimmedText.Remove(trimmedText.Length - 4, 1);
                currentWidth = TextWidthApproximation(trimmedText);
            }

            return trimmedText;
        }


        // Below method is thanks to: https://forum.unity.com/threads/calculate-width-of-a-text-before-without-assigning-it-to-a-tmp-object.758867/#post-5057900
        public float TextWidthApproximation(string text)
        {
            int fontSize = 24;
            TMP_FontAsset fontAsset = _previewLabel.font;
            FontStyles style = _previewLabel.fontStyle;

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
