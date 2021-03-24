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

        internal bool Selected = false;

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
                    mopc.LoadModOptionsFromContainer(ContainerIndex, transform.parent.parent.parent.parent.parent);

                    mopc.ResetModListButtons(transform.parent.parent.parent.parent.parent);

                    Selected = true;
                    showImageOnHover = false;

                    Color color = imageOnHover.color;

                    imageOnHover.color = new Color(color.r, color.g, color.b, 1f);
                    imageOnHover.transform.localScale = new Vector3(1f, 1f, 1f);
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

            interactable = !Selected;
        }

        //public override void OnPointerClick(PointerEventData eventData)
        //{
        //    base.OnPointerClick(eventData);

        //    Debug.Log("1");

        //    test();

        //    Debug.Log("2");
        //}

        private void SetDescription()
        {
            if (!tmp || Description == "")
                return;

            tmp.SetText(Description);
        }
    }
}
