using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RiskOfOptions.Components.Options;
using RiskOfOptions.Components.RuntimePrefabs;
using RiskOfOptions.Containers;
using RiskOfOptions.Options;
using RiskOfOptions.Resources;
using RoR2.UI;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 618

namespace RiskOfOptions.Components.Panel
{
    public class ModOptionPanelController : MonoBehaviour
    {
        public static Action OnModOptionsExit;
        
        public bool initialized;

        public GameObject modListHighlight;

        private GameObject _checkBoxPrefab;
        private GameObject _sliderPrefab;
        private GameObject _stepSliderPrefab;
        private GameObject _intSliderPrefab;
        private GameObject _floatFieldPrefab;
        private GameObject _intFieldPrefab;
        private GameObject _keyBindPrefab;
        private GameObject _inputFieldPrefab;
        private GameObject _choicePrefab;
        private GameObject _genericButtonPrefab;
        private GameObject _colorButtonPrefab;

        private MPEventSystem _mpEventSystem;
        private MPButton _revertButton;
        private SimpleDialogBox _dialogBox;

        //private GameObject _leftGlyph;
        //private GameObject _rightGlyph;
        
        public float degreesPerSecond = 2f;

        private bool _warningShown;

        private IEnumerator _animateRoutine;
        
        private ModOptionsPanelPrefab _panel;
        private ModSetting[] _modSettings = [];

        private void Awake()
        {
            _mpEventSystem = GetComponentInParent<MPEventSystemLocator>().eventSystem;
        }

        public void Start()
        {
            CreatePrefabs();
            CreateModListButtons();
            AddPanelsToSettings();
            CheckIfRestartNeeded();
        }
        
        private void CreatePrefabs()
        {
            RuntimePrefabManager.InitializePrefabs(gameObject);

            _panel = RuntimePrefabManager.Get<ModOptionsPanelPrefab>();
            
            _revertButton = transform.Find("SafeArea").Find("FooterContainer").Find("FooterPanel, M&KB").Find("RevertAndBack (JUICED)").Find("NakedButton (Revert)").GetComponent<HGButton>();
            _revertButton.onClick.AddListener(RevertChanges);

            _checkBoxPrefab = RuntimePrefabManager.Get<CheckBoxPrefab>().CheckBoxButton;
            _sliderPrefab = RuntimePrefabManager.Get<SliderPrefab>().Slider;
            _stepSliderPrefab = RuntimePrefabManager.Get<StepSliderPrefab>().StepSlider;
            _intSliderPrefab = RuntimePrefabManager.Get<IntSliderPrefab>().IntSlider;
            _floatFieldPrefab = RuntimePrefabManager.Get<FloatFieldPrefab>().FloatField;
            _intFieldPrefab = RuntimePrefabManager.Get<IntFieldPrefab>().IntField;
            _keyBindPrefab = RuntimePrefabManager.Get<KeyBindPrefab>().KeyBind;
            _inputFieldPrefab = RuntimePrefabManager.Get<InputFieldPrefab>().InputField;
            _choicePrefab = RuntimePrefabManager.Get<ChoicePrefab>().ChoiceButton;
            _genericButtonPrefab = RuntimePrefabManager.Get<GenericButtonPrefab>().GenericButton;
            _colorButtonPrefab = Prefabs.colorPickerButton;

            _checkBoxPrefab.SetActive(false);
            _sliderPrefab.SetActive(false);
            _stepSliderPrefab.SetActive(false);
            _intSliderPrefab.SetActive(false);
            _floatFieldPrefab.SetActive(false);
            _intFieldPrefab.SetActive(false);
            _keyBindPrefab.SetActive(false);
            _inputFieldPrefab.SetActive(false);
            _choicePrefab.SetActive(false);
            _genericButtonPrefab.SetActive(false);
            _colorButtonPrefab.SetActive(false);
        }

        private void AddPanelsToSettings()
        {
            Transform headerArea = transform.Find("SafeArea").Find("HeaderContainer").Find("Header (JUICED)");
            
            HGHeaderNavigationController navigationController = GetComponent<HGHeaderNavigationController>();
            
            _panel.ModOptionsHeaderButton.transform.SetParent(headerArea);
            
            _panel.ModOptionsHeaderButton.GetComponentInChildren<HGButton>().onClick.AddListener(delegate ()
            {
                navigationController.ChooseHeaderByButton(_panel.ModOptionsHeaderButton.GetComponentInChildren<HGButton>());
            });

            List<HGHeaderNavigationController.Header> headers = GetComponent<HGHeaderNavigationController>().headers.ToList();

            //modOptionsHeaderButton.GetComponentInChildren<HGTextMeshProUGUI>().SetText("MOD OPTIONS");

            HGHeaderNavigationController.Header header = new HGHeaderNavigationController.Header
            {
                headerButton = _panel.ModOptionsHeaderButton.GetComponent<HGButton>(),
                headerName = "Mod Options",
                tmpHeaderText = _panel.ModOptionsHeaderButton.GetComponentInChildren<HGTextMeshProUGUI>(),
                headerRoot = _panel.Canvas
            };

            headers.Add(header);

            GetComponent<HGHeaderNavigationController>().headers = headers.ToList();

            transform.Find("SafeArea").Find("HeaderContainer").Find("Header (JUICED)").Find("GenericGlyph (Right)").SetAsLastSibling();
            
            _panel.ModOptionsHeaderButton.transform.localScale = Vector3.one;
        }


        private void CreateModListButtons()
        {
            HGHeaderNavigationController navigationController = _panel.ModListPanel.GetComponent<HGHeaderNavigationController>();

            List<HGHeaderNavigationController.Header> headers = new List<HGHeaderNavigationController.Header>();

            //_leftGlyph.transform.SetParent(modListLayout);

            Transform modListVerticalLayout = _panel.ModListPanel.transform.Find("Scroll View").Find("Viewport")
                .Find("VerticalLayout");

            if (ModSettingsManager.OptionCollection.Count == 0)
            {
                ShowNoModsDialogBox();
            }
            else
            {
                ShowModsDialogBox();
            }

            foreach (var collection in ModSettingsManager.OptionCollection)
            {
                GameObject newModButton = Instantiate(_panel.ModListButton, modListVerticalLayout);
                
                ModListButton modListButton = newModButton.GetComponent<ModListButton>();
                
                modListButton.descriptionToken = collection.DescriptionToken;
                modListButton.nameLabel = modListButton.GetComponent<LanguageTextMeshController>();
                modListButton.token = collection.NameToken;
                modListButton.modGuid = collection.ModGuid;
                modListButton.navigationController = navigationController;
                modListButton.descriptionLabel = _panel.ModDescriptionPanel.GetComponentInChildren<HGTextMeshProUGUI>();

                // RectTransform modIconRectTransform = newModButton.transform.Find("ModIcon").gameObject.GetComponent<RectTransform>();
                //
                // modIconRectTransform.localPosition = new Vector3(-147f, -0.32f, 0f);
                // modIconRectTransform.sizeDelta = Vector2.zero;
                // modIconRectTransform.anchoredPosition = Vector2.zero;
                // modIconRectTransform.gameObject.AddComponent<FetchIconWhenReady>().modGuid = collection.ModGuid;

                newModButton.name = $"ModListButton ({collection.ModName})";
                newModButton.SetActive(true);

                HGHeaderNavigationController.Header header = new HGHeaderNavigationController.Header
                {
                    headerButton = newModButton.GetComponent<ModListButton>(),
                    headerName = $"ModListButton ({collection.ModName})",
                    tmpHeaderText = newModButton.GetComponentInChildren<HGTextMeshProUGUI>(),
                    headerRoot = null
                };
                
                headers.Add(header);
            }

            //_rightGlyph.transform.SetParent(modListLayout);

            //_leftGlyph.SetActive(true);
            //_rightGlyph.SetActive(true);

            navigationController.headers = headers.ToList();

            navigationController.currentHeaderIndex = -1;
        }

        private void ShowNoModsDialogBox()
        {
            if (_dialogBox)
                DestroyDialogBox();

            if (RiskOfOptionsPlugin.seenNoMods.Value)
                return;
            RiskOfOptionsPlugin.seenNoMods.Value = true;

            _dialogBox = SimpleDialogBox.Create(_mpEventSystem);

            _dialogBox.headerToken = new SimpleDialogBox.TokenParamsPair
            {
                token =  LanguageTokens.NoModsHeaderToken,
                formatParams = Array.Empty<object>()
            };

            _dialogBox.descriptionToken = new SimpleDialogBox.TokenParamsPair
            {
                token = LanguageTokens.NoModsDescriptionToken,
                formatParams = Array.Empty<object>()
            };

            _dialogBox.AddCancelButton(LanguageTokens.DialogButtonToken);
        }

        private void ShowModsDialogBox()
        {
            if (_dialogBox)
                DestroyDialogBox();
            
            if (RiskOfOptionsPlugin.seenMods.Value)
                return;
            RiskOfOptionsPlugin.seenMods.Value = true;
            
            _dialogBox = SimpleDialogBox.Create(_mpEventSystem);

            _dialogBox.headerToken = new SimpleDialogBox.TokenParamsPair
            {
                token =  LanguageTokens.ModsHeaderToken,
                formatParams = Array.Empty<object>()
            };

            _dialogBox.descriptionToken = new SimpleDialogBox.TokenParamsPair
            {
                token = LanguageTokens.ModsDescriptionToken,
                formatParams = Array.Empty<object>()
            };

            _dialogBox.AddCancelButton(LanguageTokens.DialogButtonToken);
        }

        private void DestroyDialogBox()
        {
            if (!_dialogBox.rootObject)
                return;
            
            DestroyImmediate(_dialogBox.rootObject);
            _dialogBox = null;
        }

        internal void LoadModOptionsFromOptionCollection(string modGuid)
        {
            OptionCollection collection = ModSettingsManager.OptionCollection[modGuid];
            
            GameObject categoriesObject = _panel.CategoryHeader.transform.Find("Scroll View").Find("Viewport").Find("Categories (JUICED)").gameObject;

            HGHeaderNavigationController navigationController = categoriesObject.GetComponent<HGHeaderNavigationController>();
            
            CategoryScrollRect categoryScrollRect = _panel.CategoryHeader.GetComponentInChildren<CategoryScrollRect>();

            if (_panel.CategoryHeader.activeSelf || _panel.ModOptionsPanel.activeSelf || _panel.ModOptionsDescriptionPanel.activeSelf || categoryScrollRect.categoryButtons.Count > 0)
                UnloadExistingCategoryButtons();

            if (_panel.ModDescriptionPanel.activeSelf || !_panel.CategoryHeader.activeSelf || !_panel.ModOptionsPanel.activeSelf || !_panel.ModOptionsDescriptionPanel.activeSelf)
            {
                _panel.ModDescriptionPanel.GetComponentInChildren<HGTextMeshProUGUI>().SetText("");

                _panel.ModDescriptionPanel.SetActive(false);
                _panel.CategoryHeader.SetActive(true);
                _panel.ModOptionsPanel.SetActive(true);
                _panel.ModOptionsDescriptionPanel.SetActive(true);
            }

            List<HGHeaderNavigationController.Header> headers = new List<HGHeaderNavigationController.Header>();
            navigationController.currentHeaderIndex = 0;
            
            categoryScrollRect.Categories = collection.CategoryCount;

            _panel.CategoryLeftButton.SetActive(true);
            _panel.CategoryLeftButton.transform.SetSiblingIndex(_panel.CategoryPageIndicators.transform.GetSiblingIndex() - 1);

            for (int i = 0; i < collection.CategoryCount; i++)
            {
                GameObject newCategoryButton = Instantiate(_panel.CategoryHeaderButton, categoriesObject.transform);

                LayoutElement le = newCategoryButton.AddComponent<LayoutElement>();

                le.preferredWidth = 200;

                newCategoryButton.GetComponentInChildren<LanguageTextMeshController>().token = collection[i].NameToken;
                newCategoryButton.GetComponentInChildren<HGTextMeshProUGUI>().SetText(collection[i].name);
                newCategoryButton.GetComponentInChildren<HGButton>().onClick.RemoveAllListeners();

                var categoryIndex = i;

                newCategoryButton.GetComponentInChildren<HGButton>().onClick.AddListener(delegate
                {
                    navigationController.ChooseHeaderByButton(newCategoryButton.GetComponentInChildren<HGButton>());

                    LoadOptionListFromCategory(modGuid, categoryIndex);
                });

                newCategoryButton.name = $"Category Button, {collection[i].name}";
                newCategoryButton.SetActive(true);

                HGHeaderNavigationController.Header header = new HGHeaderNavigationController.Header
                {
                    headerButton = newCategoryButton.GetComponent<HGButton>(),
                    headerName = $"Category Button, {collection[i].name}",
                    tmpHeaderText = newCategoryButton.GetComponentInChildren<HGTextMeshProUGUI>(),
                    headerRoot = null
                };
                
                categoryScrollRect.categoryButtons.Add(newCategoryButton);

                headers.Add(header);
            }
            categoryScrollRect.FixExtra();
            categoryScrollRect.Reload();

            navigationController.headers = headers.ToList();
            navigationController.MoveHeaderLeft();

            _panel.CategoryRightButton.SetActive(true);
            _panel.CategoryRightButton.transform.SetAsLastSibling();

            LoadOptionListFromCategory(collection.ModGuid, navigationController.currentHeaderIndex);
        }

        internal void LoadOptionListFromCategory(string modGuid, int categoryIndex)
        {
            UnloadExistingOptionButtons();

            _panel.ModOptionsDescriptionPanel.GetComponentInChildren<HGTextMeshProUGUI>().SetText("");

            Category category = ModSettingsManager.OptionCollection[modGuid][categoryIndex];

            Transform verticalLayoutTransform = _panel.ModOptionsPanel.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout");

            Selectable lastSelectable = null;

            _modSettings = new ModSetting[category.OptionCount];

            for (int i = 0; i < category.OptionCount; i++)
            {
                var option = category[i];

                GameObject button = option switch
                {
                    CheckBoxOption => option.CreateOptionGameObject(_checkBoxPrefab, verticalLayoutTransform),
                    SliderOption => option.CreateOptionGameObject(_sliderPrefab, verticalLayoutTransform),
                    StepSliderOption => option.CreateOptionGameObject(_stepSliderPrefab, verticalLayoutTransform),
                    IntSliderOption => option.CreateOptionGameObject(_intSliderPrefab, verticalLayoutTransform),
                    FloatFieldOption => option.CreateOptionGameObject(_floatFieldPrefab, verticalLayoutTransform),
                    IntFieldOption => option.CreateOptionGameObject(_intFieldPrefab, verticalLayoutTransform),
                    KeyBindOption => option.CreateOptionGameObject(_keyBindPrefab, verticalLayoutTransform),
                    StringInputFieldOption => option.CreateOptionGameObject(_inputFieldPrefab, verticalLayoutTransform),
                    GenericButtonOption => option.CreateOptionGameObject(_genericButtonPrefab, verticalLayoutTransform),
                    ChoiceOption => option.CreateOptionGameObject(_choicePrefab, verticalLayoutTransform),
                    ColorOption => option.CreateOptionGameObject(_colorButtonPrefab, verticalLayoutTransform),
                    _ => throw new ArgumentOutOfRangeException(option.Name)
                };

                _modSettings[i] = button.GetComponentInChildren<ModSetting>();
                _modSettings[i].optionController = this;
                
                CanvasGroup canvasGroup = button.AddComponent<CanvasGroup>();

                var buttonComponent = button.GetComponentInChildren<HGButton>();
                if (buttonComponent)
                {
                    button.GetComponentInChildren<HGButton>().hoverToken = option.GetDescriptionToken();

                    button.GetComponentInChildren<HGButton>().onSelect.AddListener(delegate
                    {
                        _panel.ModOptionsDescriptionPanel.GetComponentInChildren<HGTextMeshProUGUI>().SetText(option.GetLocalizedDescription());
                    });
                }

                var selectable = button.GetComponentInChildren<Selectable>();

                if (selectable)
                {
                    var selectableNavigation = selectable.navigation;
                    selectableNavigation.mode = Navigation.Mode.Explicit;
                    
                    if (i == 0 || i > category.OptionCount)
                    {
                        // Todo if at top of list, select category.
                    }
                    else if (lastSelectable)
                    {
                        var lastSelectableNavigation = lastSelectable.navigation;
                        lastSelectableNavigation.selectOnDown = selectable;
                        lastSelectable.navigation = lastSelectableNavigation;
                        
                        selectableNavigation.selectOnUp = lastSelectable;
                    }
                    
                    selectable.navigation = selectableNavigation;
                    lastSelectable = selectable;
                }
                else
                {
                    lastSelectable = null;
                }

                // if (!option.Visibility)
                // {
                //     canvasGroup.alpha = 1;
                //     canvasGroup.blocksRaycasts = false;
                //     button.GetComponent<LayoutElement>().ignoreLayout = true;
                //     continue;
                // }

                button.SetActive(true);
            }
        }

        internal void AddRestartRequired(string settingToken)
        {
            if (!ModSettingsManager.RestartRequiredOptions.Contains(settingToken))
                ModSettingsManager.RestartRequiredOptions.Add(settingToken);
        }

        internal void RemoveRestartRequired(string settingToken)
        {
            if (ModSettingsManager.RestartRequiredOptions.Contains(settingToken))
                ModSettingsManager.RestartRequiredOptions.Remove(settingToken);
        }

        private void CheckIfRestartNeeded()
        {
            bool warningAlreadyShown = _warningShown;
            _warningShown = ModSettingsManager.RestartRequiredOptions.Count > 0;

            if (warningAlreadyShown && _warningShown)
                return;

            if (_warningShown)
            {
                ShowRestartWarning();
            }
            else
            {
                HideRestartWarning();
            }
        }

        private void ShowRestartWarning()
        {
            RectTransform modListScrollViewRT = _panel.ModListPanel.transform.Find("Scroll View").GetComponent<RectTransform>();
            RectTransform warningPanelRT = _panel.WarningPanel.GetComponent<RectTransform>();

            warningPanelRT.gameObject.SetActive(true);

            warningPanelRT.GetComponentInChildren<HGTextMeshProUGUI>().SetText($"Restart Required!");

            if (_animateRoutine != null)
                StopCoroutine(_animateRoutine);

            HGTextMeshProUGUI warningText = warningPanelRT.GetComponentInChildren<HGTextMeshProUGUI>();

            Color showColor = new Color(warningText.color.r, warningText.color.b, warningText.color.g);

            _animateRoutine = AnimateWarningPanel(modListScrollViewRT, new Vector2(0f, 0.074f), warningPanelRT, new Vector2(1f, 0.08f), showColor, degreesPerSecond, 720f);

            StartCoroutine(_animateRoutine);
        }

        private void HideRestartWarning()
        {
            RectTransform modListScrollViewRT = _panel.ModListPanel.transform.Find("Scroll View").GetComponent<RectTransform>();
            RectTransform warningPanelRT = _panel.WarningPanel.GetComponent<RectTransform>();

            if (_animateRoutine != null)
                StopCoroutine(_animateRoutine);

            HGTextMeshProUGUI warningText = warningPanelRT.GetComponentInChildren<HGTextMeshProUGUI>();

            Color hideColor = new Color(warningText.color.r, warningText.color.b, warningText.color.g, 0f);

            _animateRoutine = AnimateWarningPanel(modListScrollViewRT, new Vector2(0f, 0f), warningPanelRT, new Vector2(1f, 0f), hideColor, degreesPerSecond, -360f);

            StartCoroutine(_animateRoutine);
        }

        private IEnumerator AnimateWarningPanel(RectTransform modListTransform, Vector2 newModListPos, RectTransform warningTransform, Vector2 newWarningPos, Color textColor, float angleIncrement, float maxAngleRotation)
        {
            bool animating = true;

            float animSpeed = 2.25f;

            HGTextMeshProUGUI warningText = warningTransform.GetComponentInChildren<HGTextMeshProUGUI>();

            Image restartIcon = warningText.transform.parent.Find("RestartIcon").GetComponent<Image>();

            RectTransform restartRectTransform = restartIcon.GetComponent<RectTransform>();

            float max = Mathf.Abs(maxAngleRotation);

            while (animating)
            {
                //modListTransform.anchorMin = Vector2.Lerp(modListTransform.anchorMin, newModListPos, animSpeed * Time.unscaledDeltaTime);

                //warningTransform.anchorMax = Vector2.Lerp(warningTransform.anchorMax, newWarningPos, animSpeed * Time.unscaledDeltaTime);

                modListTransform.anchorMin = ExtensionMethods.SmoothStep(modListTransform.anchorMin, newModListPos, (animSpeed * 5.25f) * Time.unscaledDeltaTime);

                warningTransform.anchorMax = ExtensionMethods.SmoothStep(warningTransform.anchorMax, newWarningPos, (animSpeed * 5.25f) * Time.unscaledDeltaTime);

                float angle = Mathf.Clamp(Mathf.Lerp(angleIncrement * Time.unscaledDeltaTime, max, 1f * Time.unscaledDeltaTime), 90 * Time.unscaledDeltaTime, Math.Abs(maxAngleRotation));

                if (angle > 90 * Time.unscaledDeltaTime)
                    max -= angle;

                restartRectTransform.localRotation *= Quaternion.AngleAxis(maxAngleRotation > 0 ? angle : -angle, Vector3.forward);

                switch (textColor.a)
                {
                    case 1f:
                        warningText.color = Color.Lerp(warningText.color, textColor, (animSpeed * 2) * Time.unscaledDeltaTime);
                        restartIcon.color = Color.Lerp(restartIcon.color, textColor, (animSpeed * 2) * Time.unscaledDeltaTime);
                        break;
                    case 0f:
                        warningText.color = Color.Lerp(warningText.color, textColor, (animSpeed * 4) * Time.unscaledDeltaTime);
                        restartIcon.color = Color.Lerp(restartIcon.color, textColor, (animSpeed * 4) * Time.unscaledDeltaTime);
                        break;
                }

                if (ExtensionMethods.CloseEnough(modListTransform.anchorMin, newModListPos) &&
                    ExtensionMethods.CloseEnough(warningTransform.anchorMax, newWarningPos) &&
                    ExtensionMethods.CloseEnough(warningText.color, textColor) &&
                    ExtensionMethods.CloseEnough(restartIcon.color, textColor))
                {
                    modListTransform.anchorMin = newModListPos;
                    warningTransform.anchorMax = newWarningPos;

                    warningText.color = textColor;
                    restartIcon.color = textColor;
                }

                yield return new WaitForEndOfFrame();
            }
        }

        public void UpdateVisibility(string optionToken, bool visible)
        {
            foreach (var modSetting in GetComponentsInChildren<ModSetting>())
            {
                if (modSetting.settingToken != optionToken)
                    continue;

                var canvasGroup = modSetting.GetComponent<CanvasGroup>();

                canvasGroup.alpha = visible ? 1 : 0;
                canvasGroup.blocksRaycasts = visible;

                modSetting.GetComponent<LayoutElement>().ignoreLayout = !visible;
                break;
            }
        }

        internal void UnLoad()
        {
            if (!initialized)
                return;

            if (!_panel.ModDescriptionPanel.activeSelf)
            {
                _panel.ModDescriptionPanel.SetActive(true);

                _panel.CategoryHeader.SetActive(false);
                _panel.ModOptionsPanel.SetActive(false);
                _panel.ModOptionsDescriptionPanel.SetActive(false);
            }

            modListHighlight.transform.SetParent(transform);
            modListHighlight.SetActive(false);

            UnloadExistingOptionButtons();
            UnloadExistingCategoryButtons();
        }


        private void UnloadExistingCategoryButtons()
        {
            _panel.CategoryHeaderHighlight.transform.SetParent(transform);
            _panel.CategoryHeaderHighlight.SetActive(false);

            var scrollRect = _panel.CategoryHeader.GetComponentInChildren<CategoryScrollRect>();

            foreach (var activeCategoryButton in scrollRect.categoryButtons)
            {
                if (activeCategoryButton.gameObject == null)
                    continue;
                
                GameObject buttonGameObject = activeCategoryButton.gameObject;
                buttonGameObject.SetActive(false);

                DestroyImmediate(buttonGameObject);
            }
            
            scrollRect.categoryButtons.Clear();

            //_leftGlyph.SetActive(false);
            //_rightGlyph.SetActive(false);
        }

        private void UnloadExistingOptionButtons()
        {
            if (_modSettings.Length <= 0)
                return;

            foreach (var button in _modSettings)
            {
                if (button.gameObject == null)
                    continue;

                GameObject buttonGameObject = button.gameObject;
                buttonGameObject.SetActive(false);

                DestroyImmediate(buttonGameObject);
            }
        }

        private void OnDestroy()
        {
            DestroyImmediate(_keyBindPrefab);
            DestroyImmediate(modListHighlight);
            
            RuntimePrefabManager.DestroyPrefabs();
        }

        public void OnDisable()
        {
            initialized = false;
        }

        public void OnEnable()
        {
            initialized = true;
        }

        public void Update()
        {
            bool interactable = false;
            foreach (var modSetting in _modSettings)
            {
                if (!modSetting)
                {
                    Debug.LogWarning("Found null mod setting!");
                    continue;
                }
                
                if (modSetting.HasChanged())
                    interactable = true;
            }

            _revertButton.interactable = interactable;
        }

        public void RevertChanges()
        {
            foreach (var modSetting in _modSettings)
                modSetting.Revert();
        }

        public void OptionChanged()
        {
            CheckIfRestartNeeded();

            foreach (var modSetting in _modSettings)
            {
                if (modSetting)
                {
                    modSetting.CheckIfDisabled();
                    modSetting.UpdateModifiedIndicator();
                }
            }
                
        }
    }
}