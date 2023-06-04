using System;
using System.Collections.Generic;
using RiskOfOptions.Lib;
using UnityEngine;

namespace RiskOfOptions.Components.RuntimePrefabs
{
    public static class RuntimePrefabManager
    {
        private static readonly Dictionary<Type, IRuntimePrefab> Prefabs = new();

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
            foreach (var (_, runtimePrefab) in Prefabs)
            {
                runtimePrefab.Instantiate(panel);
            }
        }

        internal static void DestroyPrefabs()
        {
            foreach (var (_, runtimePrefab) in Prefabs)
            {
                runtimePrefab.Destroy();
            }
        }
    }
}