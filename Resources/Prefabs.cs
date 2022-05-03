using System.Reflection;
using RiskOfOptions.Components.AssetResolution;
using UnityEngine;

namespace RiskOfOptions.Resources
{
    public static class Prefabs
    {
        private static AssetBundle _uiBundle;

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
            
            boolButton = LoadPrefab("Assets/RiskOfOptions/prefabs/ModSettingsButton, Bool.prefab");
            sliderButton = LoadPrefab("Assets/RiskOfOptions/prefabs/ModSettingsButton, Slider.prefab");
            stepSliderButton = LoadPrefab("Assets/RiskOfOptions/prefabs/ModSettingsButton, Step Slider.prefab");
            intSliderButton = LoadPrefab("Assets/RiskOfOptions/prefabs/ModSettingsButton, Int Slider.prefab");
            inputFieldButton = LoadPrefab("Assets/RiskOfOptions/prefabs/ModSettingsButton, InputField.prefab");
            colorPickerButton = LoadPrefab("Assets/RiskOfOptions/prefabs/ModSettingsButton, ColorPicker.prefab");
            colorPickerOverlay = LoadPrefab("Assets/RiskOfOptions/prefabs/Color Picker Overlay.prefab");
        }

        private static GameObject LoadPrefab(string path)
        {
            var prefab = _uiBundle.LoadAsset<GameObject>(path);

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