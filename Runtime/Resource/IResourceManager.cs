using System;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace ZeroFramework
{
	public interface IResourceManager
	{
		HasAssetResult HasAsset (string assetName);

		T LoadAsset<T> (string location, string packageName = "") where T : Object;
		Object LoadAsset (string location, Type type, string packageName = "");
		byte[] LoadRawFile (string location, string packageName = "");

		UniTask<T> LoadAssetAsync<T> (string location, string packageName = "") where T : Object;
		UniTask LoadSceneAsync (string location, string packageName = "");
		UniTask LoadRawFileAsync (string location, string packageName = "");

		void UnloadAsset (object asset);
	}
}