using System;
using UnityEditor;
using UnityEngine;

namespace RoOToolkit.Editor.ContextActions
{
    public class CloneGameObjectAction : EditorWindow
    {
        private static GameObject _reference;
        private GameObject _targetParent;

        [MenuItem("GameObject/RoO-Toolkit/CloneFromAddressable", true)]
        public static bool Validation()
        {
            return Selection.activeGameObject != null;
        }

        [MenuItem("GameObject/RoO-Toolkit/CloneFromAddressable")]
        public static void ShowWindow()
        {
            _reference = Selection.activeGameObject;
            var window = GetWindow<CloneGameObjectAction>();
            window.Show();
        }

        private void OnGUI()
        {
            _targetParent = EditorGUILayout.ObjectField("Target Parent", _targetParent, typeof(GameObject), true) as GameObject;

            if (GUILayout.Button("Clone"))
                RecursiveClone(_reference, _targetParent);
        }

        private void RecursiveClone(GameObject original, GameObject targetParent)
        {
            var clone = new GameObject(original.name)
            {
                layer = original.layer
            };

            if (targetParent)
                clone.transform.SetParent(targetParent.transform, false);

            foreach (var component in original.GetComponents<Component>())
            {
                UnityEditorInternal.ComponentUtility.CopyComponent(component);
                UnityEditorInternal.ComponentUtility.PasteComponentAsNew(clone);
            }

            for (int i = 0; i < original.transform.childCount; i++)
                RecursiveClone(original.transform.GetChild(i).gameObject, clone);
        }
    }
}
