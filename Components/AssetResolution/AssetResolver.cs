using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace RiskOfOptions.Components.AssetResolution
{
    public abstract class AssetResolver : MonoBehaviour
    {
        /// <summary>
        /// Because of a fun little <a href="http://issuetracker.unity3d.com/issues/assetbundle-is-not-loaded-correctly-when-they-reference-a-script-in-custom-dll-which-contains-system-dot-serializable-in-the-build"> bug </a>
        /// I have to serialize structs into bytes and load from there. Thanks Unity...
        /// </summary>
        [HideInInspector, SerializeField]
        public byte[] serializedData;
        
        public void Awake()
        {
            AttemptResolve();
        }

        public void Start()
        {
            AttemptResolve();
        }

        public void OnEnable()
        {
            AttemptResolve();
        }

        internal void AttemptResolve()
        {
            Resolve();
            
            DestroyImmediate(this);
        }

        protected abstract void Resolve();
    }
}