using RoR2.UI;
using UnityEngine.EventSystems;

namespace RiskOfOptions.Components.Panel
{
    public class ModListButton : HGButton
    {
        public string description = "";
        public string token;
        public LanguageTextMeshController nameLabel;
        public HGTextMeshProUGUI descriptionText;
        public ModOptionPanelController Mopc { get; internal set; }
        public string modGuid;
        public HGHeaderNavigationController navigationController;

        //private UnityEngine.UI.Image _icon;

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            //_icon.color = new Color(0.984f, 1, 0.69f, 0.73f);
            SetDescription();
        }

        protected override void Awake()
        {
            base.Awake();

            if (nameLabel)
                nameLabel.token = token;
        }

        protected override void Start()
        {
            base.Start();

            if (nameLabel)
                nameLabel.token = token;

            if (!Mopc)
                Mopc = GetComponentInParent<ModOptionPanelController>();

            //_icon = GetComponentInChildren<UnityEngine.UI.Image>();

            onClick.AddListener(delegate
            {
                navigationController.ChooseHeaderByButton(this);

                Mopc.LoadModOptionsFromOptionCollection(modGuid);
            });
        }

        private new void Update()
        {
            if (!eventSystem)
            {
                return;
            }
            if (!disableGamepadClick && eventSystem.player.GetButtonDown(14) && eventSystem.currentSelectedGameObject == gameObject)
            {
                InvokeClick();
            }
            if (defaultFallbackButton && eventSystem.currentInputSource == MPEventSystem.InputSource.Gamepad && eventSystem.currentSelectedGameObject == null && CanBeSelected())
            {
                Select();
            }
        }


        private void SetDescription()
        {
            if (!descriptionText || description == "")
                return;

            descriptionText.SetText(description);
        }
    }
}
