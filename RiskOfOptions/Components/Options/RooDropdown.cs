using RoR2;
using RoR2.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RiskOfOptions.Components.Options;

/// <summary>
/// This is basically a reimplementation of MPDropdown with my own methods of handling the dropdown.
/// After all... why not?
/// Why shouldn't I change it?
/// </summary>
[RequireComponent(typeof(HGButton))]
public class RooDropdown : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISubmitHandler, ICancelHandler
{
    public HGButton? dropDownButton;
    public GameObject? choiceItemPrefab;
    public GameObject? template;
    public GameObject? content;
    public ColorBlock defaultColors;
    public string[] choices = [];

    private MPEventSystemLocator? _eventSystemLocator;
    private bool _isPointerInside;
    private LanguageTextMeshController? _label;
    private int _currentIndex;
    private GameObject[] _buttons = [];
    private ColorBlock _selectedColors;
    private bool _heldDown;

    private MPEventSystem EventSystem => _eventSystemLocator!.eventSystem;
    private bool Showing => template && template!.activeSelf;

    public DropDownEvent OnValueChanged { get; set; } = new();

    public bool Interactable
    {
        get => dropDownButton!.interactable;
        set => dropDownButton!.interactable = value;
    }

    protected void Awake()
    {
        _eventSystemLocator = GetComponent<MPEventSystemLocator>();

        if (!dropDownButton)
            dropDownButton = GetComponent<HGButton>();

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

        var validKey = Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Escape);

        var validKeyReleased = Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.Escape);

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!EventSystem || EventSystem.currentInputModule != eventData.currentInputModule)
            return;
            
        _isPointerInside = true;
    }
        
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!EventSystem || EventSystem.currentInputModule != eventData.currentInputModule)
            return;
            
        if (gameObject == EventSystem.currentSelectedGameObject)
        {
            enabled = false;
            enabled = true;
        }
        _isPointerInside = false;
    }

    public void OnDisable()
    {
        _isPointerInside = false;
    }
    
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
        if (_buttons.Length > 0)
            _buttons[_currentIndex].GetComponentInChildren<HGButton>().colors = defaultColors;

        if (index >= choices.Length)
            return;

        _currentIndex = index;

        if (!_label)
            _label = transform.GetComponentInChildren<LanguageTextMeshController>();
            
        _label!.token = choices[_currentIndex];

        if (_buttons.Length > 0)
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
        foreach (var button in _buttons)
            DestroyImmediate(button);

        _buttons = [];
    }

    public void ToggleShow()
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

        template!.SetActive(true);
        // template.GetComponent<Canvas>().sortingOrder = 6;

        _buttons[_currentIndex].GetComponentInChildren<HGButton>().colors = _selectedColors;

    }

    private void Hide()
    {
        template!.SetActive(false);
    }

    private void CreateChoices()
    {
        if (_buttons is { Length: > 0 })
        {
            foreach (var button in _buttons)
                DestroyImmediate(button);
        }
            
        _buttons = new GameObject[choices.Length];
            
        for (var i = 0; i < choices.Length; i++)
        {
            var button = Instantiate(choiceItemPrefab, content!.transform)!;

            button.GetComponentInChildren<LanguageTextMeshController>().token = choices[i];

            button.name = choices[i];

            button.SetActive(true);

            button.GetComponentInChildren<RefreshCanvasDrawOrder>().canvasSortingOrderDelta = 30001;

            var hgButton = button.GetComponent<HGButton>();

            var index = i;
            hgButton.onClick.AddListener(delegate()
            {
                SubmitChoice(index);
            });

            _buttons[i] = button;
        }
    }

    public void OnDestroy()
    {
        DestroyImmediateChoices();
    }

    public class DropDownEvent : UnityEvent<int>;
}