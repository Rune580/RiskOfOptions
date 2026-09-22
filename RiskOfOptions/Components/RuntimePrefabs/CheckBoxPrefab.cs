using RiskOfOptions.Resources;
using UnityEngine;

namespace RiskOfOptions.Components.RuntimePrefabs
{
    public class CheckBoxPrefab : IRuntimePrefab
    {
        public GameObject CheckBoxButton { get; private set; }
        
        public void Instantiate(GameObject settingsPanel)
        {
            CheckBoxButton = Object.Instantiate(Prefabs.boolButton);
        }

        public void Destroy()
        {
            Object.DestroyImmediate(CheckBoxButton);
        }
    }
}