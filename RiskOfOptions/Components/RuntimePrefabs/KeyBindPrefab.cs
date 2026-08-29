using RiskOfOptions.Resources;
using UnityEngine;

namespace RiskOfOptions.Components.RuntimePrefabs
{
    public class KeyBindPrefab : IRuntimePrefab
    {
        public GameObject KeyBind { get; private set; }

        public void Instantiate(GameObject settingsPanel)
        {
            KeyBind = Object.Instantiate(Prefabs.keyBindButton);
        }
        
        public void Destroy()
        {
            Object.DestroyImmediate(KeyBind);
        }
    }
}