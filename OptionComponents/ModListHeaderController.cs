﻿using RoR2.UI;
using UnityEngine;

namespace RiskOfOptions.OptionComponents
{
    public class ModListHeaderController : MonoBehaviour
    {
        private ModOptionPanelController _mopc;
        private void OnEnable()
        {
            if (!GetComponentInParent<ModOptionPanelController>().initilized)
            {
                return;
            }

            if (!_mopc)
                _mopc = GetComponentInParent<ModOptionPanelController>();

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