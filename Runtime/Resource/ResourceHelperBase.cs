using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ZeroFramework.Resource
{
    public abstract class ResourceHelperBase : IResourceHelper
    {
        public abstract T LoadAsset<T>(string location, string packageName = "") where T : Object;
		public abstract T LoadSubAsset<T> (string location, string subName, string packageName = "") where T : Object;
		public abstract Object[] LoadSubAssets(string location, string packageName = "");
        public abstract byte[] LoadRawFile(string location, string packageName = "");

        public abstract UniTask LoadAssetAsync<T>(string location, string packageName = "") where T : Object;
		public abstract UniTask LoadSubAssetAsync<T> (string location, string subName, string packageName = "") where T : Object;
		public abstract UniTask LoadSubAssetsAsync(string location, string packageName = "");
        public abstract UniTask LoadSceneAsync(string location, string packageName = "");
        public abstract UniTask LoadRawFileAsync(string location, string packageName = "");
		
	}
}