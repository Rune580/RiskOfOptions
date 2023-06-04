using System;
using RoR2.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RiskOfOptions.Components.Panel
{
    public class ModListController : MonoBehaviour
    {
        private ModOptionPanelController _mopc;
        
        private void OnEnable()
        {
            if (!GetComponentInParent<ModOptionPanelController>().initialized)
            {
                return;
            }

            if (!_mopc)
                _mopc = GetComponentInParent<ModOptionPanelController>();

            HGHeaderNavigationController navigationController = GetComponent<HGHeaderNavigationController>();

            navigationController.headerHighlightObject.transform.SetParent(transform);
            navigationController.headerHighlightObject.SetActive(false);

            if (navigationController.currentHeaderIndex >= 0 && navigationController.headers != null)
            {
                navigationController.headers[navigationController.currentHeaderIndex].headerButton.interactable = true;
            }
        }
    }
}
