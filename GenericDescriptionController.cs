using R2API;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RiskOfOptions
{
    class GenericDescriptionController : MonoBehaviour
    {
        private GameObject GenericDescriptionPanel;
        public ModOptionPanelController mopc { get; internal set; }
        private Transform canvas;
        void OnEnable()
        {
            if (!GenericDescriptionPanel)
                GenericDescriptionPanel = GameObject.Find("GenericDescriptionPanel");

            GenericDescriptionPanel.SetActive(false);
        }

        void OnDisable()
        {
            if (!mopc)
                mopc = GetComponentInParent<ModOptionPanelController>();

            if (!mopc.initilized)
            {
                return;
            }

            if (GenericDescriptionPanel)
            {
                GenericDescriptionPanel.SetActive(true);
            }

            if (!canvas)
                canvas = GenericDescriptionPanel.transform.parent.Find("SettingsSubPanel, Mod Options(Clone)");

            mopc.UnLoad(canvas);
        }
    }
}
