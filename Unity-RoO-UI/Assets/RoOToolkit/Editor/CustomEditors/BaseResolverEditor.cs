using System;
using System.Collections.Generic;
using RiskOfOptions.Components.AssetResolution;
using RiskOfOptions.Components.AssetResolution.Data;
using SimpleJSON;
using UnityEditor;
using UnityEngine;

namespace RoOToolkit.Editor.CustomEditors
{
    public abstract class BaseResolverEditor<TAssetResolver, TEntry, TComponent> : UnityEditor.Editor
        where TAssetResolver : MultiAssetResolver<TEntry>
        where TComponent : Component
        where TEntry : BaseAssetEntry<TComponent>, ISerializableEntry, new()
    {
        protected TComponent RefObj;
        protected TComponent TargetObj;
        protected bool[] ShowArray = Array.Empty<bool>();

        protected bool ShowDangerZone;
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var resolver = (TAssetResolver)target;

            DrawObjectFields(resolver);
            DrawBeforeAddElements(resolver);
            DrawAddElements(resolver);
            DrawManipulationElements(resolver);
            DrawDangerZone(resolver);
            DrawEntries(resolver);

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawObjectFields(TAssetResolver resolver)
        {
            GUILayout.BeginVertical();
            RefObj = EditorGUILayout.ObjectField("Reference", RefObj, typeof(TComponent), true) as TComponent;
            TargetObj = EditorGUILayout.ObjectField("Target", TargetObj, typeof(TComponent), true) as TComponent;
            GUILayout.EndVertical();
        }
        
        protected virtual void DrawBeforeAddElements(TAssetResolver resolver) { }

        protected virtual void DrawAddElements(TAssetResolver resolver)
        {
            GUILayout.BeginHorizontal();
            bool add = GUILayout.Button("Add");
            bool paste = GUILayout.Button("Paste Entry");
            bool prune = GUILayout.Button("Prune Entries");
            GUILayout.EndHorizontal();
            

            if (add && RefObj && TargetObj)
            {
                if (resolver.entries == null)
                    resolver.entries = new List<TEntry>();
                
                Undo.RecordObject(resolver, "Resolver entry added");
                
                AddEntry(resolver);
                
                PrefabUtility.RecordPrefabInstancePropertyModifications(resolver);
            }

            if (paste)
                PasteEntryFromClipboard(resolver);

            if (prune)
                PruneEntries(resolver);
        }

        protected virtual void DrawManipulationElements(TAssetResolver resolver)
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Resolve"))
            {
                if (resolver.entries != null)
                    ResolveAssets(resolver);
            }
            
            if (GUILayout.Button("Reset"))
            {
                if (resolver.entries != null)
                    ResetAssets(resolver);
            }

            GUILayout.EndHorizontal();
        }

        protected virtual void DrawDangerZone(TAssetResolver resolver)
        {
            ShowDangerZone = EditorGUILayout.Foldout(ShowDangerZone, "Danger Zone");

            if (!ShowDangerZone)
                return;
            
            
            GUILayout.BeginHorizontal();
            bool copy = GUILayout.Button("Copy Entries");
            bool paste = GUILayout.Button("Paste Entries");
            GUILayout.EndHorizontal();

            if (copy)
                CopyEntriesToClipboard(resolver);

            if (paste)
                PasteEntriesFromClipboard(resolver);
        }

        protected virtual void DrawEntries(TAssetResolver resolver)
        {
            if (resolver.entries is null)
                return;

            if (resolver.entries.Count != ShowArray.Length)
                Array.Resize(ref ShowArray, resolver.entries.Count);
            
            GUILayout.Label($"{resolver.entries.Count} Entries:");

            for (int i = 0; i < resolver.entries.Count; i++)
            {
                var entry = resolver.entries[i];

                int indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 2;

                ShowArray[i] = EditorGUILayout.Foldout(ShowArray[i], $"Entry {i + 1}");

                if (ShowArray[i])
                {
                    DrawEntry(resolver, entry, i);
                    
                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button("Copy"))
                    {
                        CopyEntryToClipboard(entry);
                    }
                    else if (GUILayout.Button("Remove"))
                    {
                        Undo.RecordObject(resolver, "Resolver entry removed");
                        
                        resolver.entries.Remove(entry);
                        
                        PrefabUtility.RecordPrefabInstancePropertyModifications(resolver);
                    }
                    
                    GUILayout.EndHorizontal();
                        
                }

                EditorGUI.indentLevel = indent;
            }
        }

        private void CopyEntriesToClipboard(TAssetResolver resolver)
        {
            byte[] bytes = resolver.serializedData;
            
            JSONArray array = new JSONArray();

            foreach (var b in bytes)
            {
                array.Add(b);
            }

            EditorGUIUtility.systemCopyBuffer = array.ToString();
        }

        private void CopyEntryToClipboard(TEntry entry)
        {
            var hashcode = entry.GetType().ToString().GetHashCode();
            var bytes = entry.Serialize();

            JSONNode node = new JSONObject();

            node["code"] = hashcode;
            JSONArray array = new JSONArray();

            foreach (var b in bytes)
            {
                array.Add(b);
            }

            node["data"] = array;

            EditorGUIUtility.systemCopyBuffer = node.ToString();
        }

        private void PasteEntryFromClipboard(TAssetResolver resolver)
        {
            string text = EditorGUIUtility.systemCopyBuffer;
            try
            {
                var node = JSON.Parse(text);

                var targetHash = typeof(TEntry).ToString().GetHashCode();
                var dataHash = node["code"].AsInt;

                if (targetHash != dataHash)
                {
                    Debug.LogError("Data in clipboard doesn't match expected type!");
                    return;
                }

                var dataArray = node["data"].AsArray;

                List<byte> bytes = new List<byte>();
                for (int i = 0; i < dataArray.Count; i++)
                    bytes.Add((byte)dataArray[i].AsInt);

                var entry = new TEntry();
                entry.Deserialize(bytes.ToArray());

                if (resolver.entries == null)
                    resolver.entries = new List<TEntry>();
                
                Undo.RecordObject(resolver, "Resolver entry added");
                
                resolver.entries.Add(entry);
                
                PrefabUtility.RecordPrefabInstancePropertyModifications(resolver);
            }
            catch 
            {
                Debug.LogWarning($"Couldn't parse data from clipboard!");
            }
            
        }

        private void PasteEntriesFromClipboard(TAssetResolver resolver)
        {
            string text = EditorGUIUtility.systemCopyBuffer;

            JSONArray array = (JSONArray)JSONNode.Parse(text);
                
            List<byte> bytes = new List<byte>();
            for (int i = 0; i < array.Count; i++)
                bytes.Add((byte)array[i].AsInt);
            
            Undo.RecordObject(resolver, "Data Override");

            resolver.serializedData = bytes.ToArray();
            resolver.OnAfterDeserialize();
            
            PrefabUtility.RecordPrefabInstancePropertyModifications(resolver);
        }

        private void PruneEntries(TAssetResolver resolver)
        {
            List<TEntry> entriesToPrune = new List<TEntry>();
            
            foreach (var entry in resolver.entries)
            {
                bool validTarget;
                try
                {
                    var target = entry.GetTarget(resolver.transform);

                    validTarget = target;

                }
                catch
                {
                    validTarget = false;
                }

                if (validTarget)
                    continue;
                
                entriesToPrune.Add(entry);
            }

            if (entriesToPrune.Count == 0)
                return;
            
            Undo.RecordObject(resolver, "Pruned Entries");

            foreach (var entry in entriesToPrune)
                resolver.entries.Remove(entry);
            
            PrefabUtility.RecordPrefabInstancePropertyModifications(resolver);
        }

        protected abstract void DrawEntry(TAssetResolver resolver, TEntry entry, int index);

        protected abstract void AddEntry(TAssetResolver resolver);

        protected abstract void ResolveAssets(TAssetResolver resolver);

        protected abstract void ResetAssets(TAssetResolver resolver);

        protected string GetTransformPath(Transform root, TComponent comp)
        {
            var transform = comp.gameObject.transform;

            if (transform.name == root.name)
                return "";

            string path = transform.name;
            
            transform = transform.parent;

            while (transform.name != root.name)
            {
                path = $"{transform.name}/{path}";
                
                transform = transform.parent;
            }

            return path;
        }
    }
}