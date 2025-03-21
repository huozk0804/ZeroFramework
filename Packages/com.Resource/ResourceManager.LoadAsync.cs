using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

namespace ZeroFramework.Resource
{
	/// <summary>
	/// 异步加载接口
	/// </summary>
	public sealed partial class ResourceManager
	{
		public void LoadAssetAsync (string location, Type assetType, Action<UnityEngine.Object, string> callback, object userData, string packageName = "") {
			throw new NotImplementedException();
		}

		public UniTask<T> LoadAssetAsync<T> (string location, Action<T, string> callback, object userData, string packageName = "") where T : UnityEngine.Object {
			throw new NotImplementedException();
		}

		public UniTask<T> LoadSubAssetAsync<T> (string location, string packageName = "") where T : UnityEngine.Object {
			throw new NotImplementedException();
		}

		public UniTask<T> LoadAssetAsync<T> (string location, CancellationToken cancellationToken = default, string packageName = "") where T : UnityEngine.Object {
			throw new NotImplementedException();
		}

		public UniTask<GameObject> LoadGameObjectAsync (string location, Transform parent = null, CancellationToken cancellationToken = default, string packageName = "") {
			throw new NotImplementedException();
		}

		public UniTask<byte[]> LoadRawAssetAsync (string location, CancellationToken cancellationToken = default, string packageName = "") {
			throw new NotImplementedException();
		}

		public UniTask<byte[]> LoadSceneAsync (string location, LoadSceneMode sceneMode, CancellationToken cancellationToken = default, string packageName = "") {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 获取异步资源句柄。
		/// </summary>
		/// <param name="location">资源定位地址</param>
		/// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
		/// <typeparam name="T">资源类型</typeparam>
		/// <returns>资源句柄。</returns>
		private AssetHandle GetHandleAsync<T> (string location, string packageName = "") where T : UnityEngine.Object {
			return GetHandleAsync(location, typeof(T), packageName);
		}

		/// <summary>
		/// 获取异步资源句柄。
		/// </summary>
		/// <param name="location">资源定位地址</param>
		/// <param name="assetType">资源类型</param>
		/// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
		/// <returns></returns>
		private AssetHandle GetHandleAsync (string location, Type assetType, string packageName = "") {
			if (string.IsNullOrEmpty(packageName)) {
				return YooAssets.LoadAssetAsync(location, assetType);
			}

			var package = YooAssets.GetPackage(packageName);
			return package.LoadAssetAsync(location, assetType);
		}
	}
}