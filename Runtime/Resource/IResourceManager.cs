using System;
using Cysharp.Threading.Tasks;

namespace ZeroFramework
{
	public interface IResourceManager
	{
		T LoadAsset<T> (string location, string packageName = "") where T : UnityEngine.Object;
		T LoadSubAsset<T> (string location, string subName, string packageName = "") where T : UnityEngine.Object;
		UnityEngine.Object LoadAsset (string location, Type type, string packageName = "");
		UnityEngine.Object[] LoadSubAssets (string location, string packageName = "");
		byte[] LoadRawFile (string location, string packageName = "");

		UniTask<T> LoadAssetAsync<T> (string location, string packageName = "") where T : UnityEngine.Object;
		UniTask<T> LoadSubAssetAsync<T> (string location, string subName, string packageName = "") where T : UnityEngine.Object;
		UniTask LoadSubAssetsAsync (string location, string packageName = "");
		UniTask LoadSceneAsync (string location, string packageName = "");
		UniTask LoadRawFileAsync (string location, string packageName = "");
	}
}