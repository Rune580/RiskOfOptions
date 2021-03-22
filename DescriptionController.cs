using R2API;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RiskOfOptions
{
    class DescriptionController : MonoBehaviour
    {
        private GameObject GenericDescriptionPanel;
        void OnEnable()
        {
            if (!GenericDescriptionPanel)
                GenericDescriptionPanel = GameObject.Find("GenericDescriptionPanel").gameObject;

            GenericDescriptionPanel.SetActive(false);
        }

        void OnDisable()
        {
            GenericDescriptionPanel.SetActive(true);
        }
    }
}
