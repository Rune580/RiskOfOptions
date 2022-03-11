using R2API.Utils;
using RoR2.UI;
using TMPro;
using UnityEngine;
using Language = On.RoR2.Language;

namespace RiskOfOptions.Components.Options
{
    public class InputFieldController : ModSettingsControl<string>
    {
        public GameObject overlay;
        public RooInputField inputField;
        
        private LanguageTextMeshController _previewLanguage;
        private HGTextMeshProUGUI _previewLabel;
        private bool _exitQueued;

        public TMP_InputValidator validator;
        public TMP_InputField.CharacterValidation characterValidation;
        //public bool validateOnSubmit;

        protected override void Awake()
        {
            _previewLanguage = transform.Find("Text Preview").GetComponentInChildren<LanguageTextMeshController>();
            _previewLanguage.token = $"{settingToken}.VALUE";
            
            _previewLabel = _previewLanguage.GetComponentInChildren<HGTextMeshProUGUI>();

            nameLabel = GetComponent<LanguageTextMeshController>();
            nameLabel.token = nameToken;
            
            base.Awake();
            
            if (!inputField)
                return;
            
            var button = transform.Find("Text Preview").GetComponentInChildren<HGButton>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(AttemptShow);
            button.disablePointerClick = false;

            inputField.onSubmit.AddListener(GetInput);
            inputField.onValueChanged.AddListener(OnValueChanged);

            inputField.text = GetCurrentValue();
            _previewLanguage.InvokeMethod("Start");
        }
        
        public void Update()
        {
            bool submitKey = Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter);
            bool validKey = Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Escape);

            if (validKey && !inputField.inField)
            {
                _exitQueued = true;
            }
            else if (submitKey)
            {
                _exitQueued = true;
            }

            if (_exitQueued)
                AttemptHide();
            
        }

        protected new void OnEnable()
        {
            base.OnEnable();

            HookLanguage();

            FixTextLabel();
        }

        protected override void Disable()
        {
            
        }

        protected override void Enable()
        {
            
        }
        
        protected override void OnUpdateControls()
        {
            _previewLanguage.InvokeMethod("Start");
        }

        protected void OnDisable()
        {
            UnHookLanguage();
        }

        private void GetInput(string input)
        {
            _previewLanguage.InvokeMethod("Start");
            SubmitValue(input);
        }

        private void OnValueChanged(string input)
        {
            _previewLanguage.InvokeMethod("Start");
            SubmitValue(input);
        }
        
        private void AttemptShow()
        {
            if (!overlay)
                return;

            if (overlay.activeSelf)
                return;

            _exitQueued = false;

            ShowInputField();
        }

        private void AttemptHide()
        {
            if (!overlay)
                return;

            if (!overlay.activeSelf)
                return;

            _exitQueued = false;

            HideInputField();
        }

        private void ShowInputField()
        {
            overlay.SetActive(true);

            var canvas = overlay.GetComponent<Canvas>();

            canvas.overrideSorting = true;
            canvas.sortingOrder = 30001;
        }

        private void HideInputField()
        {
            overlay.SetActive(false);
        }
        
        private void FixTextLabel()
        {
            // _previewLabel.autoSizeTextContainer = false;
            // _previewLabel.fontSize = 24;
            //
            // _inputField.textComponent.autoSizeTextContainer = false;
            // _inputField.textComponent.fontSize = 24;

            if (validator != null)
                inputField.inputValidator = validator;

            inputField.characterValidation = characterValidation;
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
            if (token != $"{settingToken}.VALUE")
                return orig(token);

            if (!inputField)
                return orig(token);

            string text = TrimStringWithinWidth(inputField.text, 238.7f);

            return text;
        }

        private string TrimStringWithinWidth(string input, float width)
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
