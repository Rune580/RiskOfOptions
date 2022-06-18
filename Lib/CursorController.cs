using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MonoMod.RuntimeDetour;
using RiskOfOptions.Components.Misc;
using RoR2.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static RoR2.UI.CursorIndicatorController;

namespace RiskOfOptions.Lib;

/// <summary>
/// Contains helper methods for modifying the cursor.
/// </summary>
internal static class CursorController
{
    private static Hook _controllerHook;
    private static CursorSet _origMouse;
    private static CursorSet _origGamepad;
    private static CursorSet _cursorSet = new();
    
    private static CursorIndicatorController _indicatorControllerInstance;

    private static CursorIndicatorController IndicatorController
    {
        get
        {
            if (!_indicatorControllerInstance)
            {
                _indicatorControllerInstance = MPEventSystem.readOnlyInstancesList
                    .FirstOrDefault(instance => instance.cursorIndicatorController != null)?.cursorIndicatorController;
            }

            return _indicatorControllerInstance;
        }
    }

    internal static readonly Dictionary<string, GameObject> Cursors = new();

    internal static void Init()
    {
        InitHooks();
        LoadCursors();
        ModifyCursorIndicatorController();
    }

    private static void InitHooks()
    {
        var destMethod = typeof(CursorController).GetMethod(nameof(SetCursorOverride), BindingFlags.NonPublic | BindingFlags.Static);
        _controllerHook = HookHelper.NewHook<CursorIndicatorController>(nameof(CursorIndicatorController.SetCursor), destMethod);
    }

    private static void LoadCursors()
    {
        // Todo actually have cursors to load here
    }

    private static void ModifyCursorIndicatorController()
    {
        GameObject controllerPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/UI/CursorIndicator.prefab").WaitForCompletion();
        controllerPrefab.AddComponent<AddCursorsToController>();

        var controller = controllerPrefab.GetComponent<CursorIndicatorController>();
        _origMouse = controller.mouseCursorSet;
        _origGamepad = controller.gamepadCursorSet;
    }

    private static void SetCursorOverride(Action<CursorIndicatorController, CursorSet, CursorImage, Color> orig,
        CursorIndicatorController self, CursorSet set, CursorImage image, Color color)
    {
        orig(self, set, image, color);
    }
}