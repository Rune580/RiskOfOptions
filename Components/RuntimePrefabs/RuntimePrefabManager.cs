using System;
using System.Collections.Generic;
using R2API.MiscHelpers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RiskOfOptions.Components.RuntimePrefabs
{
    public static class RuntimePrefabManager
    {
        private static readonly Dictionary<Type, IRuntimePrefab> Prefabs = new();
        private static readonly Dictionary<Type, GameObject> GameObjects = new();

        public static void Register<T>() where T : IRuntimePrefab, new()
        {
            T runtimePrefab = new T();
            
            Prefabs.Add(runtimePrefab.GetType(), runtimePrefab);
        }

        public static T Get<T>() where T : IRuntimePrefab
        {
            return (T)Prefabs[typeof(T)];
        }

        internal static void InitializePrefabs(GameObject panel)
        {
            foreach (var (type, runtimePrefab) in Prefabs)
            {
                runtimePrefab.Instantiate(panel);

                GameObjects[type] = runtimePrefab.GetRoot();
            }
        }

        internal static void DestroyPrefabs()
        {
            foreach (var (_, instance) in GameObjects)
            {
                Object.DestroyImmediate(instance);
            }
        }
    }
}