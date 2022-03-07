using UnityEngine;

namespace RiskOfOptions.Components.RuntimePrefabs
{
    public interface IRuntimePrefab
    {
        void Instantiate(GameObject settingsPanel);

        void Destroy();
    }
}