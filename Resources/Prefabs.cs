using System.Reflection;
using RiskOfOptions.Components.AssetResolution;
using UnityEngine;

namespace RiskOfOptions.Resources
{
    public static class Prefabs
    {
        private static AssetBundle _uiBundle;

        public static GameObject modOptionsPanel;
        public static GameObject modListPanel;
        public static GameObject modDescriptionPanel;

        public static GameObject boolButton;
        public static GameObject sliderButton;
        public static GameObject stepSliderButton;
        public static GameObject intSliderButton;
        public static GameObject inputFieldButton;
        public static GameObject colorPickerButton;
        
        public static GameObject colorPickerOverlay;

        internal static void Init()
        {
            _uiBundle = LoadBundle("uielements");

            modOptionsPanel = LoadPrefab("Options Panel.prefab");
            modListPanel = LoadPrefab("Mod List Panel.prefab");
            modDescriptionPanel = LoadPrefab("Mod Description Panel.prefab");
            
            boolButton = LoadPrefab("ModSettingsButton, Bool.prefab");
            sliderButton = LoadPrefab("ModSettingsButton, Slider.prefab");
            stepSliderButton = LoadPrefab("ModSettingsButton, Step Slider.prefab");
            intSliderButton = LoadPrefab("ModSettingsButton, Int Slider.prefab");
            inputFieldButton = LoadPrefab("ModSettingsButton, InputField.prefab");
            colorPickerButton = LoadPrefab("ModSettingsButton, ColorPicker.prefab");
            colorPickerOverlay = LoadPrefab("Color Picker Overlay.prefab");
        }

        private static GameObject LoadPrefab(string path)
        {
            var prefab = _uiBundle.LoadAsset<GameObject>($"Assets/RiskOfOptions/prefabs/{path}");

            foreach (var resolver in prefab.GetComponentsInChildren<AssetResolver>())
                resolver.AttemptResolve();

            return prefab;
        }
            
        private static AssetBundle LoadBundle(string name)
        {
            using var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"RiskOfOptions.Resources.AssetBundles.{name}");
            return AssetBundle.LoadFromStream(assetStream);
        }
    }
}