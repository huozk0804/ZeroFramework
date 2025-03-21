//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

namespace ZeroFramework.Resource
{
	/// <summary>
	/// 资源管理器接口。
	/// </summary>
	public interface IResourceManager
	{
		/// <summary>
		/// 默认资源包
		/// </summary>
		ResourcePackage DefaultPackage { get; set; }

		/// <summary>
		/// 获取或设置运行模式。
		/// </summary>
		EPlayMode PlayMode { get; set; }

		/// <summary>
		/// UniTask取消句柄
		/// </summary>
		CancellationToken taskCancellationToken { get; set; }

		#region 资源生命周期管理

		/// <summary>
		/// 初始化接口。
		/// </summary>
		/// <param name="defaultPackageName">默认资源包名称</param>
		/// <param name="milliseconds">每帧用于资源加载的最大用时</param>
		void Initialize (string defaultPackageName);

		/// <summary>
		/// 初始化构建资源包
		/// </summary>
		/// <param name="playMode">资源包运行模式</param>
		/// <param name="packageName">资源包名</param>
		/// <returns>执行句柄</returns>
		UniTask<InitializationOperation> InitPackage (EPlayMode playMode, string packageName);

		/// <summary>
		/// 卸载指定资源。
		/// </summary>
		/// <param name="asset">要卸载的资源。</param>
		void UnloadAsset (object asset);

		/// <summary>
		/// 资源回收（卸载引用计数为零的资源）
		/// </summary>
		void UnloadUnusedAssets ();

		/// <summary>
		/// 强制回收所有资源
		/// </summary>
		void ForceUnloadAllAssets ();

		/// <summary>
		/// 检查资源是否存在。
		/// </summary>
		/// <param name="location">资源定位地址。</param>
		/// <param name="packageName">资源包名称。</param>
		/// <returns>检查资源是否存在的结果。</returns>
		public HasAssetResult HasAsset (string location, string packageName = "");

		/// <summary>
		/// 是否需要从远端更新下载。
		/// </summary>
		/// <param name="location">资源的定位地址。</param>
		/// <param name="packageName">资源包名称。</param>
		bool IsNeedDownloadFromRemote (string location, string packageName = "");

		/// <summary>
		/// 是否需要从远端更新下载。
		/// </summary>
		/// <param name="assetInfo">资源信息。</param>
		/// <param name="packageName">资源包名称。</param>
		public bool IsNeedDownloadFromRemote (AssetInfo assetInfo, string packageName = "");

		/// <summary>
		/// 检查资源定位地址是否有效。
		/// </summary>
		/// <param name="location">资源的定位地址</param>
		/// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
		bool CheckLocationValid (string location, string packageName = "");

		/// <summary>
		/// 获取资源信息列表。
		/// </summary>
		/// <param name="resTag">资源标签。</param>
		/// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
		/// <returns>资源信息列表。</returns>
		AssetInfo[] GetAssetInfos (string resTag, string packageName = "");

		/// <summary>
		/// 获取资源信息列表。
		/// </summary>
		/// <param name="tags">资源标签列表。</param>
		/// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
		/// <returns>资源信息列表。</returns>
		AssetInfo[] GetAssetInfos (string[] tags, string packageName = "");

		/// <summary>
		/// 获取资源信息。
		/// </summary>
		/// <param name="location">资源的定位地址。</param>
		/// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
		/// <returns>资源信息。</returns>
		AssetInfo GetAssetInfo (string location, string packageName = "");

		#endregion

		#region 同步资源加载接口

		/// <summary>
		/// 同步加载资源。
		/// </summary>
		/// <param name="location">资源的定位地址。</param>
		/// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
		/// <typeparam name="T">要加载资源的类型。</typeparam>
		/// <returns>资源实例。</returns>
		T LoadAsset<T> (string location, string packageName = "") where T : UnityEngine.Object;

		/// <summary>
		/// 同步加载游戏物体并实例化。
		/// </summary>
		/// <param name="location">资源的定位地址。</param>
		/// <param name="parent">资源实例父节点。</param>
		/// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
		/// <returns>资源实例。</returns>
		/// <remarks>会实例化资源到场景，无需主动UnloadAsset，Destroy时自动UnloadAsset。</remarks>
		GameObject LoadGameObject (string location, Transform parent = null, string packageName = "");

		/// <summary>
		/// 同步加载子资源。
		/// </summary>
		/// <param name="location">资源的定位地址。</param>
		/// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
		/// <typeparam name="T">要加载资源的类型。</typeparam>
		/// <returns>资源实例。</returns>
		T LoadSubAsset<T> (string location, string packageName = "") where T : UnityEngine.Object;

		/// <summary>
		/// 同步加载原生资源
		/// </summary>
		/// <param name="location">资源的定位地址。</param>
		/// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
		/// <returns>资源实例。</returns>
		byte[] LoadRawAsset (string location, string packageName = "");

		#endregion

		#region 异步资源加载接口

		/// <summary>
		/// 异步加载资源
		/// </summary>
		/// <param name="location">资源的定位地址</param>
		/// <param name="assetType">要加载的资源类型</param>
		/// <param name="loadAssetCallbacks">加载资源回调函数集</param>
		/// <param name="userData">用户自定义数据</param>
		/// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
		void LoadAssetAsync (string location, Type assetType, Action<UnityEngine.Object, string> callback, object userData, string packageName = "");

		/// <summary>
		/// 异步加载资源。
		/// </summary>
		/// <param name="location">资源的定位地址。</param>
		/// <param name="callback">回调函数。</param>
		/// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
		/// <typeparam name="T">要加载资源的类型。</typeparam>
		UniTask<T> LoadAssetAsync<T> (string location, Action<T, string> callback, object userData, string packageName = "") where T : UnityEngine.Object;

		/// <summary>
		/// 异步加载子资源对象。
		/// </summary>
		/// <param name="location">资源的定位地址。</param>
		/// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
		/// <typeparam name="T">要加载资源的类型。</typeparam>
		UniTask<T> LoadSubAssetAsync<T> (string location, string packageName = "") where T : UnityEngine.Object;

		/// <summary>
		/// 异步加载资源。
		/// </summary>
		/// <param name="location">资源定位地址。</param>
		/// <param name="cancellationToken">取消操作Token。</param>
		/// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
		/// <typeparam name="T">要加载资源的类型。</typeparam>
		/// <returns>异步资源实例。</returns>
		UniTask<T> LoadAssetAsync<T> (string location, CancellationToken cancellationToken = default,
			string packageName = "") where T : UnityEngine.Object;

		/// <summary>
		/// 异步加载游戏物体并实例化。
		/// </summary>
		/// <param name="location">资源定位地址。</param>
		/// <param name="parent">资源实例父节点。</param>
		/// <param name="cancellationToken">取消操作Token。</param>
		/// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
		/// <returns>异步游戏物体实例。</returns>
		/// <remarks>会实例化资源到场景，无需主动UnloadAsset，Destroy时自动UnloadAsset。</remarks>
		UniTask<GameObject> LoadGameObjectAsync (string location, Transform parent = null, CancellationToken cancellationToken = default, string packageName = "");

		/// <summary>
		/// 异步加载原生资源。
		/// </summary>
		/// <param name="location">资源定位地址。</param>
		/// <param name="cancellationToken">取消操作Token。</param>
		/// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
		/// <typeparam name="T">要加载资源的类型。</typeparam>
		/// <returns>异步资源实例。</returns>
		UniTask<byte[]> LoadRawAssetAsync (string location, CancellationToken cancellationToken = default, string packageName = "");

		/// <summary>
		/// 异步加载场景。
		/// </summary>
		/// <param name="location">资源定位地址。</param>
		/// <param name="sceneMode">资源加载方式</param>
		/// <param name="cancellationToken">取消操作Token。</param>
		/// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
		/// <typeparam name="T">要加载资源的类型。</typeparam>
		/// <returns>异步资源实例。</returns>
		UniTask<byte[]> LoadSceneAsync (string location, LoadSceneMode sceneMode, CancellationToken cancellationToken = default, string packageName = "");

		#endregion
	}
}