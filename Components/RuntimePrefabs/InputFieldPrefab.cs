using RiskOfOptions.Components.Options;
using RiskOfOptions.Resources;
using RoR2.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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