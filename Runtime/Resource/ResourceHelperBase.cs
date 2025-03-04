using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ZeroFramework
{
	public abstract class ResourceHelperBase : MonoBehaviour, IResourceHelper
	{
		public virtual void OnStart () { }

		public virtual void OnUpdate (float elapseSeconds, float realElapseSeconds) { }

		public virtual void OnShutdown () { }


		#region Sync load asset method

		public abstract T LoadAsset<T> (string location, string packageName = "") where T : Object;
		public abstract T LoadSubAsset<T> (string location, string subName, string packageName = "") where T : Object;
		public abstract Object[] LoadSubAssets (string location, string packageName = "");
		public abstract byte[] LoadRawFile (string location, string packageName = "");

		#endregion

		#region Async load asset method

		public abstract UniTask LoadAssetAsync<T> (string location, string packageName = "") where T : Object;
		public abstract UniTask LoadSubAssetAsync<T> (string location, string subName, string packageName = "") where T : Object;
		public abstract UniTask LoadSubAssetsAsync (string location, string packageName = "");
		public abstract UniTask LoadSceneAsync (string location, string packageName = "");
		public abstract UniTask LoadRawFileAsync (string location, string packageName = "");

		#endregion
	}
}