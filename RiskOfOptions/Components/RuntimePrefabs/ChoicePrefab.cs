using RiskOfOptions.Resources;
using UnityEngine;

namespace RiskOfOptions.Components.RuntimePrefabs;

public class ChoicePrefab : IRuntimePrefab
{
    public GameObject ChoiceButton { get; private set; }

    public void Instantiate(GameObject settingsPanel)
    {
        ChoiceButton = Object.Instantiate(Prefabs.enumDropDownButton);
    }

    public void Destroy()
    {
        Object.DestroyImmediate(ChoiceButton);
    }
}