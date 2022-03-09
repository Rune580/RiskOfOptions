using RiskOfOptions.Components.Options;
using RoR2;
using RoR2.UI;
using UnityEngine;

namespace RiskOfOptions.Components.RuntimePrefabs
{
    public class KeyBindPrefab : IRuntimePrefab
    {
        public GameObject KeyBind { get; private set; }
        
        public void Instantiate(GameObject settingsPanel)
        {
            GameObject gameObject = settingsPanel.transform.Find("SafeArea").Find("SubPanelArea").Find("SettingsSubPanel, Controls (M&KB)")
                .Find("Scroll View").Find("Viewport").Find("VerticalLayout").Find("SettingsEntryButton, Binding (Jump)").gameObject;

            KeyBind = Object.Instantiate(gameObject);
            KeyBind.name = "Mod Option Prefab, KeyBind";
            
            var inputBindingControl = KeyBind.GetComponentInChildren<InputBindingControl>();
            var controller = KeyBind.AddComponent<KeybindController>();

            controller.nameLabel = inputBindingControl.nameLabel;
            
            Object.DestroyImmediate(inputBindingControl);
            Object.DestroyImmediate(KeyBind.GetComponentInChildren<InputBindingDisplayController>());

            foreach (var button in KeyBind.GetComponentsInChildren<HGButton>())
                button.onClick.RemoveAllListeners();
        }

        public void Destroy()
        {
            Object.DestroyImmediate(KeyBind);
        }
    }
}