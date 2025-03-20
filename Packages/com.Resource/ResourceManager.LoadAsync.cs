using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

namespace ZeroFramework.Resource
{
	/// <summary>
	/// 异步加载接口
	/// </summary>
	public sealed partial class ResourceManager : GameFrameworkModule, IResourceManager
	{
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