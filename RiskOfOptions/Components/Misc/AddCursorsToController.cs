using System;
using System.Collections.Generic;
using RiskOfOptions.Lib;
using RoR2.UI;
using UnityEngine;

namespace RiskOfOptions.Components.Misc;

/// <summary>
/// Single-frame component that adds additional cursors from RoO to the CursorIndicatorController.
/// </summary>
public class AddCursorsToController : MonoBehaviour
{
    private static readonly List<GameObject> CursorPrefabs = new();

    public static void AddCursor(GameObject prefab)
    {
        CursorPrefabs.Add(prefab);
    }

    private void Awake()
    {
        AddCursors();
    }

    private void Start()
    {
        AddCursors();
    }

    private void OnEnable()
    {
        AddCursors();
    }

    private void AddCursors()
    {
        var indicatorController = GetComponent<CursorIndicatorController>();
        
        foreach (var prefab in CursorPrefabs)
        {
            var instance = Instantiate(prefab, indicatorController.containerTransform);
            instance.SetActive(false);

            CursorController.Cursors[prefab.name] = instance;
        }

        DestroyImmediate(this);
    }
}