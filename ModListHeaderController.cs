using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RiskOfOptions
{
    class ModListHeaderController : MonoBehaviour
    {
        private ModOptionPanelController mopc;
        private void OnEnable()
        {
            if (!GetComponentInParent<ModOptionPanelController>().initilized)
            {
                return;
            }

            if (!mopc)
                mopc = GetComponentInParent<ModOptionPanelController>();

            HGHeaderNavigationController navigationController = GetComponent<HGHeaderNavigationController>();

            navigationController.headerHighlightObject.transform.SetParent(transform);
            navigationController.headerHighlightObject.SetActive(false);

            if (navigationController.currentHeaderIndex >= 0)
            {
                navigationController.headers[navigationController.currentHeaderIndex].headerButton.interactable = true;
            }
        }
    }
}
