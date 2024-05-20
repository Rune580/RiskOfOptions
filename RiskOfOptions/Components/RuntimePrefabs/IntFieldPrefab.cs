using RiskOfOptions.Resources;
using UnityEngine;

namespace RiskOfOptions.Components.RuntimePrefabs;

public class IntFieldPrefab : IRuntimePrefab
{
    public GameObject IntField { get; private set; }
    
    public void Instantiate(GameObject settingsPanel)
    {
        IntField = Object.Instantiate(Prefabs.intFieldButton);
    }

    public void Destroy()
    {
        Object.DestroyImmediate(IntField);
    }
}