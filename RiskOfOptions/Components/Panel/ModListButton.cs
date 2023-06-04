using RoR2.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        public Image modIcon;

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            SetDescription();
        }
        public override void Awake()
        {
            base.Awake();

            if (nameLabel)
                nameLabel.token = token;

            if (!modIcon)
                return;

            Sprite icon = ModSettingsManager.OptionCollection[modGuid].icon;

            if (!icon)
                return;
            
            modIcon.sprite = icon;
        }

        public override void Start()
        {
            base.Start();

            if (nameLabel)
                nameLabel.token = token;

            if (!Mopc)
                Mopc = GetComponentInParent<ModOptionPanelController>();
            
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
