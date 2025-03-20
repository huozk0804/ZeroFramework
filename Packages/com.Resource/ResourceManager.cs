//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

namespace ZeroFramework.Resource
{
	/// <summary>
	/// 资源管理器。
	/// </summary>
	public sealed partial class ResourceManager : GameFrameworkModule, IResourceManager
	{
		/// <summary>
		/// 默认资源包名称。
		/// </summary>
		public string DefaultPackageName { get; set; }

		/// <summary>
		/// 默认资源包
		/// </summary>
		public ResourcePackage DefaultPackage { get; set; }

		/// <summary>
		/// 获取或设置运行模式。
		/// </summary>
		public EPlayMode PlayMode { get; set; }

		/// <summary>
		/// 缓存系统启动时的验证级别。
		/// </summary>
		public EFileVerifyLevel VerifyLevel { get; set; }

		/// <summary>
		/// 获取或设置异步系统参数，每帧执行消耗的最大时间切片（单位：毫秒）。
		/// </summary>
		public long Milliseconds { get; set; }

		/// <summary>
		/// 同时下载的最大数目。
		/// </summary>
		public int DownloadingMaxNum { get; set; }

		/// <summary>
		/// 失败重试最大数目。
		/// </summary>
		public int FailedTryAgain { get; set; }

		/// <summary>
		/// UniTask取消句柄
		/// </summary>
		public CancellationToken taskCancellationToken { get; set; }

		/// <summary>
		/// 获取游戏框架模块优先级。
		/// </summary>
		/// <remarks>优先级较高的模块会优先轮询，并且关闭操作会后进行。</remarks>
		protected internal override int Priority => 4;

		private Dictionary<string, ResourcePackage> _packageMap;
		private readonly Dictionary<string, AssetInfo> _assetInfoMap;
		private readonly HashSet<string> _assetLoadingList;
		private readonly TimeoutController _timeoutController;

		public ResourceManager () {
			_assetLoadingList = new HashSet<string>();
			_assetInfoMap = new Dictionary<string, AssetInfo>();
			_packageMap = new Dictionary<string, ResourcePackage>();
			_timeoutController = new TimeoutController();

			SetObjectPoolManager(Zero.objectPool);
		}

		protected internal override void Update (float elapseSeconds, float realElapseSeconds) {
		}

		protected internal override async void Shutdown () {
			foreach (var package in _packageMap.Values) {
				await package.ClearCacheFilesAsync(EFileClearMode.ClearAllBundleFiles);
				await package.ClearCacheFilesAsync(EFileClearMode.ClearAllManifestFiles);
			}

			_packageMap.Clear();
			_assetPool = null;
			_assetLoadingList.Clear();
			_assetInfoMap.Clear();
		}

		/// <summary>
		/// 初始化接口。
		/// </summary>
		/// <param name="defaultPackageName">默认资源包名称</param>
		/// <param name="milliseconds">每帧用于资源加载的最大用时</param>
		public void Initialize (string defaultPackageName = null, long milliseconds = 10) {
			if (!YooAssets.Initialized) {
				YooAssets.Initialize(new ResourceLogger());
			}

			Milliseconds = milliseconds;
			YooAssets.SetOperationSystemMaxTimeSlice(Milliseconds);

			if (!defaultPackageName.IsNullOrEmpty())
				DefaultPackageName = defaultPackageName;
			var defaultPackage = YooAssets.TryGetPackage(DefaultPackageName);
			if (defaultPackage == null) {
				defaultPackage = YooAssets.CreatePackage(DefaultPackageName);
				YooAssets.SetDefaultPackage(defaultPackage);
				DefaultPackage = defaultPackage;
			}
		}

		/// <summary>
		/// 初始化构建资源包
		/// </summary>
		/// <param name="playMode">资源包运行模式</param>
		/// <param name="packageName">资源包名</param>
		/// <returns>执行句柄</returns>
		public async UniTask<InitializationOperation> InitPackage (EPlayMode playMode, string packageName) {
			if (_packageMap.ContainsKey(packageName)) {
				Log.Error($"ResourceManager has already init package : {packageName}");
				return null;
			}

			var package = YooAssets.TryGetPackage(packageName);
			if (package == null) {
				package = YooAssets.CreatePackage(packageName);
			}

			_packageMap[packageName] = package;
			string defaultHostServer = "http://127.0.0.1/CDN/Android/v1.0";
			string fallbackHostServer = "http://127.0.0.1/CDN/Android/v1.0";

			// 编辑器下的模拟模式
			InitializationOperation initializationOperation = null;
			if (playMode == EPlayMode.EditorSimulateMode) {
				var buildResult = EditorSimulateModeHelper.SimulateBuild(packageName);
				var packageRoot = buildResult.PackageRootDirectory;
				var editorFileSystemParams = FileSystemParameters.CreateDefaultEditorFileSystemParameters(packageRoot);
				var initParameters = new EditorSimulateModeParameters {
					EditorFileSystemParameters = editorFileSystemParams
				};
				initializationOperation = package.InitializeAsync(initParameters);
			}

			// 单机运行模式
			if (playMode == EPlayMode.OfflinePlayMode) {
				var buildinFileSystemParams = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
				var initParameters = new OfflinePlayModeParameters {
					BuildinFileSystemParameters = buildinFileSystemParams
				};
				initializationOperation = package.InitializeAsync(initParameters);
			}

			// 联机运行模式
			if (playMode == EPlayMode.HostPlayMode) {
				IRemoteServices remoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
				var cacheFileSystemParams = FileSystemParameters.CreateDefaultCacheFileSystemParameters(remoteServices);
				var buildinFileSystemParams = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();

				var initParameters = new HostPlayModeParameters {
					BuildinFileSystemParameters = buildinFileSystemParams,
					CacheFileSystemParameters = cacheFileSystemParams
				};
				initializationOperation = package.InitializeAsync(initParameters);
			}

			// WebGL运行模式
			if (playMode == EPlayMode.WebPlayMode) {
				IRemoteServices remoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
				var webServerFileSystemParams = FileSystemParameters.CreateDefaultWebServerFileSystemParameters();
				var webRemoteFileSystemParams =
					FileSystemParameters.CreateDefaultWebRemoteFileSystemParameters(remoteServices); //支持跨域下载

				var initParameters = new WebPlayModeParameters {
					WebServerFileSystemParameters = webServerFileSystemParams,
					WebRemoteFileSystemParameters = webRemoteFileSystemParams
				};

				initializationOperation = package.InitializeAsync(initParameters);
			}

			await initializationOperation.ToUniTask();
			Log.Info($"Init resource package version : {initializationOperation.PackageName}");

			return initializationOperation;
		}

		public void UnloadUnusedAssets () {
			_assetPool.ReleaseAllUnused();
			foreach (var package in _packageMap.Values) {
				if (package is { InitializeStatus: EOperationStatus.Succeed }) {
					package.UnloadUnusedAssetsAsync();
				}
			}
		}

		public void ForceUnloadAllAssets () {
#if UNITY_WEBGL
            Log.Warning($"WebGL not support invoke {nameof(ForceUnloadAllAssets)}");
			return;
#else

			foreach (var package in _packageMap.Values) {
				if (package is { InitializeStatus: EOperationStatus.Succeed }) {
					package.UnloadAllAssetsAsync();
				}
			}
#endif
		}

		/// <summary>
		/// 是否需要从远端更新下载。
		/// </summary>
		/// <param name="location">资源的定位地址。</param>
		/// <param name="packageName">资源包名称。</param>
		public bool IsNeedDownloadFromRemote (string location, string packageName = "") {
			if (string.IsNullOrEmpty(packageName)) {
				return YooAssets.IsNeedDownloadFromRemote(location);
			} else {
				var package = YooAssets.GetPackage(packageName);
				return package.IsNeedDownloadFromRemote(location);
			}
		}

		/// <summary>
		/// 是否需要从远端更新下载。
		/// </summary>
		/// <param name="assetInfo">资源信息。</param>
		/// <param name="packageName">资源包名称。</param>
		public bool IsNeedDownloadFromRemote (AssetInfo assetInfo, string packageName = "") {
			if (string.IsNullOrEmpty(packageName)) {
				return YooAssets.IsNeedDownloadFromRemote(assetInfo);
			} else {
				var package = YooAssets.GetPackage(packageName);
				return package.IsNeedDownloadFromRemote(assetInfo);
			}
		}

		/// <summary>
		/// 检查资源定位地址是否有效。
		/// </summary>
		/// <param name="location">资源的定位地址</param>
		/// <param name="packageName">资源包名称。</param>
		public bool CheckLocationValid (string location, string packageName = "") {
			if (string.IsNullOrEmpty(packageName)) {
				return YooAssets.CheckLocationValid(location);
			} else {
				var package = YooAssets.GetPackage(packageName);
				return package.CheckLocationValid(location);
			}
		}

		/// <summary>
		/// 获取资源信息列表。
		/// </summary>
		/// <param name="tag">资源标签。</param>
		/// <param name="packageName">资源包名称。</param>
		/// <returns>资源信息列表。</returns>
		public AssetInfo[] GetAssetInfos (string tag, string packageName = "") {
			if (string.IsNullOrEmpty(packageName)) {
				return YooAssets.GetAssetInfos(tag);
			} else {
				var package = YooAssets.GetPackage(packageName);
				return package.GetAssetInfos(tag);
			}
		}

		/// <summary>
		/// 获取资源信息列表。
		/// </summary>
		/// <param name="tags">资源标签列表。</param>
		/// <param name="packageName">资源包名称。</param>
		/// <returns>资源信息列表。</returns>
		public AssetInfo[] GetAssetInfos (string[] tags, string packageName = "") {
			if (string.IsNullOrEmpty(packageName)) {
				return YooAssets.GetAssetInfos(tags);
			} else {
				var package = YooAssets.GetPackage(packageName);
				return package.GetAssetInfos(tags);
			}
		}

		/// <summary>
		/// 获取资源信息。
		/// </summary>
		/// <param name="location">资源的定位地址。</param>
		/// <param name="packageName">资源包名称。</param>
		/// <returns>资源信息。</returns>
		public AssetInfo GetAssetInfo (string location, string packageName = "") {
			if (string.IsNullOrEmpty(location)) {
				throw new GameFrameworkException("Asset name is invalid.");
			}

			if (string.IsNullOrEmpty(packageName)) {
				if (_assetInfoMap.TryGetValue(location, out AssetInfo assetInfo)) {
					return assetInfo;
				}

				assetInfo = YooAssets.GetAssetInfo(location);
				_assetInfoMap[location] = assetInfo;
				return assetInfo;
			} else {
				string key = $"{packageName}/{location}";
				if (_assetInfoMap.TryGetValue(key, out AssetInfo assetInfo)) {
					return assetInfo;
				}

				var package = YooAssets.GetPackage(packageName);
				if (package == null) {
					throw new GameFrameworkException($"The package does not exist. Package Name :{packageName}");
				}

				assetInfo = package.GetAssetInfo(location);
				_assetInfoMap[key] = assetInfo;
				return assetInfo;
			}
		}

		/// <summary>
		/// 检查资源是否存在。
		/// </summary>
		/// <param name="location">资源定位地址。</param>
		/// <param name="packageName">资源包名称。</param>
		/// <returns>检查资源是否存在的结果。</returns>
		public HasAssetResult HasAsset (string location, string packageName = "") {
			if (string.IsNullOrEmpty(location)) {
				throw new GameFrameworkException("Asset name is invalid.");
			}

			AssetInfo assetInfo = GetAssetInfo(location, packageName);

			if (!CheckLocationValid(location)) {
				return HasAssetResult.Valid;
			}

			if (assetInfo == null) {
				return HasAssetResult.NotExist;
			}

			if (IsNeedDownloadFromRemote(assetInfo)) {
				return HasAssetResult.AssetOnline;
			}

			return HasAssetResult.AssetOnDisk;
		}

		/// <summary>
		/// 获取资源定位地址的缓存Key。
		/// </summary>
		/// <param name="location">资源定位地址。</param>
		/// <param name="packageName">资源包名称。</param>
		/// <returns>资源定位地址的缓存Key。</returns>
		private string GetCacheKey (string location, string packageName = "") {
			if (string.IsNullOrEmpty(packageName) || packageName.Equals(DefaultPackageName)) {
				return location;
			}

			return $"{packageName}/{location}";
		}

		private async UniTaskVoid InvokeProgress (string location, AssetHandle assetHandle,
			LoadAssetUpdateCallback loadAssetUpdateCallback, object userData) {
			if (string.IsNullOrEmpty(location)) {
				throw new GameFrameworkException("Asset name is invalid.");
			}

			if (loadAssetUpdateCallback != null) {
				while (assetHandle is { IsValid: true, IsDone: false }) {
					await UniTask.Yield();

					loadAssetUpdateCallback.Invoke(location, assetHandle.Progress, userData);
				}
			}
		}

		private async UniTask TryWaitingLoading (string assetObjectKey) {
			if (_assetLoadingList.Contains(assetObjectKey)) {
				try {
					await UniTask.WaitUntil(() => !_assetLoadingList.Contains(assetObjectKey))
#if UNITY_EDITOR
						.AttachExternalCancellation(_timeoutController.Timeout(TimeSpan.FromSeconds(60)));
					_timeoutController.Reset();
#else
                    ;
#endif
				} catch (OperationCanceledException ex) {
					if (_timeoutController.IsTimeout()) {
						Log.Error($"LoadAssetAsync Waiting {assetObjectKey} timeout. reason:{ex.Message}");
					}
				}
			}
		}
	}
}