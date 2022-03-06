using RoR2.UI;
using UnityEngine.EventSystems;

namespace RiskOfOptions.Components.OptionComponents
{
    public class RooModListButton : HGButton
    {
        public string description = "";
        public HGTextMeshProUGUI tmp;
        public ModOptionPanelController Mopc { get; internal set; }
        public int containerIndex;
        public HGHeaderNavigationController navigationController;

        //private UnityEngine.UI.Image _icon;

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            //_icon.color = new Color(0.984f, 1, 0.69f, 0.73f);
            SetDescription();
        }

        protected override void Start()
        {
            base.Start();

            if (!Mopc)
                Mopc = GetComponentInParent<ModOptionPanelController>();

            //_icon = GetComponentInChildren<UnityEngine.UI.Image>();

            base.onClick.AddListener(new UnityEngine.Events.UnityAction(
                delegate ()
                {
                    navigationController.ChooseHeaderByButton(this);

                    Mopc.LoadModOptionsFromContainer(containerIndex, transform.parent.parent.parent.parent.parent);
                }));
        }

        private new void Update()
        {
            if (!this.eventSystem || this.eventSystem.player == null)
            {
                return;
            }
            if (!this.disableGamepadClick && this.eventSystem.player.GetButtonDown(14) && this.eventSystem.currentSelectedGameObject == base.gameObject)
            {
                this.InvokeClick();
            }
            if (this.defaultFallbackButton && this.eventSystem.currentInputSource == MPEventSystem.InputSource.Gamepad && this.eventSystem.currentSelectedGameObject == null && this.CanBeSelected())
            {
                this.Select();
            }
        }


        private void SetDescription()
        {
            if (!tmp || description == "")
                return;

            tmp.SetText(description);
        }
    }
}
