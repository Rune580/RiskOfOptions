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
    public class RooDropdown : Selectable, IPointerClickHandler, ISubmitHandler, ICancelHandler
    {
        public GameObject choiceItemPrefab;
        public GameObject template;
        public GameObject content;
        public ColorBlock defaultColors;
        public bool allowAllEventSystems;
        public string[] choices;

        private MPEventSystemLocator _eventSystemLocator;
        private bool _isPointerInside;
        private SelectionState _previousState = SelectionState.Disabled;
        private LanguageTextMeshController _label;
        private int _currentIndex;
        private GameObject[] _buttons = Array.Empty<GameObject>();
        private ColorBlock _selectedColors;
        private bool _heldDown;

        private MPEventSystem EventSystem => _eventSystemLocator.eventSystem;
        private bool Showing => template && template.activeSelf;

        public DropDownEvent OnValueChanged { get; set; } = new();

        protected override void Awake()
        {
            base.Awake();

            colors = defaultColors;
            _eventSystemLocator = GetComponent<MPEventSystemLocator>();

            if (!_label)
                _label = transform.GetComponentInChildren<LanguageTextMeshController>();

            _selectedColors = defaultColors;
            _selectedColors.normalColor = new Color(0.3f, 0.3f, 0.3f, 1);
            _selectedColors.highlightedColor = new Color(0.3f, 0.3f, 0.3f, 1);
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
            EventSystem eventSystem = EventSystem;
            return eventSystem && inputModule == eventSystem.currentInputModule;
        }

        private void AttemptSelection(PointerEventData eventData)
        {
            if (EventSystem && EventSystem.currentInputModule == eventData.currentInputModule)
            {
                EventSystem.SetSelectedGameObject(gameObject, eventData);
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
            if (EventSystem && gameObject == EventSystem.currentSelectedGameObject)
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

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            if (_previousState == state)
                return;

            if (state == SelectionState.Highlighted)
                Util.PlaySound("Play_UI_menuHover", RoR2Application.instance.gameObject);
            
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
            bool cacheIsValid = _buttons != null && _buttons.Length != 0;

            if (cacheIsValid)
                _buttons[_currentIndex].GetComponentInChildren<HGButton>().colors = defaultColors;

            if (index >= choices.Length)
                return;

            _currentIndex = index;

            if (!_label)
                _label = transform.GetComponentInChildren<LanguageTextMeshController>();
            
            _label.token = choices[_currentIndex];

            if (cacheIsValid)
                _buttons[_currentIndex].GetComponentInChildren<HGButton>().colors = _selectedColors;
        }

        private void SubmitChoice(int index)
        {
            OnValueChanged.Invoke(index);
            SetChoice(index);
            Hide();
        }

        private void DestroyImmediateChoices()
        {
            if (_buttons == null)
                return;

            foreach (var button in _buttons)
                DestroyImmediate(button);

            _buttons = Array.Empty<GameObject>();
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
            if (_buttons.Length == 0)
                CreateChoices();

            template.SetActive(true);
            template.GetComponent<Canvas>().overrideSorting = true;

            _buttons[_currentIndex].GetComponentInChildren<HGButton>().colors = _selectedColors;

        }

        private void Hide()
        {
            template.SetActive(false);
        }

        private void CreateChoices()
        {
            if (_buttons is { Length: > 0 })
            {
                foreach (var button in _buttons)
                    DestroyImmediate(button);
            }
            
            _buttons = new GameObject[choices.Length];
            
            for (int i = 0; i < choices.Length; i++)
            {
                var button = Instantiate(choiceItemPrefab, content.transform);

                button.GetComponentInChildren<LanguageTextMeshController>().token = choices[i];

                button.name = choices[i];

                button.SetActive(true);

                button.GetComponentInChildren<RefreshCanvasDrawOrder>().canvasSortingOrderDelta = 30001;

                var hgButton = button.GetComponent<HGButton>();

                int index = i;
                hgButton.onClick.AddListener(delegate()
                {
                    SubmitChoice(index);
                });

                _buttons[i] = button;
            }
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            DestroyImmediateChoices();
        }

        // private void SetupTemplate()
        // {
            // _template = GameObject.Instantiate(panelPrefab, transform);
            // _template.name = "Drop Down Parent";
            // _template.SetActive(false);
            //
            // GameObject.DestroyImmediate(_template.GetComponent<SettingsPanelController>());
            // GameObject.DestroyImmediate(_template.GetComponent<HGButtonHistory>());
            // GameObject.DestroyImmediate(_template.GetComponent<OnEnableEvent>());
            // GameObject.DestroyImmediate(_template.GetComponent<UIJuice>());
            //
            // var templateCanvas = _template.GetOrAddComponent<Canvas>();
            //
            // templateCanvas.overrideSorting = true;
            // templateCanvas.sortingOrder = 30000;
            //
            // _template.GetOrAddComponent<GraphicRaycaster>();
            // _template.GetOrAddComponent<CanvasGroup>();
            //
            // var templateRectTransform = _template.GetComponent<RectTransform>();
            //
            // templateRectTransform.anchorMin = new Vector2(0, 0);
            // templateRectTransform.anchorMax = new Vector2(1, 0);
            // templateRectTransform.pivot = new Vector2(0.5f, 1);
            // templateRectTransform.sizeDelta = new Vector2(0, 300f);
            // templateRectTransform.anchoredPosition = Vector2.zero;
            //
            // var scrollbarRectTransform = _template.transform.Find("Scroll View").Find("Scrollbar Vertical").GetComponent<RectTransform>();
            //
            // scrollbarRectTransform.anchorMin = new Vector2(1, 0.02f);
            // scrollbarRectTransform.anchorMax = new Vector2(1, 0.98f);
            // scrollbarRectTransform.sizeDelta = new Vector2(24, 0);
            // scrollbarRectTransform.anchoredPosition = new Vector2(-26, 0);
            //
            // _content = _template.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").gameObject;
            //
            // _content.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(4, 18, 4, 4);
        // }

        // private static void CreatePrefab()
        // {
        //     choiceItemPrefab = GameObject.Instantiate(CheckBoxPrefab);
        //     choiceItemPrefab.SetActive(false);
        //
        //     var hgButton = choiceItemPrefab.GetComponent<HGButton>();
        //     hgButton.onClick.RemoveAllListeners();
        //     hgButton.disablePointerClick = false;
        //
        //     GameObject.DestroyImmediate(choiceItemPrefab.GetComponent<CarouselController>());
        //     GameObject.DestroyImmediate(choiceItemPrefab.transform.Find("CarouselRect").gameObject);
        //
        //     var dropDownLayoutElement = choiceItemPrefab.GetComponent<LayoutElement>();
        //
        //     dropDownLayoutElement.minHeight = 40;
        //     dropDownLayoutElement.preferredHeight = 40;
        // }

        public class DropDownEvent : UnityEvent<int>
        {

        }
    }
}