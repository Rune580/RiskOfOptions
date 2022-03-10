using System;
using RoR2;
using RoR2.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RiskOfOptions.Components.Options
{
    /// <summary>
    /// This is basically a reimplementation of MPDropdown with my own methods of handling the dropdown.
    /// After all... why not?
    /// Why shouldn't I change it?
    /// </summary>
    public class RooDropdown : Selectable, IPointerClickHandler, IEventSystemHandler, ISubmitHandler, ICancelHandler
    {
        internal static GameObject CheckBoxPrefab;
        internal static GameObject PanelPrefab;
        internal static Sprite CheckMarkSprite;
        private static GameObject _dropDownChoicePrefab;

        private MPEventSystemLocator _eventSystemLocator;
        private bool _isPointerInside;
        private MPEventSystem _eventSystem => _eventSystemLocator.eventSystem;
        private SelectionState _previousState = SelectionState.Disabled;
        private HGTextMeshProUGUI _label;
        private int _currentIndex = 0;

        private GameObject _template;
        private GameObject _content;
        private GameObject[] _buttonCache;

        private ColorBlock _defaultColors;
        private ColorBlock _selectedColors;

        private bool _heldDown = false;

        private bool Showing => _template.activeSelf;

        public bool allowAllEventSystems;
        public string[] choices;

        public DropDownEvent OnValueChanged { get; set; } = new DropDownEvent();

        protected override void Awake()
        {
            base.Awake();
            _eventSystemLocator = GetComponent<MPEventSystemLocator>();

            _label = transform.Find("Label").GetComponent<HGTextMeshProUGUI>();

            _defaultColors = CheckBoxPrefab.GetComponentInChildren<HGButton>().colors;

            _selectedColors = _defaultColors;
            _selectedColors.normalColor = new Color(0.3f, 0.3f, 0.3f, 1);
            _selectedColors.highlightedColor = new Color(0.3f, 0.3f, 0.3f, 1);

            SetupTemplate();

            if (CheckBoxPrefab && !_dropDownChoicePrefab)
                CreatePrefab();
        }

        protected void Update()
        {
            if (!Showing)
                return;

            bool validKey = Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Escape);

            bool validKeyReleased = Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.Escape);

            if (validKeyReleased && _heldDown)
            {
                _heldDown = false;
                return;
            }

            if (!validKey)
                return;

            if (_heldDown)
                return;

            if (!_isPointerInside || Input.GetKey(KeyCode.Escape))
                Hide();

            _heldDown = true;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DestroyImmediateChoices();
        }

        #region ReImplementations

        private Selectable FilterSelectable(Selectable selectable)
        {
            if (!selectable)
                return selectable;

            var component = selectable.GetComponent<MPEventSystemLocator>();

            if (!component || component.eventSystem != _eventSystemLocator.eventSystem)
            {
                selectable = null;
            }
            return selectable;
        }
        public override Selectable FindSelectableOnDown()
        {
            return FilterSelectable(base.FindSelectableOnDown());
        }

        public override Selectable FindSelectableOnLeft()
        {
            return FilterSelectable(base.FindSelectableOnLeft());
        }

        public override Selectable FindSelectableOnRight()
        {
            return FilterSelectable(base.FindSelectableOnRight());
        }

        public override Selectable FindSelectableOnUp()
        {
            return FilterSelectable(base.FindSelectableOnUp());
        }

        public bool InputModuleIsAllowed(BaseInputModule inputModule)
        {
            if (allowAllEventSystems)
            {
                return true;
            }
            EventSystem eventSystem = _eventSystem;
            return eventSystem && inputModule == eventSystem.currentInputModule;
        }

        private void AttemptSelection(PointerEventData eventData)
        {
            if (_eventSystem && _eventSystem.currentInputModule == eventData.currentInputModule)
            {
                _eventSystem.SetSelectedGameObject(gameObject, eventData);
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (!InputModuleIsAllowed(eventData.currentInputModule))
            {
                return;
            }
            _isPointerInside = true;
            base.OnPointerEnter(eventData);
            AttemptSelection(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            if (!InputModuleIsAllowed(eventData.currentInputModule))
            {
                return;
            }
            if (_eventSystem && gameObject == _eventSystem.currentSelectedGameObject)
            {
                enabled = false;
                enabled = true;
            }
            _isPointerInside = false;
            base.OnPointerExit(eventData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!InputModuleIsAllowed(eventData.currentInputModule))
            {
                return;
            }
            ToggleShow();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (!InputModuleIsAllowed(eventData.currentInputModule))
            {
                return;
            }
            base.OnPointerUp(eventData);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (!InputModuleIsAllowed(eventData.currentInputModule))
            {
                return;
            }
            if (IsInteractable() && navigation.mode != Navigation.Mode.None)
            {
                AttemptSelection(eventData);
            }
            base.OnPointerDown(eventData);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _isPointerInside = false;
        }

        protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            if (_previousState == state)
                return;

            if (state == Selectable.SelectionState.Highlighted)
            {
                Util.PlaySound("Play_UI_menuHover", RoR2Application.instance.gameObject);
            }
            _previousState = state;
        }

        #endregion

        public void OnSubmit(BaseEventData eventData)
        {
            ToggleShow();
        }

        public void OnCancel(BaseEventData eventData)
        {
            Hide();
        }

        public void SetChoice(int index)
        {
            bool cacheIsValid = _buttonCache != null && _buttonCache.Length != 0;

            if (cacheIsValid)
                _buttonCache[_currentIndex].GetComponentInChildren<HGButton>().colors = _defaultColors;


            _currentIndex = index;

            if (!_label)
                _label = transform.Find("Label").GetComponent<HGTextMeshProUGUI>();

            _label.SetText(choices[_currentIndex]);

            if (cacheIsValid)
                _buttonCache[_currentIndex].GetComponentInChildren<HGButton>().colors = _selectedColors;
        }

        private void SubmitChoice(int index)
        {
            OnValueChanged.Invoke(index);
            SetChoice(index);
            Hide();
        }

        private void DestroyImmediateChoices()
        {
            if (_buttonCache == null)
                return;

            foreach (var button in _buttonCache)
            {
                GameObject.DestroyImmediate(button);
            }

            _buttonCache = Array.Empty<GameObject>();
        }

        private void ToggleShow()
        {
            if (Showing)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        private void Show()
        {
            if (_buttonCache == null || _buttonCache.Length == 0)
            {
                CreateChoices();
            }

            _template.SetActive(true);
            _template.GetComponent<Canvas>().overrideSorting = true;

            _buttonCache[_currentIndex].GetComponentInChildren<HGButton>().colors = _selectedColors;

        }

        private void Hide()
        {
            //Debug.Log($"ball");
            _template.SetActive(false);
        }

        private void CreateChoices()
        {
            _buttonCache = new GameObject[choices.Length];
            for (int i = 0; i < choices.Length; i++)
            {
                var button = GameObject.Instantiate(_dropDownChoicePrefab, _content.transform);

                button.GetComponentInChildren<HGTextMeshProUGUI>().SetText(choices[i]);

                button.name = choices[i];

                button.SetActive(true);

                button.GetComponentInChildren<RefreshCanvasDrawOrder>().canvasSortingOrderDelta = 30001;

                var hgButton = button.GetComponent<HGButton>();

                int index = i;
                hgButton.onClick.AddListener(delegate()
                {
                    SubmitChoice(index);
                });

                _buttonCache[i] = button;
            }
        }

        private void SetupTemplate()
        {
            _template = GameObject.Instantiate(PanelPrefab, transform);
            _template.name = "Drop Down Parent";
            _template.SetActive(false);

            GameObject.DestroyImmediate(_template.GetComponent<SettingsPanelController>());
            GameObject.DestroyImmediate(_template.GetComponent<HGButtonHistory>());
            GameObject.DestroyImmediate(_template.GetComponent<OnEnableEvent>());
            GameObject.DestroyImmediate(_template.GetComponent<UIJuice>());

            var templateCanvas = _template.GetOrAddComponent<Canvas>();

            templateCanvas.overrideSorting = true;
            templateCanvas.sortingOrder = 30000;

            _template.GetOrAddComponent<GraphicRaycaster>();
            _template.GetOrAddComponent<CanvasGroup>();

            var templateRectTransform = _template.GetComponent<RectTransform>();

            templateRectTransform.anchorMin = new Vector2(0, 0);
            templateRectTransform.anchorMax = new Vector2(1, 0);
            templateRectTransform.pivot = new Vector2(0.5f, 1);
            templateRectTransform.sizeDelta = new Vector2(0, 300f);
            templateRectTransform.anchoredPosition = Vector2.zero;

            var scrollbarRectTransform = _template.transform.Find("Scroll View").Find("Scrollbar Vertical").GetComponent<RectTransform>();

            scrollbarRectTransform.anchorMin = new Vector2(1, 0.02f);
            scrollbarRectTransform.anchorMax = new Vector2(1, 0.98f);
            scrollbarRectTransform.sizeDelta = new Vector2(24, 0);
            scrollbarRectTransform.anchoredPosition = new Vector2(-26, 0);

            _content = _template.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").gameObject;

            _content.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(4, 18, 4, 4);
        }

        private static void CreatePrefab()
        {
            _dropDownChoicePrefab = GameObject.Instantiate(CheckBoxPrefab);
            _dropDownChoicePrefab.SetActive(false);

            var hgButton = _dropDownChoicePrefab.GetComponent<HGButton>();
            hgButton.onClick.RemoveAllListeners();
            hgButton.disablePointerClick = false;

            GameObject.DestroyImmediate(_dropDownChoicePrefab.GetComponent<CarouselController>());
            GameObject.DestroyImmediate(_dropDownChoicePrefab.transform.Find("CarouselRect").gameObject);

            var dropDownLayoutElement = _dropDownChoicePrefab.GetComponent<LayoutElement>();

            dropDownLayoutElement.minHeight = 40;
            dropDownLayoutElement.preferredHeight = 40;
        }

        public class DropDownEvent : UnityEvent<int>
        {

        }
    }
}