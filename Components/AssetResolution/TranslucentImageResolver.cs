using LeTai.Asset.TranslucentImage;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RiskOfOptions.Components.AssetResolution
{
    public class TranslucentImageResolver : AssetResolver
    {
        private const string Path = "TranslucentImage/Default-Translucent.mat";

        public TranslucentImage image;
        
        protected override void Resolve()
        {
            image.material = Addressables.LoadAssetAsync<Material>(Path).WaitForCompletion();

            image.enabled = true;
        }
    }
}