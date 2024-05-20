using RiskOfOptions.Resources;
using UnityEngine;

namespace RiskOfOptions.Components.RuntimePrefabs;

public class FloatFieldPrefab : IRuntimePrefab
{
    public GameObject FloatField { get; private set; }
    
    public void Instantiate(GameObject settingsPanel)
    {
        FloatField = Object.Instantiate(Prefabs.floatFieldButton);
    }

    public void Destroy()
    {
        Object.DestroyImmediate(FloatField);
    }
}