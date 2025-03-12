using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace ZeroFramework
{
	public partial class ResourceManager : GameFrameworkModule, IResourceManager
	{
		private int _downloadingMaxNum = 10;
		private float _minUnloadUnusedAssetsInterval = 60f;
		private float _maxUnloadUnusedAssetsInterval = 300f;
		private bool _useSystemUnloadUnusedAssets = true;
		private int _assetCapacity = 64;
		private float _assetExpireTime = 60f;
		private int _assetPriority = 0;

		private Dictionary<string, ResourcePackage> _packageMap = new Dictionary<string, ResourcePackage>();
		private readonly Dictionary<string, AssetInfo> _assetInfoMap = new Dictionary<string, AssetInfo>();
		private readonly HashSet<string> _assetLoadingList = new HashSet<string>();

		/// <summary>
		/// 当前最新的包裹版本。
		/// </summary>
		public string PackageVersion { set; get; }

		/// <summary>
		/// 资源包名称。
		/// </summary>
		public string PackageName = "DefaultPackage";

		/// <summary>
		/// 设置异步系统参数，每帧执行消耗的最大时间切片（单位：毫秒）
		/// </summary>
		[SerializeField] public long Milliseconds = 30;

		/// <summary>
		/// 获取或设置同时最大下载数目。
		/// </summary>
		public int DownloadingMaxNum {
			get => _downloadingMaxNum;
			set => _downloadingMaxNum = value;
		}

		/// <summary>
		/// 获取当前资源适用的游戏版本号。
		/// </summary>
		public string ApplicableGameVersion { get; private set; }

		/// <summary>
		/// 获取当前内部资源版本号。
		/// </summary>
		public int InternalResourceVersion { get; private set; }

		/// <summary>
		/// 获取或设置无用资源释放的最小间隔时间，以秒为单位。
		/// </summary>
		public float MinUnloadUnusedAssetsInterval {
			get => _minUnloadUnusedAssetsInterval;
			set => _minUnloadUnusedAssetsInterval = value;
		}

		/// <summary>
		/// 获取或设置无用资源释放的最大间隔时间，以秒为单位。
		/// </summary>
		public float MaxUnloadUnusedAssetsInterval {
			get => _maxUnloadUnusedAssetsInterval;
			set => _maxUnloadUnusedAssetsInterval = value;
		}

		/// <summary>
		/// 使用系统释放无用资源策略。
		/// </summary>
		public bool UseSystemUnloadUnusedAssets {
			get => _useSystemUnloadUnusedAssets;
			set => _useSystemUnloadUnusedAssets = value;
		}

		/// <summary>
		/// 获取游戏框架模块优先级。
		/// </summary>
		/// <remarks>优先级较高的模块会优先轮询，并且关闭操作会后进行。</remarks>
		public override int Priority => 4;

		public ResourceManager () {
			SetObjectPoolManager(Zero.Instance.ObjectPool);
		}

		public override void Shutdown () {
			_packageMap.Clear();
			_assetPool = null;
			_assetLoadingList.Clear();
			_assetInfoMap.Clear();
		}

		public override void Update (float elapseSeconds, float realElapseSeconds) {
		}

		/// <summary>
		/// 是否需要从远端更新下载。
		/// </summary>
		/// <param name="location">资源的定位地址。</param>
		/// <param name="packageName">资源包名称。</param>
		public bool IsNeedDownloadFromRemote (string location, string packageName = "") {
			return false;
		}

		/// <summary>
		/// 获取资源信息。
		/// </summary>
		/// <param name="location">资源的定位地址。</param>
		/// <param name="packageName">资源包名称。</param>
		/// <returns>资源信息。</returns>
		public AssetInfo GetAssetInfo (string location, string packageName = "") {
			return null;
		}

		/// <summary>
		/// 检查资源是否存在。
		/// </summary>
		/// <param name="location">资源定位地址。</param>
		/// <param name="packageName">资源包名称。</param>
		/// <returns>检查资源是否存在的结果。</returns>
		public HasAssetResult HasAsset (string location, string packageName = "") {
			return HasAssetResult.NotExist;
		}

		/// <summary>
		/// 同步获取Unity资源
		/// </summary>
		/// <typeparam name="T">资源类型</typeparam>
		/// <param name="location">资源定位地址</param>
		/// <param name="packageName">资源包名称</param>
		/// <returns>获取的资源</returns>
		public T LoadAsset<T> (string location, string packageName = "") where T : UnityEngine.Object {
			return null;
		}

		/// <summary>
		/// 同步获取原生资源
		/// </summary>
		/// <typeparam name="T">资源类型</typeparam>
		/// <param name="location">资源定位地址</param>
		/// <param name="packageName">资源包名称</param>
		/// <returns>获取的资源</returns>
		public byte[] LoadRawAsset (string location, string packageName = "") {
			return null;
		}

		/// <summary>
		/// 异步加载资源。
		/// </summary>
		/// <param name="location">资源的定位地址。</param>
		/// <param name="callback">回调函数。</param>
		/// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
		/// <typeparam name="T">要加载资源的类型。</typeparam>
		public async UniTaskVoid LoadAssetAsync<T> (string location, Action<T> callback, string packageName = "") where T : UnityEngine.Object {

		}

		public async UniTaskVoid LoadRawAsset(string location, string packageName = "") {

		}

		public async UniTaskVoid LoadSceneAsync(string location, string packageName = "") {
		
		}
	}
}