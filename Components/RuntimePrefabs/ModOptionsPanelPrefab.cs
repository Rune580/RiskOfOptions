using RoR2.UI;
using RoR2.UI.SkinControllers;
using UnityEngine;

namespace RiskOfOptions.Components.RuntimePrefabs
{
    public class ModOptionsPanelPrefab : IRuntimePrefab
    {
        public GameObject Root { get; private set; }
        public GameObject GenericDescriptionPanel { get; private set; }
        public GameObject ModOptionsHeaderButton { get; private set; }
        public GameObject VerticalLayout { get; private set; }
        public GameObject ModButton { get; private set; }
        
        
        public void Instantiate(GameObject settingsPanel)
        {
            Transform transform = settingsPanel.transform;
            Transform subPanelArea = transform.Find("SafeArea").Find("SubPanelArea");
            Transform headerArea = transform.Find("SafeArea").Find("HeaderContainer").Find("Header (JUICED)");
            
            CreateRoot(subPanelArea);
            CreateGenericDescriptionPanel(subPanelArea);
            CreateModOptionsHeaderButton(headerArea);
            CreateVerticalLayout();
        }

        public GameObject GetRoot()
        {
            return Root;
        }

        private void CreateRoot(Transform subPanelArea)
        {
            GameObject audioPanel = subPanelArea.Find("SettingsSubPanel, Audio").gameObject;

            Root = Object.Instantiate(audioPanel);
            Root.name = "SettingsSubPanel, Mod Options";
        }

        private void CreateGenericDescriptionPanel(Transform subPanelArea)
        {
            GenericDescriptionPanel = Object.Instantiate(subPanelArea.Find("GenericDescriptionPanel").gameObject);
            
            Object.DestroyImmediate(GenericDescriptionPanel.GetComponentInChildren<DisableIfTextIsEmpty>());
            Object.DestroyImmediate(GenericDescriptionPanel.GetComponentInChildren<LanguageTextMeshController>());
            Object.DestroyImmediate(GenericDescriptionPanel.transform.Find("ContentSizeFitter").Find("BlurPanel").gameObject);
            Object.DestroyImmediate(GenericDescriptionPanel.transform.Find("ContentSizeFitter").Find("CornerRect").gameObject);
        }

        private void CreateModOptionsHeaderButton(Transform headerArea)
        {
            ModOptionsHeaderButton = Object.Instantiate(headerArea.Find("GenericHeaderButton (Audio)").gameObject);
            ModOptionsHeaderButton.name = "GenericHeaderButton (Mod Options)";
        }

        private void CreateVerticalLayout()
        {
            VerticalLayout = Root.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").gameObject;
        }

        private void CreateModButton()
        {
            ModButton = Object.Instantiate(VerticalLayout.transform.Find("SettingsEntryButton, Bool (Audio Focus)").gameObject);
            
            Object.DestroyImmediate(ModButton.GetComponentInChildren<CarouselController>());
            Object.DestroyImmediate(ModButton.GetComponentInChildren<ButtonSkinController>());
            Object.DestroyImmediate(ModButton.transform.Find("CarouselRect").gameObject);
        }
    }
}