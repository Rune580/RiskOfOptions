using System.Reflection;
using UnityEngine;

namespace RiskOfOptions.Resources
{
    public static class Prefabs
    {
        // private static AssetBundle _subPanel;
        // public static GameObject SubPanel => _subPanel.LoadAsset<GameObject>("Assets/RiskOfOptions/prefabs/SubPanel.prefab");

        private static AssetBundle _uiBundle;
        public static GameObject BoolButton => _uiBundle.LoadAsset<GameObject>("Assets/RiskOfOptions/prefabs/ModSettingsButton, Bool.prefab"); 
        public static GameObject SliderButton => _uiBundle.LoadAsset<GameObject>("Assets/RiskOfOptions/prefabs/ModSettingsButton, Slider.prefab");
        public static GameObject StepSliderButton => _uiBundle.LoadAsset<GameObject>("Assets/RiskOfOptions/prefabs/ModSettingsButton, Step Slider.prefab");
        public static GameObject IntSliderButton => _uiBundle.LoadAsset<GameObject>("Assets/RiskOfOptions/prefabs/ModSettingsButton, Int Slider.prefab");
        public static GameObject InputFieldButton => _uiBundle.LoadAsset<GameObject>("Assets/RiskOfOptions/prefabs/ModSettingsButton, InputField.prefab");
        public static GameObject ColorPickerOverlay => _uiBundle.LoadAsset<GameObject>("Assets/RiskOfOptions/prefabs/Color Picker Overlay.prefab");

        internal static void Init()
        {
            // _subPanel = LoadBundle("subpanel");
            _uiBundle = LoadBundle("uielements");
        }

        private static AssetBundle LoadBundle(string name)
        {
            using var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"RiskOfOptions.Resources.AssetBundles.{name}");
            return AssetBundle.LoadFromStream(assetStream);
        }
    }
}