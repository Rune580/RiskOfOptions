using RiskOfOptions.Resources;
using UnityEngine;

namespace RiskOfOptions.Components.RuntimePrefabs
{
    public class GenericButtonPrefab : IRuntimePrefab
    {
        public GameObject GenericButton { get; private set; }

        public void Instantiate(GameObject settingsPanel)
        {
            GenericButton = Object.Instantiate(Prefabs.genericButton);
        }

        public void Destroy()
        {
            Object.DestroyImmediate(GenericButton);
        }
    }
}