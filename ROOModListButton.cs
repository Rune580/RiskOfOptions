using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RiskOfOptions
{
    public class ROOModListButton : HGButton
    {
        public string Description = "";
        public HGTextMeshProUGUI tmp;
        public ModOptionPanelController mopc { get; internal set; }
        public int ContainerIndex;
        public HGHeaderNavigationController navigationController;

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            SetDescription();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void Start()
        {
            base.Start();

            if (!mopc)
                mopc = GetComponentInParent<ModOptionPanelController>();

            base.onClick.AddListener(new UnityEngine.Events.UnityAction(
                delegate ()
                {
                    navigationController.ChooseHeaderByButton(this);

                    mopc.LoadModOptionsFromContainer(ContainerIndex, transform.parent.parent.parent.parent.parent);
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
            if (!tmp || Description == "")
                return;

            tmp.SetText(Description);
        }
    }
}
