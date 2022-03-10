using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RiskOfOptions.Components.Options
{
    public class RooInputField : TMP_InputField
    {
        public GameObject overlay;

        private RooInputFieldOverlay _inputFieldOverlay;
        private bool Showing => overlay.activeSelf;

        private bool _inField = false;

        private bool _inOverlay = false;

        private bool PointerInside => _inField || _inOverlay;

        private bool _exitQueued = false;

        private new void Awake()
        {
            base.Awake();

            onSubmit.AddListener(AttemptHide);

            _inputFieldOverlay = overlay.GetComponent<RooInputFieldOverlay>();

            _inputFieldOverlay.OnPointerEnterCanvas.AddListener(OnPointerEnterCanvas);
            _inputFieldOverlay.OnPointerExitCanvas.AddListener(OnPointerExitCanvas);
            _inputFieldOverlay.OnPointerDownCanvas.AddListener(OnPointerDown);
            _inputFieldOverlay.OnBeginDragCanvas.AddListener(OnBeginDrag);
            _inputFieldOverlay.OnDragCanvas.AddListener(OnDrag);
            _inputFieldOverlay.OnEndDragCanvas.AddListener(OnEndDrag);
        }

        public void Update()
        {
            bool submitKey = Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter);
            bool validKey = Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Escape);

            if (validKey && !PointerInside)
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

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            AttemptShow();
        }

        private void OnPointerEnterCanvas()
        {
            _inOverlay = true;
        }

        private void OnPointerExitCanvas()
        {
            _inOverlay = false;
        }

        public override void OnPointerEnter(PointerEventData pointerEventData)
        {
            _inField = true;
            
            base.OnPointerEnter(pointerEventData);
        }

        public override void OnPointerExit(PointerEventData pointerEventData)
        {
            _inField = false;

            base.OnPointerExit(pointerEventData);
        }

        private void AttemptShow()
        {
            if (!overlay)
                return;

            if (Showing)
                return;

            _exitQueued = false;

            ShowInputField();
        }

        private void AttemptHide()
        {
            if (!overlay)
                return;

            if (!Showing)
                return;

            _exitQueued = false;

            HideInputField();
        }

        private void AttemptHide(string input)
        {
            AttemptHide();
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
    }
}