using RiskOfOptions.Resources;
using UnityEngine;

namespace RiskOfOptions.Components.RuntimePrefabs
{
    public class InputFieldPrefab : IRuntimePrefab
    {
        public GameObject InputField { get; private set; }

        public void Instantiate(GameObject settingsPanel)
        {
            InputField = Object.Instantiate(Prefabs.inputFieldButton);
            InputField.SetActive(false);
        }
        
        public void Destroy()
        {
            Object.DestroyImmediate(InputField);
        }
    }
}